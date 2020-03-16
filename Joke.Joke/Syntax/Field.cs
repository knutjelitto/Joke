using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Joke.Syntax
{
    public class Field
    {
        public Field(Class @class, Tree.Field source)
        {
            Class = @class;
        }

        public Class Class { get; }
    }
}
