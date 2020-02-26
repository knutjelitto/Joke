using System;
using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyPackage
    {
        public PonyPackage(CompilerContext context, ErrorAccu errors, DirRef packageDir)
        {
            Context = context;
            Errors = errors;
            PackageDir = packageDir;

            Console.WriteLine($"package: {packageDir.FileName}");

            var files = new List<PonyFile>();
            foreach (var ponyFile in packageDir.Files("*.pony"))
            {
                Console.WriteLine($"  .. {ponyFile.FileName}");
                files.Add(new PonyFile(errors, ponyFile));
            }
        }

        public CompilerContext Context { get; }
        public ErrorAccu Errors { get; }
        public DirRef PackageDir { get; }
    }
}
