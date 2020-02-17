using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Class : Node
    {
        public Class(TSpan span,
            ClassKind kind,
            Annotations? annotations,
            bool bare,
            Cap? cap,
            Identifier name,
            TypeParameters? typeParameters,
            Type? provides,
            String? doc,
            Members members)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Bare = bare;
            Cap = cap;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Doc = doc;
            Members = members;
        }

        public ClassKind Kind { get; }
        public Annotations? Annotations { get; }
        public bool Bare { get; }
        public Cap? Cap { get; }
        public Identifier Name { get; }
        public TypeParameters? TypeParameters { get; }
        public Type? Provides { get; }
        public String? Doc { get; }
        public Members Members { get; }
    }
}
