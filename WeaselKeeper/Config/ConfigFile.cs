using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WeaselKeeper.Config
{
    internal class ConfigFile
    {
        // Reads Nonempty Lines that are not comments
        protected static IEnumerable<string> ReadLines(string filename)
        {
            string path = EnsureThatFileWasPlacedAlongExecutable(filename);
            IEnumerable<string> lines = File
                .ReadAllLines(path)
                .Where(line => !line.Trim().StartsWith("#"))
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim());
            return lines;
        }

        private static string EnsureThatFileWasPlacedAlongExecutable(string filename)
        {
            string exe = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exe);
            string path = Path.Combine(exeDirectory, filename);
            if (!File.Exists(path))
            {
                string message = "Method Blacklist not found. \nPlace '{0}' next to exe file in '{1}'".Format_(
                    filename, exeDirectory);
                throw new NotPlacedLocallyComplaint(message);
            }
            return path;
        }
    }
}