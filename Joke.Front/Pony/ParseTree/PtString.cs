using System.Diagnostics;
using Joke.Front.Pony.Lexing;

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
#if true
            return string.Empty;
#else
            Debug.Assert(span.Length == 1);

            var value = span[0].GetPayload();
            if (value.StartsWith("\"\"\""))
            {
                value = value.Substring(3, value.Length - 6);
            }
            else
            {
                value = value.Substring(1, value.Length - 2);

                //TODO: decode string
            }

            return value;
#endif
        }

    }
}
