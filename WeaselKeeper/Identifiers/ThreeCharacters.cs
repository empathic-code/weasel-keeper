using System;
using System.Collections.Generic;
using System.Linq;
using WeaselKeeper.Config;

namespace WeaselKeeper.Identifiers
{
    internal class ThreeCharacters : IReplace
    {
        public const int MinimumAbbreviationLength = 3;
        private readonly Translations _translations = new Translations();
        public int Offset = 0;
        private string _lastIdentifier = "";

        public ThreeCharacters()
        {
        }

        public ThreeCharacters(Translations translations)
        {
            _translations = translations;
        }

        public string Replace(string identifier)
        {
            PreventShort(identifier);
            PreventCollision(identifier);
            return Abbreviate(identifier);
        }

        private static void PreventShort(string identifier)
        {
            if (IsLongEnough(identifier)) return;
            const string message = "Identifier is too short: {0}";
            throw new Exception(string.Format(message, identifier));
        }

        private static bool IsLongEnough(string identifier)
        {
            return identifier.Length >= MinimumAbbreviationLength;
        }

        private void PreventCollision(string identifier)
        {
            if (identifier == _lastIdentifier)
            {
                ShiftOffset();
                return;
            }
            Remember(identifier);
            ResetOffset();
        }

        private void ShiftOffset()
        {
            Offset++;
        }

        private void Remember(string identifier)
        {
            _lastIdentifier = identifier;
        }

        private void ResetOffset()
        {
            Offset = 0;
        }

        private string Abbreviate(string identifier)
        {
            string abbreviation = LookupFixedAbbreviation(identifier);
            if (string.IsNullOrEmpty(abbreviation))
            {
                abbreviation = GenerateAbbreviation(identifier);
            }
            return abbreviation;
        }

        private string LookupFixedAbbreviation(string identifier)
        {
            return _translations.Translate(identifier, "");
        }

        private string GenerateAbbreviation(string identifier)
        {
            // Keep first character intact, 
            // Then take all consonants.
            // In case of collisions, try again and increase offset, padd at the back!
            // Customer --> 1. Cst, 2. Ctm 3. Cmr 4. Crs
            char firstCharacter = identifier[0];
            List<char> consonants = identifier.Skip(1).Where(c => "aeiou".IndexOf(c) == -1).ToList();
            consonants = ExtendCharacterSetForShortIdentifiers(consonants);
            IEnumerable<char> chars = consonants.Skip(Offset).Take(MinimumAbbreviationLength - 1);
            return firstCharacter + string.Join("", chars).ToLower();
        }

        private List<char> ExtendCharacterSetForShortIdentifiers(List<char> consonants)
        {
            while ((consonants.Count - Offset) <= MinimumAbbreviationLength)
            {
                consonants.AddRange(consonants);
            }
            return consonants;
        }
    }
}