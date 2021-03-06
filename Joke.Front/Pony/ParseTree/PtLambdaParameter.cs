﻿using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaParameter : PtParameter
    {
        public PtLambdaParameter(PonyTokenSpan span, PtIdentifier name, PtType? type, PtExpression? value)
            : base(span)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public PtIdentifier Name { get; }
        public PtType? Type { get; }
        public PtExpression? Value { get; }
    }
}
