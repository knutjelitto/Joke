﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            foreach (var unitFile in EnumerateJokes(BuiltinDir))
            {
                Console.WriteLine($"{unitFile}");
                Compile(unitFile);
            }
        }

        private void Compile(FileRef file)
        {
            var source = Source.FromFile(file);
            var errors = new Errors();
            var tokenizer = new Tokenizer(errors, source);
            var tokens = tokenizer.Tokenize();

            errors.Describe(Console.Out);

            var builder = new StringBuilder(source.Content.Length);
            foreach (var token in tokens)
            {
                builder.Append(token.GetClutter());
                builder.Append(token.GetPayload());

                Console.WriteLine($"{token}");
            }

            Debug.Assert(builder.ToString() == source.Content);

            var parser = new Parser(errors, tokens);

            var unit = parser.ParseUnit();
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