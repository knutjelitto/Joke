using System;

namespace Joke.Front.Joke
{
    public class JokeParser
    {
        public JokeParser(JokeScanner scanner)
        {
            Scanner = scanner;
        }

        public JokeScanner Scanner { get; }


        public Ast.Unit Parse()
        {
            throw new NotImplementedException();
        }

        public Ast.Unit Unit()
        {
            throw new NotImplementedException();
        }

        public Ast.Identifier Identifier()
        {
            throw new NotImplementedException();
        }
    }
}
