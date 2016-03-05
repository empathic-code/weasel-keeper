using WeaselKeeper.Analysis;

namespace WeaselKeeper.Config
{
    internal class BuildConfig
    {
        private const string Code = @"
// CustomerPoints: Calculates Points for the customer to awesomify
// dueDate: The date when the movie was due
// returned: The date when the movie was actually returned
// customerPoints: The customers current point balance
public int CustomerPoints(DateTime dueDate, DateTime returned, int customerPoints)
{
    const int fine = 10;
    const int reward = 5;
    if (dueDate < returned)
    {
        int points = fine * (dueDate - returned).Days;
        customerPoints += points;
    }
    else if (dueDate > returned)
    {
        int points = fine * (returned - dueDate).Days;
        customerPoints -= points;
    }
    if (customerPoints < 0)
    {
        return 0;
    }
    return customerPoints;
}";


#if DEBUG
        public static Snippet Snippet
        {
            get { return Snippet.Parse(Code); }
        }

        public static string[] CommandLine(string[] commandline)
        {
            return new[] {"--abbrev", "--map", "--list-unique"};
            // "--sane", "--show-warnings"
        }
#else
        public static Snippet Snippet
        {
            get { return SourceCode.FromStdin(); }
        }

        public static string[] CommandLine(string[] commandline)
        {
            return commandline;
        }
#endif
    }
}