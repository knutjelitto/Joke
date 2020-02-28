using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joke.Front.Err
{
    public class Errors : IDescription
    {
        public List<Error> Items = new List<Error>();

        public Errors()
        {
            Help = new ErrorsHelper(this);
        }

        public ErrorsHelper Help { get; }

        public void Add(Error error)
        {
            Items.Add(error);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool NoFatal()
        {
            return !Items.Any(e => e.Severity == Severity.Fatal);
        }

        public bool NoError()
        {
            return NoFatal() && !Items.Any(e => e.Severity == Severity.Error);
        }

        public void Describe(TextWriter writer)
        {
            foreach (var error in Items)
            {
                error.Description.Describe(writer);
            }
        }
    }
}
