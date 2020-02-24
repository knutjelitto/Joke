using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Members : Node
    {
        public Members(PonyTokenSpan span, Fields fields, Methods methods)
            : base(span)
        {
            Fields = fields;
            Methods = methods;
        }

        public Fields Fields { get; }
        public Methods Methods { get; }
    }
}
