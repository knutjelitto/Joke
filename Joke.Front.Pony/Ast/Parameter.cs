﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Parameter : Base
    {
        public Parameter(ISpan span, Identifier name, Type type, Expression? expression)
            : base(span)
        {
            Name = name;
            Type = type;
            Expression = expression;
        }

        public Identifier Name { get; }
        public Type Type { get; }
        public Expression? Expression { get; }
    }
}