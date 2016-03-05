using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Analysis
{
    internal class ReplacedSnippet : Snippet
    {
        private readonly ReplacementMap _map;

        internal ReplacedSnippet(CompilationUnitSyntax root, IEnumerable<SyntaxToken> tokens, ReplacementMap map)
            : base(root, tokens)
        {
            _map = map;
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendNewLine(Documentation.From(HeaderComments, _map))
                .AppendNewLine(base.ToString())
                .ToString()
                .Trim();
        }
    }

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendNewLine(this StringBuilder builder, string str)
        {
            return builder.AppendFormat("{0}\n", str);
        }
    }
}