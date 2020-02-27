using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyPackage
    {
        public PonyPackage(CompilerContext context, DirRef packageDir, bool isBuiltin = false)
        {
            Context = context;
            PackageDir = packageDir;

            Logger.WriteLine($"pack: {packageDir.FileName}");

            if (!isBuiltin)
            {
                context.LoadPackage("builtin");
            }

            using (Logger.Indent())
            {
                var units = new List<PonyUnit>();
                foreach (var unitFile in packageDir.Files("*.pony"))
                {
                    Logger.WriteLine($"file: {unitFile.FileName}");
                    units.Add(new PonyUnit(this, unitFile));
                }
            }
        }

        public CompilerContext Context { get; }
        public ErrorAccu Errors => Context.Errors;
        public DirRef PackageDir { get; }
        public IndentWriter Logger => Context.Logger;
    }
}
