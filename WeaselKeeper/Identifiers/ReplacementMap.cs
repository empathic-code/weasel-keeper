using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeaselKeeper.Identifiers
{
    public class ReplacementMap
    {
        private readonly Dictionary<string, string> _replacements = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> Replacements
        {
            get { return new ReadOnlyDictionary<string, string>(_replacements); }
        }

        public bool WasAlreadyReplaced(string identifier)
        {
            return _replacements.ContainsKey(identifier);
        }

        // Alias
        public string For(string identifierName)
        {
            return _replacements[identifierName];
        }
            
        public string OldReplacementFor(string identifierName)
        {
            return For(identifierName);
        }

        public bool IsNotUnique(string replacement)
        {
            return _replacements.ContainsValue(replacement);
        }

        public void Remember(string originalIdentifier, string replacement)
        {
            _replacements.Add(originalIdentifier.Trim(), replacement.Trim());
        }
    }
}