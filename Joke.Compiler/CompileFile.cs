using System;
using System.Diagnostics;
using System.Linq;

using Joke.Front;
using Joke.Front.Err;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Parsing;
using Joke.Outside;

namespace Joke.Compiler
{
    public class CompileFile
    {
        public CompileFile(CompilePackage package, FileRef unitFile)
        {
            Package = package;
            UnitFile = unitFile;

            var source = Source.FromFile(unitFile);
            var tokenizer = new PonyTokenizer(Errors, source);
            tokenizer.Tokenize();
            var parser = new PonyParser(Errors, source, tokenizer.Tokens);
            Unit = parser.Unit();
            UsePackages();
            Errors.Describe(Console.Out);
        }

        public CompilePackage Package { get; }
        public FileRef UnitFile { get; }
        public ErrorAccu Errors => Package.Errors;
        public Compilation Compilation => Package.Compilation;
        public IndentWriter Logger => Compilation.Logger;
        public PtUnit Unit { get; }

        private void UsePackages()
        {
            foreach (var use in Unit.Uses.OfType<PtUseUri>())
            {
                Use(use);
            }
        }

        private void Use(PtUseUri use)
        {
            var uri = use.Uri.Value;

            var parts = uri.Split(':');

            if (parts.Length == 1)
            {
                parts = $"package:{uri}".Split(':');
            }

            Debug.Assert(parts.Length == 2);

            var protocol = parts[0];
            var value = parts[1];
            string? name = use.Name?.Value;

            switch (protocol)
            {
                case "package":
                    var pack = Compilation.UsePackage(value);
                    break;
            }

            if (!uri.StartsWith("lib:") && !uri.StartsWith("path:"))
            {
                Debug.Assert(parts.Length == 1 || (parts.Length == 2 && parts[0] == "package" && parts[0].Length > 0 && parts[1].Length > 0));

                if (parts.Length == 2)
                {
                    uri = parts[1];
                }

            }
        }
    }
}
