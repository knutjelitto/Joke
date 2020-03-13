using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Joke.Decoding;
using Joke.Joke.Err;
using Joke.Joke.Tools;
using Joke.Joke.Tree;

namespace Joke.Joke
{
    public class Tester
    {
        public void Run()
        {
            MakeBuiltin();
        }

        private void MakeBuiltin()
        {
            var units = new List<CompilationUnit>();

            foreach (var unitFile in EnumerateJokes(BuiltinDir).Skip(0))
            {
                Console.WriteLine($"{unitFile}");
                var (errors, unit) = Compile(unitFile);
                if (!errors.NoError() || unit == null)
                {
                    errors.Describe(Console.Out);
                    break;
                }
                units.Add(unit);
            }

            foreach (var unit in units)
            {
                foreach (var member in unit.Members.OfType<INamed>())
                {
                    Console.Write($"{member.Name} ");
                }
            }
            Console.WriteLine();
        }

        private (Errors, CompilationUnit?) Compile(FileRef file)
        {
            var source = Source.FromFile(file);
            var errors = new Errors();
            var tokenizer = new Tokenizer(errors, source);
            var tokens = tokenizer.Tokenize();

            if (!errors.NoError())
            {
                return (errors, null);
            }

#if false
            var builder = new StringBuilder(source.Content.Length);
            foreach (var token in tokens)
            {
                builder.Append(token.Clutter);
                builder.Append(token.Payload);
            }

            Debug.Assert(builder.ToString() == source.Content);
#endif

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

        private IEnumerable<FileRef> EnumerateJokes(params DirRef[] roots)
        {
            foreach (var root in roots)
            {
                foreach (var joke in root.Files("*.joke", true))
                {
                    yield return joke;
                }
            }
        }

    }
}
