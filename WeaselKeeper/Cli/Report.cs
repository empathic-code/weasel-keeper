using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WeaselKeeper.Analysis;
using WeaselKeeper.Config;

namespace WeaselKeeper.Cli
{
    internal class Report
    {
        public static void MethodName(Snippet snippet)
        {
            Console.WriteLine("Method Name: {0}", snippet.MethodName);
        }

        public static void CountTokens(Snippet snippet)
        {
            Console.WriteLine("Tokens: {0}", snippet.Tokens.Count());
        }

        public static void CountIdenfifiers(Snippet snippet)
        {
            Console.WriteLine("Identifiers: {0}", snippet.Identifiers.Count());
        }

        public static void CountUnique(Snippet snippet)
        {
            Console.WriteLine("Unique Identifiers: {0}", snippet.Identifiers.Distinct(Identifier.CompareByName).Count());
        }

        public static void CountDomain(Snippet snippet, Blacklist blacklist)
        {
            int count = snippet
                .Identifiers
                .Distinct(Identifier.CompareByName)
                .Count(token => !blacklist.Contains(token.ToString()));
            Console.WriteLine("Domain Identifiers: {0}", count);
        }

        public static void CountLines(Snippet snippet)
        {
            Console.WriteLine("LOC: {0}", snippet.LinesOfCode);
        }

        public static void ListIdentifiers(Snippet snippet)
        {
            ListTokens(snippet.Identifiers);
        }

        public static void ListUniqueIdentifiers(Snippet snippet)
        {
            ListTokens(snippet.Identifiers.Distinct(Identifier.CompareByName));
        }

        public static void ListDomain(Snippet snippet, Blacklist blacklist)
        {
            ListTokens(snippet.Identifiers.Distinct(Identifier.CompareByName).Where(token => !blacklist.Contains(token.ToString())));
        }

        public static void ListTokens(Snippet snippet)
        {
            ListTokens(snippet.Tokens);
        }

        private static void ListTokens(IEnumerable<SyntaxToken> tokens)
        {
            int index = 1;
            foreach (SyntaxToken syntaxToken in tokens)
            {
                string content = syntaxToken.ToString();
                string token = syntaxToken.CSharpKind().ToString();
                token = RemoveTokenSuffix(token);
                Console.WriteLine("{0}: {1} ({2})", index, content, token);
                index++;
            }
        }

        private static string RemoveTokenSuffix(string token)
        {
            if (token.Contains("Token"))
            {
                token = token.Remove(token.IndexOf("Token", StringComparison.Ordinal), "Token".Length);
            }
            return token;
        }

        private class Identifier : IEqualityComparer<SyntaxToken>
        {
            private Identifier() { }
            public static Identifier CompareByName { get { return new Identifier();  } }

            public bool Equals(SyntaxToken x, SyntaxToken y)
            {
                return string.Equals(x.ToString(), y.ToString());
            }

            public int GetHashCode(SyntaxToken token)
            {
                return token.ToString().GetHashCode();
            }
        }
    }
}