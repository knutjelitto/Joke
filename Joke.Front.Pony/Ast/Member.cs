﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Member : Base
    {
        public Member(ISpan span, MemberKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public MemberKind Kind { get; }
    }
}
