using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class ViewpointType : Type
    {
        public ViewpointType(PonyTokenSpan span, Type type, Type arrow)
            : base(span)
        {
            Type = type;
            Arrow = arrow;
        }

        public Type Type { get; }
        public Type Arrow { get; }
    }
}
