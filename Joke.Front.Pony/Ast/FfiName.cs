using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class FfiName : Identifier
    {
        public FfiName(TokenSpan span, Expression name)
            : base(span)
        {
            Name = name;
        }

        public Expression Name { get; }

        public override string? ToString()
        {
            return $"@{Name}";
        }
    }
}
