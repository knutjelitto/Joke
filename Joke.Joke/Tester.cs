using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Joke.Decoding;
using Joke.Joke.Err;
using Joke.Joke.Syntax;
using Joke.Joke.Tools;

namespace Joke.Joke
{
    public class Tester
    {
        public void Run()
        {
            MakePackage(BuiltinDir);
#if true
            foreach (var pack in GetCores())
            {
                MakePackage(pack);
            }
#endif
        }

        private void MakePackage(DirRef packageDir)
        {
            var units = new List<Tree.Unit>();

            foreach (var unitFile in EnumerateJokes(packageDir).Skip(0))
            {
                var name = "pack:" + unitFile.ToString().Substring(PackagesDir.ToString().Length + 1);

                Console.WriteLine($"{name}");
                var (errors, unit) = Compile(unitFile, name);
                if (!errors.NoError() || unit == null)
                {
                    errors.Describe(Console.Out);
                    return;
                }
                units.Add(unit);
            }

            var package = new Package(null, units);
            package.Build();

            foreach (var unit in package.Units)
            {
                foreach (var unitMember in unit.Members)
                {
                    Console.WriteLine($"{unitMember.Name}");
                    foreach (var classMember in unitMember.Items)
                    {
                        Console.WriteLine($"  {classMember}");
                    }
                }

            }

            package.Errors.Describe(Console.Out);
        }

        private (Errors, Tree.Unit?) Compile(FileRef file, string name)
        {
            var source = Source.FromFile(file, name);
            var errors = new Errors();
            var tokenizer = new Tokenizer(errors, source);
            var tokens = tokenizer.Tokenize();

            if (!errors.NoError())
            {
                return (errors, null);
            }

            var parser = new Parser(errors, tokens);

            try
            {
                var unit = parser.ParseUnit();

                return (errors, unit);
            }
            catch
            {

            }

            return (errors, null);
        }

        private DirRef ProjectDir => DirRef.ProjectDir().Up.Dir("Joke.Joke");
        private DirRef SrcDir => ProjectDir.Dir("src");
        private DirRef PackagesDir => SrcDir.Dir("packages");
        private DirRef BuiltinDir => PackagesDir.Dir("builtin");
        private DirRef CoreDir => PackagesDir.Dir("core");

        private IEnumerable<DirRef> GetCores()
        {
            foreach (var corePackage in CoreDir.Directories())
            {
                Console.WriteLine($"{corePackage}");
                yield return corePackage;
            }
        }

        private IEnumerable<FileRef> EnumerateJokes(params DirRef[] roots)
        {
            foreach (var root in roots)
            {
                foreach (var joke in root.Files("*.joke", false))
                {
                    yield return joke;
                }
            }
        }
    }
}
