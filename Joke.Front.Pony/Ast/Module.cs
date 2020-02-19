using System.Collections.Generic;

using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Module : Node
    {
        public Module(TokenSpan span, String? doc, IReadOnlyList<Use> uses, IReadOnlyList<Class> classes)
            : base(span)
        {
            Doc = doc;
            Uses = uses;
            Classes = classes;
        }

        public String? Doc { get; }
        public IReadOnlyList<Use> Uses { get; }
        public IReadOnlyList<Class> Classes { get; }
    }
}
