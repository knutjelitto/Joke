using System;
using System.Collections.Generic;
using Joke.Front.Pony.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyPackage
    {
        public PonyPackage(ErrorAccu errors, DirRef packageDir)
        {
            PackageDir = packageDir;

            Console.WriteLine($"package: {packageDir}");

            var modules = new List<PonyFile>();
            foreach (var ponyFile in packageDir.Files("*.pony"))
            {
                modules.Add(new PonyFile(errors, ponyFile));
            }
        }

        public DirRef PackageDir { get; }
    }
}
