using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class QualifiedIdentifier : Identifier
    {
        public QualifiedIdentifier(SourceSpan span, Identifier identifier, IReadOnlyList<Identifier> identifiers)
            : base(span)
        {
            Identifier = identifier;
            Identifiers = identifiers;
        }

        public Identifier Identifier { get; }
        public IReadOnlyList<Identifier> Identifiers { get; }
    }
}
