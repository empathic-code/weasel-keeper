namespace WeaselKeeper.Identifiers
{
    //http://stackoverflow.com/questions/18074206/how-to-find-variable-declare-is-not-use-in-the-class-in-roslyn
    internal interface IReplace
    {
        string Replace(string identifier);
    }
}