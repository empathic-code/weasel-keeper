using WeaselKeeper.Config;

namespace WeaselKeeper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Runs Presets for DEBUG, Reads params, STDIN for RELEASE
            new CommandLineInterface(BuildConfig.CommandLine(args)).Run(BuildConfig.Snippet);
        }
    }
}