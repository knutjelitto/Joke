using System.IO;
using System.Linq;

using Joke.Front.Pony.Ast;

namespace Joke
{
    public class Stats
    {
        private FfiVisitor ffiVisitor = new FfiVisitor();

        public void Update(Module module)
        {
            ffiVisitor.Visit(module);
        }

        public void Report(TextWriter output)
        {
            var ffis = ffiVisitor.Ffis.Distinct().OrderBy(s => s);
            foreach (var ffi in ffis)
            {
                output.WriteLine(ffi);
            }
        }
    }
}
