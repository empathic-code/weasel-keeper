using System;
using Microsoft.CodeAnalysis;
using WeaselKeeper.Analysis;
using WeaselKeeper.Cli;
using WeaselKeeper.Config;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper
{
    internal class Conditions
    {
        private readonly Blacklist _blacklist;
        private readonly Translations _translations;
        private SanityCheck _check;

        public Conditions(Blacklist blacklist, Translations translations)
        {
            _blacklist = blacklist;
            _translations = translations;
        }

        public bool PrintMap { get; private set; }

        public void ShowCodeForNormalCondition(Snippet snippet)
        {
            Print(snippet, new Normal());
        }

        public void ShowCodeForAbbrevCondition(Snippet snippet)
        {
            Print(snippet, new ThreeCharacters(_translations));
        }

        public void ShowCodeForSingleCondition(Snippet snippet)
        {
            Print(snippet, new SingleCharacter());
        }

        private void Print(Snippet snippet, IReplace strategy)
        {
            ReplacedSnippet replacedSnippet = ReplaceIdentifiers(snippet, strategy);
            Console.WriteLine(replacedSnippet);
        }

        private ReplacedSnippet ReplaceIdentifiers(Snippet snippet, IReplace strategy)
        {
            var replacer = new Condition(strategy, _blacklist);
            return snippet.ReplaceIdentifiers(replacer);
        }

        public void PerformsASanityCheck(Snippet snippet)
        {
            _check = new SanityCheck();
            _check.CheckTree(snippet.Root, snippet);
        }

        public void Warnings(Snippet snippet)
        {
            foreach (Diagnostic diagnostic in _check.Warnings())
            {
                Console.WriteLine(diagnostic.GetMessage());
            }
        }
    }
}