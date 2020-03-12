using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Joke.Joke.Decoding;
using Joke.Joke.Err;
using Joke.Joke.Tools;

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
            foreach (var unitFile in EnumerateJokes(BuiltinDir).Skip(0))
            {
                Console.WriteLine($"{unitFile}");
                var ok = Compile(unitFile);
                if (!ok)
                {
                    break;
                }
            }
        }

        private bool Compile(FileRef file)
        {
            var source = Source.FromFile(file);
            var errors = new Errors();
            var tokenizer = new Tokenizer(errors, source);
            var tokens = tokenizer.Tokenize();

            errors.Describe(Console.Out);

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
            }
            catch (NotImplementedException)
            {

            }

            errors.Describe(Console.Out);

            return errors.NoError();
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
