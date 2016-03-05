using System.Collections.Generic;
using System.Linq;
using WeaselKeeper.Analysis;
using WeaselKeeper.Cli;
using WeaselKeeper.Config;

namespace WeaselKeeper
{
    internal class CommandLineInterface
    {
        private readonly IEnumerable<Call> _actions;
        private readonly Options _options;

        public CommandLineInterface(IEnumerable<string> commandLine)
        {
            _options = Configure();
            _actions = _options.Parse(commandLine);
        }

        public void Run(Snippet snippet)
        {
            if (NoActionsSpecified(_actions))
            {
                Help.Print(_options);
                return;
            }
            InvokeAll(_actions, snippet);
        }

        private static Options Configure()
        {
            Blacklist blacklist = Blacklist.ReadFromFile(FileNames.BlacklistFile);
            Translations translations = Translations.ReadFromFile(FileNames.TranslationsFile);
            var conditions = new Conditions(blacklist, translations);
            var options = new Options();
            options
                .Add("--loc", Report.CountLines)
                .Add("--name", Report.MethodName)
                .Add("--count-tokens", Report.CountTokens)
                .Add("--count-identifiers", Report.CountIdenfifiers)
                .Add("--count-unique", Report.CountUnique)
                .Add("--count-domain", s => Report.CountDomain(s, blacklist))
                .Add("--list-tokens", Report.ListTokens)
                .Add("--list-identifiers", Report.ListIdentifiers)
                .Add("--list-unique", Report.ListUniqueIdentifiers)
                .Add("--list-domain", s => Report.ListDomain(s, blacklist))
                .Add("--normal", conditions.ShowCodeForNormalCondition)
                .Add("--abbrev", conditions.ShowCodeForAbbrevCondition)
                .Add("--single", conditions.ShowCodeForSingleCondition)
                .Add("--sane", conditions.PerformsASanityCheck)
                .Add("--show-warnings", conditions.Warnings)
                .Add("--help", t => Help.Print(options));
            return options;
        }

        private static bool NoActionsSpecified(IEnumerable<Call> actions)
        {
            return !actions.Any();
        }

        private static void InvokeAll(IEnumerable<Call> actions, Snippet snippet)
        {
            foreach (Call action in actions)
            {
                action.Invoke(snippet);
            }
        }
    }
}