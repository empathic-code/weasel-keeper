using System;

namespace WeaselKeeper.Identifiers
{
    internal class SingleCharacter : IReplace
    {
        public int CurrentCharacter = 0;
        public string Letters = "abcdefghijklmnopqrstuvwxyz";

        public string Replace(string identifier)
        {
            string letter = Letters[CurrentCharacter].ToString();
            Shift();
            return letter;
        }

        private void Shift()
        {
            CurrentCharacter++;
            if (CurrentCharacter >= Letters.Length)
            {
                throw new Exception("Single Letter Varible Pool depleted!");
            }
        }
    }
}