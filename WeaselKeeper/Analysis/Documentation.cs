using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Analysis
{
    public static class Documentation
    {
        public static string From(string comments, ReplacementMap map)
        {
            // I tried to make this look good but it doesn't work.
            var documentation = new List<string>();

            if (string.IsNullOrEmpty(comments))
            {
                documentation.Add("// The Method was undocumented!");
            }

            var explanations = ParseExplanations(comments)
                .ToDictionary(pair => map.OldReplacementFor(pair.Key), Value);

            var variables = map.Replacements
                .ToDictionary(Value, Key)
                .Where(pair => !explanations.Keys.Contains(pair.Key))
                .ToDictionary(Key, Value);

            documentation.AddRange(explanations.Union(variables).Select(AsComment));
            AppendEmptyLine(documentation);

            return string.Join("\n", documentation);
        }

        private static void AppendEmptyLine(List<string> documentation)
        {
            documentation.Add(" ");
        }

        private static Dictionary<string, string> Flip(ReplacementMap replacements)
        {
            return replacements.Replacements.ToDictionary(Value, Key);
        }

        private static string Value(KeyValuePair<string, string> p) { return p.Value; }
        private static string Key(KeyValuePair<string, string> p) { return p.Key; }


        private static string AsComment(KeyValuePair<string, string> pair)
        {
            return AsComment(pair.Key, pair.Value);
        }

        private static string AsComment(string key, string value)
        {
            return string.Format("// {0}: {1}", key, value);
        }

        /// <summary>
        ///     Splits a comment like this:
        ///     <code>"// Key: Explanation"</code>
        ///     into a Dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ParseExplanations(string comments)
        {
            Dictionary<string, string> explanations = comments
                .Split('\n')
                .Select(
                    comment => comment.Split(':')
                )
                .Where(parts => parts.Length == 2)
                .ToDictionary(
                    parts => parts[0].Replace("/", "").Trim(),
                    parts => parts[1].Trim()
                );
            return explanations;
        }
    }
}