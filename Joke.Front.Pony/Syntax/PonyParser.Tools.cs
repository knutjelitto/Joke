﻿using System.Collections.Generic;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private List<T> List<T>(System.Func<T> parse, params TK[] iffnt)
        {
            var items = new List<T>();
            if (iffnt.Length == 0 || Issnt(iffnt))
            {
                do
                {
                    items.Add(parse());
                }
                while (MayMatch(TK.Comma));
            }

            return items;
        }
    }
}