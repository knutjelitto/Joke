using System;

using Joke.Front;
using Joke.Front.Err;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.Parsing;
using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyFile
    {
        public PonyFile(ErrorAccu errors, FileRef file)
        {
            Errors = errors;
            File = file;

            var source = Source.FromFile(file);
            var tokenizer = new PonyTokenizer(errors, source);
            try
            {
                tokenizer.Tokenize();
            }
            catch (JokeException joke)
            {
                joke.Error.Description.Describe(Console.Out);
            }
            var parser = new PonyParser(errors, source, tokenizer.Tokens);
            try
            {
                var module = parser.Module();

                errors.Describe(Console.Out);
            }
            catch (JokeException joke)
            {
                joke.Error.Description.Describe(Console.Out);
            }


        }

        public ErrorAccu Errors { get; }
        public FileRef File { get; }
    }
}
