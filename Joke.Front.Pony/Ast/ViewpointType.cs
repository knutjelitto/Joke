using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class ViewpointType : Type
    {
        public ViewpointType(TokenSpan span, Type type, Type arrow)
            : base(span)
        {
            Type = type;
            Arrow = arrow;
        }

        public Type Type { get; }
        public Type Arrow { get; }
    }
}
