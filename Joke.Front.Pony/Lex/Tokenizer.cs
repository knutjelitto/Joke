using Joke.Front.Pony.Err;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony.Lex
{
    public class Tokenizer
    {
        public const char NoCharacter = '\uFFFF';

        private readonly string content;
        private readonly int limit;

        /// <summary>
        /// The current index into to <see cref="content"/> array.
        /// </summary>
        public int next;

        /// <summary>
        /// Globally marks the start of the whitespace clutter prefixing a token.
        /// </summary>
        public int clutter;

        /// <summary>
        /// Globally marks the start of a token payload.
        /// </summary>
        public int payload;

        public Tokens Tokens { get; private set; }

        public Tokenizer(ErrorAccu errors, ISource source)
        {
            Errors = errors;
            Source = source;
            Tokens = new Tokens(Source, new Token[] { });

            content = Source.Content;
            limit = content.Length;
            next = 0;
            clutter = 0;
            payload = 0;
        }

        public ErrorAccu Errors { get; }
        public ISource Source { get; }

        public void Tokenize()
        {
            Tokens = new Tokens(Source, GetTokens());
        }

        private List<Token> GetTokens()
        {
            var tokens = new List<Token>();

            Token next;
            while (true)
            {
                try
                {
                    next = Next();
                    tokens.Add(next);
                    if (next.Kind == TK.Eof)
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
            clutter = next;

            var nl = Skip();

            payload = next;

            Debug.Assert(next <= limit);
            if (next == limit)
            {
                return Token(TK.Eof);
            }
            
            switch (content[next])
            {
                case '"':
                    return String();
                case '\'':
                    return Char();
                case '#':
                    return CapOrConstant();
                case '(':
                    next += 1;
                    return Token(nl ? TK.LParenNew : TK.LParen);
                case ')':
                    next += 1;
                    return Token(TK.RParen);
                case '[':
                    next += 1;
                    return Token(nl ? TK.LSquareNew : TK.LSquare);
                case ']':
                    next += 1;
                    return Token(TK.RSquare);
                case '{':
                    next += 1;
                    return Token(TK.LBrace);
                case '}':
                    next += 1;
                    return Token(TK.RBrace);
                case ',':
                    next += 1;
                    return Token(TK.Comma);
                case '~':
                    next += 1;
                    return Token(TK.Tilde);
                case ':':
                    next += 1;
                    return Token(TK.Colon);
                case ';':
                    next += 1;
                    return Token(TK.Semi);
                case '?':
                    next += 1;
                    return Token(TK.Question);
                case '|':
                    next += 1;
                    return Token(TK.Pipe);
                case '^':
                    next += 1;
                    return Token(TK.Ephemeral);
                case '&':
                    next += 1;
                    return Token(TK.ISectType);
                case '\\':
                    next += 1;
                    return Token(TK.Backslash);

                case '.':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '>')
                        {
                            next += 1;
                            return Token(TK.Chain);
                        }
                        if (content[next] == '.' && next + 1 < limit && content[next + 1] == '.')
                        {
                            next += 2;
                            return Token(TK.Ellipsis);
                        }
                    }
                    return Token(TK.Dot);

                case '@':
                    next += 1;
                    if (next < limit && content[next] == '{')
                    {
                        next += 1;
                        return Token(TK.AtLBrace);
                    }
                    return Token(TK.At);

                case '+':
                    next += 1;
                    if (next < limit && content[next] == '~')
                    {
                        next += 1;
                        return Token(TK.PlusTilde);
                    }
                    return Token(TK.Plus);

                case '-':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '~')
                        {
                            next += 1;
                            return Token(nl ? TK.MinusTildeNew : TK.MinusTilde);
                        }
                        if (content[next] == '>')
                        {
                            next += 1;
                            return Token(TK.Arrow);
                        }
                    }
                    return Token(nl ? TK.MinusNew : TK.Minus);

                case '/':
                    next += 1;
                    if (next < limit && content[next] == '~')
                    {
                        next += 1;
                        return Token(TK.DivideTilde);
                    }
                    return Token(TK.Divide);

                case '*':
                    next += 1;
                    if (next < limit && content[next] == '~')
                    {
                        next += 1;
                        return Token(TK.MultiplyTilde);
                    }
                    return Token(TK.Multiply);

                case '%':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '~')
                        {
                            next += 1;
                            return Token(TK.RemTilde);
                        }
                        if (content[next] == '%')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.ModTilde);
                            }
                            return Token(TK.Mod);
                        }
                    }
                    return Token(TK.Rem);

                case '=':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '>')
                        {
                            next += 1;
                            return Token(TK.DblArrow);
                        }
                        else if (content[next] == '=')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.EqTilde);
                            }
                            return Token(TK.Eq);
                        }
                    }
                    return Token(TK.Assign);

                case '!':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '=')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.NeTilde);
                            }
                            return Token(TK.Ne);
                        }
                    }
                    return Token(TK.Aliased);

                case '<':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == ':')
                        {
                            next += 1;
                            return Token(TK.Subtype);
                        }
                        if (content[next] == '~')
                        {
                            next += 1;
                            return Token(TK.LtTilde);
                        }
                        if (content[next] == '=')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.LeTilde);
                            }
                            return Token(TK.Le);
                        }
                        if (content[next] == '<')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.LShiftTilde);
                            }
                            return Token(TK.LShift);
                        }
                    }
                    return Token(TK.Lt);

                case '>':
                    next += 1;
                    if (next < limit)
                    {
                        if (content[next] == '~')
                        {
                            next += 1;
                            return Token(TK.Gt);
                        }
                        if (content[next] == '=')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.GeTilde);
                            }
                            return Token(TK.Ge);
                        }
                        if (content[next] == '>')
                        {
                            next += 1;
                            if (next < limit && content[next] == '~')
                            {
                                next += 1;
                                return Token(TK.RShiftTilde);
                            }
                            return Token(TK.RShift);
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

            throw NotYet("Next");
        }

        private Token IdentifierOrKeyword()
        {
            Debug.Assert(IsLetter_());
            do
            {
                next += 1;
            }
            while (next < limit && IsLetterOrDigit_());

            while (next < limit && content[next] == '\'')
            {
                next += 1;
            }

            var tk = Keywords.Classify(content.Substring(payload, next - payload));

            return Token(tk);
        }

        private Token CapOrConstant()
        {
            do
            {
                next += 1;
            }
            while (IsLetter());

            var tk = Keywords.Classify(content.Substring(payload, next - payload));

            if (tk == TK.Identifier)
            {
                next = payload + 1;
                return Token(TK.Constant);
            }

            return Token(tk);
        }

        private Token Number()
        {
            if (content[next] == '0')
            {
                next += 1;
                if (next < limit && (content[next] == 'x' || content[next] == 'X'))
                {
                    next += 1;
                    // Hex
                    if (next == limit || !IsHexDigit())
                    {
                        throw NoScan("incomplete hex number");
                    }
                    do
                    {
                        next += 1;
                    }
                    while (IsHexDigit());

                    return Token(TK.Int);
                }
            }
            else
            {
                next += 1;
            }

            while (next < limit && (IsDigit() || content[next] == '_'))
            {
                next += 1;
            }

            var floating = false;

            if (next < limit)
            {
                if (content[next] == '.')
                {
                    floating = true;
                    next += 1;
                    if (next == limit || !IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        next += 1;
                    }
                    while (next < limit && IsDigit());
                }

                if (next < limit && (content[next] == 'e' || content[next] == 'E'))
                {
                    floating = true;
                    next += 1;
                    if (next < limit && (content[next] == '-' || content[next] == '+'))
                    {
                        next += 1;
                    }
                    if (next == limit || !IsDigit())
                    {
                        throw NoScan("incomplete floating point number");
                    }
                    do
                    {
                        next += 1;
                    }
                    while (next < limit && IsDigit());
                }
            }

            return Token(floating ? TK.Float : TK.Int);
        }

        private Token String()
        {
            Debug.Assert(StartsWith("\""));

            if (next + 3 <= limit && content[next+1] == '"' && content[next+2] == '"')
            {
                return DocString();
            }

            next += 1;
            while (next < limit)
            {
                if (content[next] == '"')
                {
                    break;
                }
                if (content[next] == '\\')
                {
                    MatchEscape("string literal");
                }
                else
                {
                    next += 1;
                }
            }
            if (next == limit)
            {
                throw NoScan("unterminated string literal");
            }
            Debug.Assert(content[next] == '"');
            next += 1;

            return Token(TK.String);
        }

        private Token Char()
        {
            Debug.Assert(content[next] == '\'');
            next += 1;
            if (next == limit)
            {
                throw NoScan("unterminated character literal");
            }
            if (content[next] == '\\')
            {
                MatchEscape("character literal");
            }
            else
            {
                next += 1;
            }

            if (next < limit && content[next] == '\'')
            {
                next += 1;
                return Token(TK.Char);
            }
            throw NoScan("unterminated character literal");
        }

        private void MatchEscape(string inWhat)
        {
            Debug.Assert(content[next] == '\\');

            next += 1;

            if (next == limit)
            {
                throw NoScan($"incomplete escape sequence in {inWhat}");
            }

            Debug.Assert(next < limit);

            switch (content[next])
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
                    next += 1;
                    break;
                case 'x':
                    next += 1;
                    MatchHexN(2, inWhat);
                    break;
                case 'u':
                    next += 1;
                    MatchHexN(4, inWhat);
                    break;
                case 'U':
                    next += 1;
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
                    next += 1;
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

            next += 3;

            var done = false;
            while (!done && next + 3 <= limit)
            {
                if (content[next] == '"' && content[next+1] == '"' && content[next+2] == '"')
                {
                    next += 3;

                    while (next < limit && content[next] == '"')
                    {
                        next += 1;
                    }
                    done = true;
                    break;
                }
                next += 1;
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
            var ch = content[next];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';
        }

        private bool IsLetter_()
        {
            var ch = content[next];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
        }

        private bool IsLetterOrDigit()
        {
            var ch = content[next];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9';
        }

        private bool IsLetterOrDigit_()
        {
            var ch = content[next];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9' || ch == '_';
        }

        private bool IsDigit()
        {
            var ch = content[next];

            return '0' <= ch && ch <= '9';
        }

        private bool IsHexDigit()
        {
            var ch = content[next];

            return '0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F';
        }

        private Token Token(TK kind)
        {
            return new Token(kind, clutter, payload, next);
        }

        private bool StartsWith(string start)
        {
            return next + start.Length <= limit && content.Substring(next, start.Length) == start;
        }

        private bool Skip()
        {
            var done = false;
            var nl = false;
            while (next < limit && !done)
            {
                switch (content[next])
                {
                    case '\n':
                        nl = true;
                        next += 1;
                        break;
                    case '\t':
                    case '\r':
                    case ' ':
                        next += 1;
                        break;
                    case '/':
                        if (next + 1 < limit)
                        {
                            if (content[next + 1] == '/')
                            {
                                next += 2;
                                while (next < limit && content[next] != '\n')
                                {
                                    next += 1;
                                }
                                if (next < limit)
                                {
                                    nl = true;
                                }
                                break;
                            }
                            if (content[next + 1] == '*')
                            {
                                nl = nl | SkipMulitlineComment();
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
            Debug.Assert(next + 1 < limit && content[next] == '/' && content[next + 1] == '*');

            var nl = false;

            next += 2;
            while (next + 1 < limit && (content[next] != '*' || content[next + 1] != '/'))
            {
                if (content[next] == '/' && content[next + 1] == '*')
                {
                    nl = nl | SkipMulitlineComment();
                }
                else
                {
                    nl = nl | content[next] == '\n';
                    next += 1;
                }
            }
            if (next + 1 < limit)
            {
                next += 2;
            }
            else
            {
                throw NoScan("unexpected EOF in multi line comment");
            }

            return nl;
        }

        protected JokeException NotYet(string message)
        {
            return new JokeException(new JokeError(new AtOffset(Source, next, 0, "not implemented: " + message)));
        }

        protected JokeException NoScan(string message)
        {
            return new JokeException(new JokeError(new AtOffset(Source, next, 0, message)));
        }
    }
}
