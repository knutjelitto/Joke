using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Postfix : Expression
    {
        public Postfix(TSpan span, Expression atom, IReadOnlyList<PostfixPart> parts)
            : base(span)
        {
            Atom = atom;
            Parts = parts;
        }

        public Expression Atom { get; }
        public IReadOnlyList<PostfixPart> Parts { get; }
    }
}
