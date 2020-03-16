using System.Collections.Generic;
using System.Diagnostics;
using Joke.Joke.Err;
using Joke.Joke.Tools;

namespace Joke.Joke.Decoding
{
    public class Tokenizer
    {
        public const char NoCharacter = '\uFFFF';

        private readonly string content;
        private readonly int limit;

        /// <summary>
        /// Globally marks the start of the whitespace clutter prefixing a token.
        /// </summary>
        public int clutter;

        /// <summary>
        /// Globally marks the start of a token payload.
        /// </summary>
        public int payload;

        /// <summary>
        /// The current index into to <see cref="content"/> array.
        /// </summary>
        public int current;

        /// <summary>
        /// Last skip() surrounds a newline
        /// </summary>
        public bool nl;

        public Tokens Tokens { get; private set; }

        public Tokenizer(Errors errors, ISource source)
        {
            Errors = errors;
            Source = source;
            Tokens = new Tokens(Source, System.Array.Empty<Token>());

            content = Source.Content;
            limit = content.Length;
            current = 0;
            clutter = 0;
            payload = 0;
        }

        public Errors Errors { get; }
        public ISource Source { get; }
        private char Current => current < limit ? content[current] : NoCharacter;
        private char Next => current + 1 < limit ? content[current + 1] : NoCharacter;
        private char OverNext => current + 2 < limit ? content[current + 2] : NoCharacter;


        public Tokens Tokenize()
        {
            return new Tokens(Source, GetTokens());
        }

        private List<Token> GetTokens()
        {
            current = 0;
            clutter = 0;
            payload = 0;

            var tokens = new List<Token>();

            Token token;
            while (true)
            {
                try
                {
                    token = NextToken();
                    tokens.Add(token);
                    if (token.Kind == TK.Eof)
                    {
                        break;
                    }
                }
                catch (JokeException joke)
                {
                    // simple catch, report & slurp
                    Errors.Add(joke.Error);
                }
            }

            return tokens;
        }

        private Token NextToken()
        {
            clutter = current;

            nl = Skip();

            while (true)
            {
                payload = current;

                Debug.Assert(current <= limit);
                if (current == limit)
                {
                    return Token(TK.Eof);
                }

                switch (Current)
                {
                    case '"':
                        return String();
                    case '\'':
                        return Char();
                    case '(':
                        current += 1;
                        return Token(TK.LParen);
                    case ')':
                        current += 1;
                        return Token(TK.RParen);
                    case '[':
                        current += 1;
                        return Token(TK.LSquare);
                    case ']':
                        current += 1;
                        return Token(TK.RSquare);
                    case '{':
                        current += 1;
                        return Token(TK.LBrace);
                    case '}':
                        current += 1;
                        return Token(TK.RBrace);
                    case ',':
                        current += 1;
                        return Token(TK.Comma);
                    case '~':
                        current += 1;
                        return Token(TK.Tilde);
                    case ':':
                        current += 1;
                        return Token(TK.Colon);
                    case ';':
                        current += 1;
                        return Token(TK.Semi);
                    case '?':
                        current += 1;
                        return Token(TK.Question);
                    case '|':
                        current += 1;
                        return Token(TK.Pipe);
                    case '^':
                        current += 1;
                        return Token(TK.Hat);
                    case '&':
                        current += 1;
                       return Token(TK.Amper);

                    case '.':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '>')
                            {
                                current += 1;
                                return Token(TK.Chain);
                            }
                            if (Current == '.' && current + 1 < limit && Next== '.')
                            {
                                current += 2;
                                return Token(TK.Ellipsis);
                            }
                        }
                        return Token(TK.Dot);

                    case '@':
                        current += 1;
                        if (current < limit && Current == '{')
                        {
                            current += 1;
                            return Token(TK.AtLBrace);
                        }
                        return Token(TK.At);

                    case '+':
                        current += 1;
                        return Token(TK.Plus);

                    case '-':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '>')
                            {
                                current += 1;
                                return Token(TK.Arrow);
                            }
                        }
                        return Token(TK.Minus);

                    case '/':
                        current += 1;
                        return Token(TK.Divide);

                    case '*':
                        current += 1;
                        return Token(TK.Multiply);

                    case '%':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '%')
                            {
                                current += 1;
                                return Token(TK.Mod);
                            }
                        }
                        return Token(TK.Rem);

                    case '=':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '>')
                            {
                                current += 1;
                                return Token(TK.DblArrow);
                            }
                            else if (Current == '=')
                            {
                                current += 1;
                                return Token(TK.Eq);
                            }
                        }
                        return Token(TK.Assign);

                    case '!':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '=')
                            {
                                current += 1;
                                return Token(TK.Ne);
                            }
                        }
                        return Token(TK.Exclamation);

                    case '<':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == ':')
                            {
                                current += 1;
                                return Token(TK.Subtype);
                            }
                            if (Current == '=')
                            {
                                current += 1;
                                return Token(TK.Le);
                            }
                            if (Current == '<')
                            {
                                current += 1;
                                return Token(TK.LShift);
                            }
                        }
                        return Token(TK.Lt);

                    case '>':
                        current += 1;
                        if (current < limit)
                        {
                            if (Current == '=')
                            {
                                current += 1;
                                return Token(TK.Ge);
                            }
                        }
                        return Token(TK.Gt);

                    default:
                        if (Current.IsLetterOrUnderscore())
                        {
                            return IdentifierOrKeyword();
                        }
                        if (Current.IsDigit())
                        {
                            return Number();
                        }
                        break;
                }

                Errors.AtOffset(ErrNo.Lex001, Source, current, $"unknown character ``{CharRep.InText(Current)}´´ in source stream");
                current += 1;

                nl = Skip() || nl;
            }
        }

        private bool CollectLetterAndLettersOrDigits()
        {
            var start = current;
            if (Current.IsLetter())
            {
                do
                {
                    current += 1;
                }
                while (Current.IsLetterOrDigit());
            }

            return current > start;
        }

        private bool CollectLetterOrDigit()
        {
            var start = current;
            while (Current.IsLetterOrDigit())
            {
                current += 1;
            }

            return current > start;

        }

        private Token IdentifierOrKeyword()
        {
            if (Current.IsUnderscore())
            {
                current += 1;
                var start = current;
                while (Current.IsLetterOrDigit())
                {
                    current += 1;
                }
                if (current == start)
                {
                    return Token(TK.Wildcard);
                }
            }
            else
            {
                CollectLetterAndLettersOrDigits();
            }
            while (Current.Is('_', '-'))
            {
                current += 1;
                if (CollectLetterOrDigit())
                {
                    continue;
                }
                else
                {
                    current -= 1;
                    break;
                }
            }

            while (current < limit && Current == '\'')
            {
                current += 1;
            }

            var tk = Keywords.Classify(content[payload..current]);

            return Token(tk);
        }

        private Token Number()
        {
            if (Current == '0')
            {
                current += 1;
                if (Current.Is('x', 'X'))
                {
                    current += 1;
                    // Hex
                    if (current == limit)
                    {
                        throw NoScan("EOF in hex-number");
                    }
                    if (!Current.IsHexDigit())
                    {
                        throw NoScan("expected hex-digit");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (Current.IsHexDigit());

                    return Token(TK.Integer);
                }
            }
            else
            {
                current += 1;
            }

            while (current < limit && (Current.IsDigit() || Current == '_'))
            {
                current += 1;
            }

            var floating = false;

            if (current < limit)
            {
                if (Current == '.')
                {
                    floating = true;
                    current += 1;
                    if (current == limit || !Current.IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (current < limit && Current.IsDigit());
                }

                if (current < limit && (Current == 'e' || Current == 'E'))
                {
                    floating = true;
                    current += 1;
                    if (current < limit && (Current == '-' || Current == '+'))
                    {
                        current += 1;
                    }
                    if (current == limit || !Current.IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (current < limit && Current.IsDigit());
                }
            }

            return Token(floating ? TK.Float : TK.Integer);
        }

        private Token String()
        {
            Debug.Assert(Current == '\"');

            if (Next == '"' && OverNext == '"')
            {
                return DocString();
            }

            current += 1;
            while (current < limit)
            {
                if (Current == '"')
                {
                    break;
                }
                if (Current == '\\')
                {
                    MatchEscape("string literal");
                }
                else
                {
                    current += 1;
                }
            }
            if (current == limit)
            {
                throw NoScan("unterminated string literal");
            }
            Debug.Assert(Current == '"');
            current += 1;

            return Token(TK.String);
        }

        private Token Char()
        {
            Debug.Assert(Current == '\'');
            current += 1;
            if (current == limit)
            {
                throw NoScan("unterminated character literal");
            }
            if (Current == '\\')
            {
                MatchEscape("character literal");
            }
            else
            {
                current += 1;
            }

            if (current < limit && Current == '\'')
            {
                current += 1;
                return Token(TK.Char);
            }
            throw NoScan("unterminated character literal");
        }

        private void MatchEscape(string inWhat)
        {
            Debug.Assert(Current == '\\');

            current += 1;

            if (current == limit)
            {
                throw NoScan($"incomplete escape sequence in {inWhat}");
            }

            Debug.Assert(current < limit);

            switch (Current)
            {
                case '\"':
                case '\'':
                case '\\':
                case '0':
                case 'a':
                case 'b':
                case 'e':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                case 'v':
                    current += 1;
                    break;
                case 'x':
                    current += 1;
                    MatchHexN(2, inWhat);
                    break;
                case 'u':
                    current += 1;
                    MatchHexN(4, inWhat);
                    break;
                case 'U':
                    current += 1;
                    MatchHexN(6, inWhat);
                    break;
                default:
                    throw NoScan($"illegal escape sequence in {inWhat}");
            }
        }

        private void MatchHexN(int n, string inWhat)
        {
            for (var i = 0; i < n; ++i)
            {
                if (Current.IsHexDigit())
                {
                    current += 1;
                }
                else
                {
                    throw NoScan($"expected {n} hex-digits in {inWhat}");
                }
            }
        }

        private Token DocString()
        {
            Debug.Assert(Current == '\"' && Next == '\"' && OverNext == '\"');

            current += 3;

            var done = false;
            while (!done && current + 3 <= limit)
            {
                if (Current == '"' && Next == '"' && OverNext == '"')
                {
                    current += 3;

                    while (current < limit && Current == '"')
                    {
                        current += 1;
                    }
                    done = true;
                    break;
                }
                current += 1;
            }
            if (!done)
            {
                throw NoScan("unterminated doc-string literal");
            }

            return Token(TK.String);
        }

        //
        // Helpers
        //

        private Token Token(TK kind)
        {
            return new Token(kind, Source, clutter, payload, current, nl);
        }

        private bool Skip()
        {
            var done = false;
            var nl = false;
            while (current < limit && !done)
            {
                switch (Current)
                {
                    case '\n':
                    case '\t':
                    case '\r':
                    case ' ':
                        nl = nl || Current == '\n';
                        current += 1;
                        break;
                    case '/':
                        if (current + 1 < limit)
                        {
                            if (Next == '/')
                            {
                                current += 2;
                                while (current < limit && Current != '\n')
                                {
                                    current += 1;
                                }
                                if (current < limit)
                                {
                                    nl = true;
                                }
                                break;
                            }
                            if (Next == '*')
                            {
                                nl = SkipMulitlineComment() || nl;
                                break;
                            }
                        }
                        done = true;
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return nl;
        }

        private bool SkipMulitlineComment()
        {
            Debug.Assert(Current == '/' && Next == '*');

            var nl = false;

            current += 2;
            while (current + 1 < limit && (Current != '*' || Next != '/'))
            {
                if (Current == '/' && Next == '*')
                {
                    nl = SkipMulitlineComment() || nl;
                }
                else
                {
                    nl = Current == '\n' || nl;
                    current += 1;
                }
            }
            if (current + 1 < limit)
            {
                current += 2;
            }
            else
            {
                throw NoScan("unexpected EOF in multi-line comment");
            }

            return nl;
        }

        protected JokeException NoScan(string message)
        {
            return new JokeException(new JokeError(ErrNo.NoScanToken, new AtOffset(new SourceSpan(Source, current, 0), message)));
        }
    }
}
