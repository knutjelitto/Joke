using System.Collections.Generic;
using System.Diagnostics;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private List<T> PlusList<T>(System.Func<T> parse, params TK[] iffnt)
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
