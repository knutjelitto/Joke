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
            Tokens = new Tokens(Source, new Token[] { });

            content = Source.Content;
            limit = content.Length;
            current = 0;
            clutter = 0;
            payload = 0;
        }

        public Errors Errors { get; }
        public ISource Source { get; }

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
                    token = Next();
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

        private Token Next()
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

                switch (content[current])
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
                            if (content[current] == '>')
                            {
                                current += 1;
                                return Token(TK.Chain);
                            }
                            if (content[current] == '.' && current + 1 < limit && content[current + 1] == '.')
                            {
                                current += 2;
                                return Token(TK.Ellipsis);
                            }
                        }
                        return Token(TK.Dot);

                    case '@':
                        current += 1;
                        if (current < limit && content[current] == '{')
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
                            if (content[current] == '>')
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
                            if (content[current] == '%')
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
                            if (content[current] == '>')
                            {
                                current += 1;
                                return Token(TK.DblArrow);
                            }
                            else if (content[current] == '=')
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
                            if (content[current] == '=')
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
                            if (content[current] == ':')
                            {
                                current += 1;
                                return Token(TK.Subtype);
                            }
                            if (content[current] == '=')
                            {
                                current += 1;
                                return Token(TK.Le);
                            }
                            if (content[current] == '<')
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
                            if (content[current] == '=')
                            {
                                current += 1;
                                return Token(TK.Ge);
                            }
                        }
                        return Token(TK.Gt);

                    default:
                        if (IsLetter_())
                        {
                            return IdentifierOrKeyword();
                        }
                        if (IsDigit())
                        {
                            return Number();
                        }
                        break;
                }

                Errors.AtOffset(ErrNo.Lex001, Source, current, $"unknown character ``{CharRep.InText(content[current])}´´ in source stream");
                current += 1;

                nl = Skip() || nl;
            }
        }

        private bool LetterAndLetterOrDigit()
        {
            var start = current;
            if (IsLetter())
            {
                do
                {
                    current += 1;
                }
                while (IsLetterOrDigit());
            }

            return current > start;
        }

        private bool LetterAndLetter()
        {
            var start = current;
            if (IsLetter())
            {
                do
                {
                    current += 1;
                }
                while (IsLetterOrDigit());
            }

            return current > start;
        }

        private bool LetterOrDigit()
        {
            var start = current;
            while (IsLetterOrDigit())
            {
                current += 1;
            }

            return current > start;

        }

        private Token IdentifierOrKeyword()
        {
            if (Is_())
            {
                current += 1;
                var start = current;
                while (IsLetterOrDigit())
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
                LetterAndLetterOrDigit();
            }
            while (Is('_', '-'))
            {
                current += 1;
                if (LetterOrDigit())
                {
                    continue;
                }
                else
                {
                    current -= 1;
                    break;
                }
            }

            while (current < limit && content[current] == '\'')
            {
                current += 1;
            }

            var tk = Keywords.Classify(content[payload..current]);

            return Token(tk);
        }

        private Token Number()
        {
            if (content[current] == '0')
            {
                current += 1;
                if (current < limit && (content[current] == 'x' || content[current] == 'X'))
                {
                    current += 1;
                    // Hex
                    if (current == limit || !IsHexDigit())
                    {
                        throw NoScan("incomplete hex number");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (IsHexDigit());

                    return Token(TK.Integer);
                }
            }
            else
            {
                current += 1;
            }

            while (current < limit && (IsDigit() || content[current] == '_'))
            {
                current += 1;
            }

            var floating = false;

            if (current < limit)
            {
                if (content[current] == '.')
                {
                    floating = true;
                    current += 1;
                    if (current == limit || !IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (current < limit && IsDigit());
                }

                if (current < limit && (content[current] == 'e' || content[current] == 'E'))
                {
                    floating = true;
                    current += 1;
                    if (current < limit && (content[current] == '-' || content[current] == '+'))
                    {
                        current += 1;
                    }
                    if (current == limit || !IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        current += 1;
                    }
                    while (current < limit && IsDigit());
                }
            }

            return Token(floating ? TK.Float : TK.Integer);
        }

        private Token String()
        {
            Debug.Assert(StartsWith("\""));

            if (current + 3 <= limit && content[current+1] == '"' && content[current+2] == '"')
            {
                return DocString();
            }

            current += 1;
            while (current < limit)
            {
                if (content[current] == '"')
                {
                    break;
                }
                if (content[current] == '\\')
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
            Debug.Assert(content[current] == '"');
            current += 1;

            return Token(TK.String);
        }

        private Token Char()
        {
            Debug.Assert(content[current] == '\'');
            current += 1;
            if (current == limit)
            {
                throw NoScan("unterminated character literal");
            }
            if (content[current] == '\\')
            {
                MatchEscape("character literal");
            }
            else
            {
                current += 1;
            }

            if (current < limit && content[current] == '\'')
            {
                current += 1;
                return Token(TK.Char);
            }
            throw NoScan("unterminated character literal");
        }

        private void MatchEscape(string inWhat)
        {
            Debug.Assert(content[current] == '\\');

            current += 1;

            if (current == limit)
            {
                throw NoScan($"incomplete escape sequence in {inWhat}");
            }

            Debug.Assert(current < limit);

            switch (content[current])
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
                if (IsHexDigit())
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
            Debug.Assert(StartsWith("\"\"\""));

            current += 3;

            var done = false;
            while (!done && current + 3 <= limit)
            {
                if (content[current] == '"' && content[current+1] == '"' && content[current+2] == '"')
                {
                    current += 3;

                    while (current < limit && content[current] == '"')
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

            return Token(TK.DocString);
        }

        //
        // Helpers
        //

        private bool IsLetter()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';
        }

        private bool IsLetter_()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
        }

        private bool IsLetterOrDigit()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9';
        }

        private bool IsLetterOrDigit_()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9' || ch == '_';
        }

        private bool Is_()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return ch == '_';
        }

        private bool Is(char ch1, char ch2)
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return ch == ch1 || ch == ch2; ;
        }

        private bool IsDigit()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return '0' <= ch && ch <= '9';
        }

        private bool IsHexDigit()
        {
            var ch = current < limit ? content[current] : NoCharacter;

            return '0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F';
        }

        private Token Token(TK kind)
        {
            return new Token(kind, Source, clutter, payload, current, nl);
        }

        private bool StartsWith(string start)
        {
            return current + start.Length <= limit && content.Substring(current, start.Length) == start;
        }

        private bool Skip()
        {
            var done = false;
            var nl = false;
            while (current < limit && !done)
            {
                switch (content[current])
                {
                    case '\n':
                        nl = true;
                        current += 1;
                        break;
                    case '\t':
                    case '\r':
                    case ' ':
                        current += 1;
                        break;
                    case '/':
                        if (current + 1 < limit)
                        {
                            if (content[current + 1] == '/')
                            {
                                current += 2;
                                while (current < limit && content[current] != '\n')
                                {
                                    current += 1;
                                }
                                if (current < limit)
                                {
                                    nl = true;
                                }
                                break;
                            }
                            if (content[current + 1] == '*')
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
            Debug.Assert(current + 1 < limit && content[current] == '/' && content[current + 1] == '*');

            var nl = false;

            current += 2;
            while (current + 1 < limit && (content[current] != '*' || content[current + 1] != '/'))
            {
                if (content[current] == '/' && content[current + 1] == '*')
                {
                    nl = SkipMulitlineComment() || nl;
                }
                else
                {
                    nl = content[current] == '\n' || nl;
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
