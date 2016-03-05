using System;
using Microsoft.CodeAnalysis;
using WeaselKeeper.Config;

namespace WeaselKeeper.Identifiers
{
    internal class Condition
    {
        // The Alphabet has 26 Elements, 
        private const int MaxTrials = 25; 
        private readonly Blacklist _blacklist;
        private readonly IReplace _strategy;

        public ReplacementMap Map { get; private set; }

        public Condition(IReplace strategy, Blacklist blacklist)
        {
            Map = new ReplacementMap();
            _blacklist = blacklist;
            _strategy = strategy;
        }

        public SyntaxToken Replace(SyntaxToken token, SyntaxToken _)
        {
            return ReplaceIdentifier(token);
        }

        private SyntaxToken ReplaceIdentifier(SyntaxToken token)
        {
            string identifier = token.IdentifierName();
            string replacedIdentifier = ReplaceIfNecessary(identifier);
            return token.RenameWith(replacedIdentifier);
        }

        private string ReplaceIfNecessary(string identifier)
        {
            if (MustNotBeReplaced(identifier))
            {
                return identifier;
            }

            if (Map.WasAlreadyReplaced(identifier))
            {
                return Map.OldReplacementFor(identifier);
            }

            var replacement = Replace(identifier);
            Map.Remember(identifier, replacement);
            return replacement;
        }

        private string Replace(string identifier)
        {
            int trials = 0;
            string replacement = "";
            do
            {
                if (trials > MaxTrials)
                {
                    throw new Exception("Can't find unique identifier replacement for " + identifier);
                }
                replacement = _strategy.Replace(identifier);
                trials++;
            } while (Map.IsNotUnique(replacement));
            return replacement;
        }

        private bool MustNotBeReplaced(string identifier)
        {
            return _blacklist.Contains(identifier);
        }
    }
}