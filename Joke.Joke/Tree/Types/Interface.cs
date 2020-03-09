﻿using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Interface : IType, IMember
    {
        public Interface(TokenSpan span, Identifier name, TypeParameterList? typeParameters, IType? provides, MemberList members)
        {
            Span = span;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Members = members;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public IType? Provides { get; }
        public MemberList Members { get; }
    }
}