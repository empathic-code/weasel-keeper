using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Analysis
{
    internal class Snippet
    {
        protected Snippet(CompilationUnitSyntax root, IEnumerable<SyntaxToken> tokens)
        {
            Root = root;
            Tokens = tokens;
        }

        public CompilationUnitSyntax Root { get; private set; }
        public IEnumerable<SyntaxToken> Tokens { get; private set; }

        public string MethodName
        {
            get
            {
                MethodDeclarationSyntax methodDeclaration = FindFirstMethodDeclaration(Tokens);
                SyntaxToken methodName = FindFirstIdentifierInDeclaration(methodDeclaration);
                return methodName.ValueText;
            }
        }

        public string HeaderComments
        {
            get { return Root.GetLeadingTrivia().ToFullString().Trim(); }
        }

        public IEnumerable<SyntaxToken> Identifiers
        {
            get { return Tokens.Where(t => t.CSharpKind() == SyntaxKind.IdentifierToken); }
        }

        public int LinesOfCode
        {
            get { return Root.ToString().Trim().Split('\n').Length; }
        }

        public ReplacedSnippet ReplaceIdentifiers(Condition condition)
        {
            CompilationUnitSyntax code = Root.ReplaceTokens(Identifiers, condition.Replace);
            var replaceIdentifiers = new ReplacedSnippet(code, code.DescendantTokens(), condition.Map);
            return replaceIdentifiers;
        }

        public static Snippet Parse(string sourceCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            var root = (CompilationUnitSyntax) tree.GetRoot();
            IEnumerable<SyntaxToken> tokens = root.DescendantTokens();
            return new Snippet(root, tokens);
        }

        private static SyntaxToken FindFirstIdentifierInDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            return methodDeclaration.ChildTokens().First(t => t.CSharpKind() == SyntaxKind.IdentifierToken);
        }

        private MethodDeclarationSyntax FindFirstMethodDeclaration(IEnumerable<SyntaxToken> tokens)
        {
            var methodDeclaration =
                (MethodDeclarationSyntax)
                    tokens.First(token => token.Parent.CSharpKind() == SyntaxKind.MethodDeclaration).Parent;
            return methodDeclaration;
        }

        public override string ToString()
        {
            return Root.ToString();
        }
    }
}