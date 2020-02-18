using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private static class First
        {
            public static readonly TK[] Class = new TK[]
            {
                TK.Type, TK.Interface, TK.Trait, TK.Primitive, TK.Struct, TK.Class, TK.Actor
            };

            public static readonly TK[] Parameter = new TK[]
            {
                TK.Identifier, TK.Ellipsis
            };
        }
    }
}
