﻿using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Module : Node
    {
        public Module(TSpan span, String? doc, IReadOnlyList<Use> uses, IReadOnlyList<Class> classes)
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
