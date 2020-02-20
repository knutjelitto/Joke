using System.Collections.Generic;
using Joke.Front.Pony.Err;
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

        private List<T> CollectRecover<T>(TokenSet recover, System.Func<T?> tryParse) where T : class
        {
            var items = new List<T>();

            while (true)
            {
                try
                {
                    var item = tryParse();
                    if (item != null)
                    {
                        items.Add(item);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (JokeException joke)
                {
                    Errors.Add(joke.Error);
                    SkipUntil(recover);
                }
            }

            return items;
        }
    }
}
