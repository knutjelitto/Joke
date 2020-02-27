using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtInt : PtLiteral<BigInteger>
    {
        public PtInt(PonyTokenSpan span)
            : base(span, CreateValue(span))
        {
        }

        private static BigInteger CreateValue(PonyTokenSpan span)
        {
#if true
            return BigInteger.Zero;
#else
            Debug.Assert(span.Length == 1);

            var str = span[0].GetPayload().Replace("_", string.Empty);
            BigInteger value;
            if (str.StartsWith("0x"))
            {
                value = BigInteger.Parse(str.Substring(2), NumberStyles.HexNumber);
            }
            else
            {
                value = BigInteger.Parse(str);
            }

            return value;
#endif
        }
    }
}
