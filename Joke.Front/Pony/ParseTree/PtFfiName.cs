using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtFfiName : PtIdentifier
    {
        public PtFfiName(PonyTokenSpan span, PtExpression name)
            : base(span)
        {
            Name = name;
        }

        public PtExpression Name { get; }

        public override string? ToString()
        {
            return $"@{Name}";
        }
    }
}
