using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Method : Node
    {
        public Method(TSpan span,
            MethodKind kind,
            Annotations annotations,
            Bare? bare,
            Cap? cap,
            Identifier name,
            TypeParameters typeParameters,
            Parameters parameters,
            Type? returnType,
            Partial? partial,
            String? doc,
            Expression? body)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Bare = bare;
            Cap = cap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Partial = partial;
            Doc = doc;
            Body = body;
        }

        public MethodKind Kind { get; }
        public Annotations Annotations { get; }
        public Bare? Bare { get; }
        public Cap? Cap { get; }
        public Identifier Name { get; }
        public TypeParameters TypeParameters { get; }
        public Parameters Parameters { get; }
        public Type? ReturnType { get; }
        public Partial? Partial { get; }
        public String? Doc { get; }
        public Expression? Body { get; }
    }
}
