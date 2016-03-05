using System.Collections.Generic;
using System.Linq;

namespace WeaselKeeper.Config
{
    internal class Translations : ConfigFile
    {
        private Dictionary<string, string> _identifiers = new Dictionary<string, string>();

        public static Translations ReadFromFile(string filename)
        {
            Dictionary<string, string> translations = ReadLines(filename)
                .Select(line => line.Split('='))
                .ToDictionary(line => line[0].Trim(), line => line[1].Trim());
            return new Translations {_identifiers = translations};
        }

        private bool Contains(string identifier)
        {
            return _identifiers.ContainsKey(identifier);
        }

        public string Translate(string identifier, string defaultValue)
        {
            return Contains(identifier) ? _identifiers[identifier] : defaultValue;
        }
    }
}