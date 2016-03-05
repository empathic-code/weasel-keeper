using System;
using System.Text;
using WeaselKeeper.Analysis;

namespace WeaselKeeper.Config
{
    internal class SourceCode
    {
        public static Snippet FromStdin()
        {
            string code = ReadCodeFromStdIn();
            return Snippet.Parse(code);
        }

        internal static string ReadCodeFromStdIn()
        {
            var code = new StringBuilder();
            string currentLine;
            while ((currentLine = Console.ReadLine()) != null)
            {
                code.AppendLine(currentLine);
            }
            return code.ToString()
        }
    }
}