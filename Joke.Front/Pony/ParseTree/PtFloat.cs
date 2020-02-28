using System.Diagnostics;
using System.Globalization;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtFloat : PtLiteral<double>
    {
        public PtFloat(PonyTokenSpan span)
            : base(span, CreateValue(span))
        {
        }

        private static double CreateValue(PonyTokenSpan span)
        {
            Debug.Assert(span.Length == 1);

            var str = span[0].GetPayload().Replace("_", string.Empty);
            var value = double.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture);

            return value;
        }
    }
}
