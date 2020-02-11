using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class MethodMember : Member
    {
        public MethodMember(ISpan span, MemberKind kind, Capability? receiverCap, bool bare, Identifier name, TypeParameters typeParameters, Parameters parameters, Type? returnType, bool partial, DocString? doc, Expression? body)
            : base(span, kind)
        {
            ReceiverCap = receiverCap;
            Bare = bare;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Partial = partial;
            Doc = doc;
            Body = body;
        }

        public Capability? ReceiverCap { get; }
        public bool Bare { get; }
        public TypeParameters TypeParameters { get; }
        public Parameters Parameters { get; }
        public Type? ReturnType { get; }
        public Boolean Partial { get; }
        public DocString? Doc { get; }
        public Expression? Body { get; }
    }
}
