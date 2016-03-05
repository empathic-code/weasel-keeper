using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.CodeAnalysis;

namespace WeaselKeeper.Analysis
{
    [Serializable]
    public class InvalidCompilationException : Exception
    {
        public InvalidCompilationException(ImmutableArray<Diagnostic> diags) : this(MakeMessage(diags))
        {
        }

        protected InvalidCompilationException()
        {
        }

        protected InvalidCompilationException(string message) : base(message)
        {
        }

        protected InvalidCompilationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidCompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string MakeMessage(ImmutableArray<Diagnostic> diags)
        {
            var messages = new StringBuilder("There were errors when compiling the source code");
            foreach (Diagnostic diagnostic in diags)
            {
                messages.AppendLine("\t").Append(diagnostic.GetMessage());
            }
            return messages.ToString();
        }
    }
}