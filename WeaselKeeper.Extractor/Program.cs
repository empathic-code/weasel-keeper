using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WeaselKeeper.Config;

namespace WeaselKeeper.Extractor
{
    internal class Program
    {
        private static string _snippetSourcePath = "Program.cs";
        private static string _snippetTargetDirectory = ".";

        /// <summary>
        ///     Takes a file that contains a namespace, a class and then extracts all the methods in the class into individual,
        ///     contextless files.
        ///     The code will be unindented, so that the method will be placed into the file in isolation.
        /// </summary>
        private static void Main(string[] args)
        {
            TerminateIfArgumentsMissing(args);
            SplitMethodsIntoIndividualFiles(args);
        }

        private static void SplitMethodsIntoIndividualFiles(string[] args)
        {
            IEnumerable<SyntaxNode> tokens = ParseSnippetSourceCodeFile(args);
            IEnumerable<MethodDeclarationSyntax> methods = SieveMethods(tokens, Blacklist());
            foreach (MethodDeclarationSyntax method in methods)
            {
                CleanAndExtract(method);
            }
        }

        private static void TerminateIfArgumentsMissing(string[] args)
        {
            if (args.Length == 2)
            {
                _snippetSourcePath = args[0];
                _snippetTargetDirectory = args[1];
                return;
            }
            PrintUsageAndExit();
        }

        private static void PrintUsageAndExit()
        {
            string exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Usage: {0} input.cs targetDirectory", exeName);
            Environment.Exit(0);
        }

        private static IEnumerable<SyntaxNode> ParseSnippetSourceCodeFile(string[] commandLineParameters)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(_snippetSourcePath));
            var root = (CompilationUnitSyntax) tree.GetRoot();
            IEnumerable<SyntaxNode> tokens = root.DescendantNodes();
            return tokens;
        }

        private static Blacklist Blacklist()
        {
            try
            {
                const string blacklistFile = "method-blacklist.txt";
                Blacklist blacklist = Config.Blacklist.ReadFromFile(blacklistFile);
                return blacklist;
            }
            catch (NotPlacedLocallyComplaint ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            return new Blacklist();
        }

        private static IEnumerable<MethodDeclarationSyntax> SieveMethods(IEnumerable<SyntaxNode> tokens,
            Blacklist blacklist)
        {
            return AllMethods(tokens).Where(NameNotOn(blacklist));
        }

        private static IEnumerable<MethodDeclarationSyntax> AllMethods(IEnumerable<SyntaxNode> tokens)
        {
            return tokens
                .Where(IsMethod)
                .Select(AsMethod);
        }

        private static bool IsMethod(SyntaxNode token)
        {
            return token.CSharpKind() == SyntaxKind.MethodDeclaration;
        }

        private static MethodDeclarationSyntax AsMethod(SyntaxNode node)
        {
            return (MethodDeclarationSyntax) node;
        }

        private static Func<MethodDeclarationSyntax, bool> NameNotOn(Blacklist blacklist)
        {
            return method => !blacklist.Contains(method.Identifier.Text);
        }

        private static void CleanAndExtract(MethodDeclarationSyntax method)
        {
            string snippet = UnindentMethod(method);
            string targetPath = FileName(method);
            WriteCodeToFile(snippet, targetPath);
        }

        private static string UnindentMethod(SyntaxNode method)
        {
            string[] lines = method.GetText().ToString().Split('\n');
            int indentation = MinimumIndentation(lines);
            string code = RemoveIndentation(lines, indentation);
            return code;
        }

        private static int MinimumIndentation(string[] lines)
        {
            return lines
                .Where(NotEmpty)
                .Select(NumberOfLeadingWhitespace)
                .Min();
        }

        private static bool NotEmpty(string line)
        {
            return !string.IsNullOrWhiteSpace(line.Trim());
        }

        private static int NumberOfLeadingWhitespace(string line)
        {
            return line.Length - line.TrimStart().Length;
        }

        private static string RemoveIndentation(string[] lines, int indentation)
        {
            var code = new StringBuilder();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.Trim()))
                {
                    code.AppendLine();
                    continue;
                }
                string unindentedLine = line.Remove(0, indentation);
                code.Append(unindentedLine);
            }
            return code.ToString().Trim();
        }

        private static string FileName(MethodDeclarationSyntax method)
        {
            string name = method.Identifier.ToString().ToLower();
            string targetPath = string.Format("{0}.cs", name);
            return targetPath;
        }

        private static void WriteCodeToFile(string code, string snippetTargetPath)
        {
            string path = Path.Combine(_snippetTargetDirectory, snippetTargetPath);
            using (StreamWriter file = File.CreateText(path))
            {
                file.Write(code);
                Console.WriteLine("Wrote {0}", snippetTargetPath);
            }
        }
    }
}