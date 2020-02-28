using Joke.Front.Pony.Lexing;
using System.Diagnostics;

namespace Joke.Front.Pony.ParseTree
{
    public class PtString : PtLiteral<string>
    {
        public PtString(PonyTokenSpan span)
            : base(span, CreateValue(span))
        {
        }

        private static string CreateValue(PonyTokenSpan span)
        {
            Debug.Assert(span.Length == 1);

            var value = span[0].GetPayload();
            //Debug.Assert(!value.Contains("\\"));
            if (value.StartsWith("\"\"\""))
            {
                value = value[3..^3];
            }
            else
            {
                value = value[1..^1];
                //TODO: decode string
            }

            return value;
        }

    }
}
