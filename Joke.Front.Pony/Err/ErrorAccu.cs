using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joke.Front.Pony.Err
{
    public class ErrorAccu : IDescription
    {
        public List<Error> Errors = new List<Error>();

        public void Add(Error error)
        {
            Errors.Add(error);
        }

        public void Clear()
        {
            Errors.Clear();
        }

        public bool NoFatal()
        {
            return !Errors.Any(e => e.Severity == Severity.Fatal);
        }

        public bool NoError()
        {
            return NoFatal() && !Errors.Any(e => e.Severity == Severity.Error);
        }

        public void Describe(TextWriter writer)
        {
            foreach (var error in Errors)
            {
                error.Description.Describe(writer);
            }
        }
    }
}
