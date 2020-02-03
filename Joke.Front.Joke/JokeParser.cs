using System;

namespace Joke.Front.Joke
{
    public class JokeParser : Parser<JokeScanner>
    {
        public JokeParser(JokeScanner scanner)
            : base(scanner)
        {
        }

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
