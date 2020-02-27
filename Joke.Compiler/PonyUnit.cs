using System;

using Joke.Front;
using Joke.Front.Err;
using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.Parsing;
using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyUnit
    {
        public PonyUnit(PonyPackage package, FileRef unitFile)
        {
            Package = package;
            UnitFile = unitFile;

            var source = Source.FromFile(unitFile);
            var tokenizer = new PonyTokenizer(Errors, source);
            tokenizer.Tokenize();
            var parser = new PonyParser(Errors, source, tokenizer.Tokens);
            Unit = parser.Unit();
            Errors.Describe(Console.Out);
        }

        public PonyPackage Package { get; }
        public FileRef UnitFile { get; }
        public ErrorAccu Errors => Package.Errors;
        public CompilerContext Context => Package.Context;
        public Unit Unit { get; }
    }
}
