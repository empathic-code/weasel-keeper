namespace WeaselKeeper.Identifiers
{
    internal class Normal : IReplace
    {
        // Doesn't Rename
        public string Replace(string identifier)
        {
            return identifier;
        }
    }
}