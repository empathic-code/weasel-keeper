using System.Collections.Generic;

namespace WeaselKeeper.Config
{
    internal class Blacklist : ConfigFile
    {
        private readonly List<string> _bannedWords = new List<string>();

        public List<string> BannedWords
        {
            get { return _bannedWords; }
        }

        public static Blacklist ReadFromFile(string filename)
        {
            var blacklist = new Blacklist();
            IEnumerable<string> identifiers = ReadLines(filename);
            blacklist._bannedWords.AddRange(identifiers);
            return blacklist;
        }

        public bool Contains(string identifier)
        {
            return _bannedWords.Contains(identifier);
        }
    }
}