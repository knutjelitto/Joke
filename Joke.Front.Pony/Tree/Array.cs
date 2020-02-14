﻿using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Array : Expression
    {
        public Array(TSpan span, Type? type, Expression? elements)
            : base(span)
        {
            Type = type;
            Elements = elements;
        }

        public Type? Type { get; }
        public Expression? Elements { get; }
    }
}
