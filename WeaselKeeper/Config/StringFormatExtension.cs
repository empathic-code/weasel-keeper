namespace WeaselKeeper.Config
{
    public static class StringFormatExtension
    {
        public static string Format_(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}