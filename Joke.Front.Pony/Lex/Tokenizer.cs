using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
        public int index;

        /// <summary>
        /// Globally marks the start of a new token.
        /// </summary>
        public int start;

        public Tokenizer(ISource source)
        {
            Source = source;

            content = Source.Content;
            index = 0;
            limit = content.Length;
        }

        public ISource Source { get; }

        private char this[int offset] => content[index+offset];

        public IEnumerable<Token> Tokens()
        {
            Token next;
            do
            {
                next = Next();
                yield return next;
            }
            while (next.Kind != TK.Eof);
        }

        public Token Next()
        {
            var nl = Skip();

            start = index;

            Debug.Assert(index <= limit);
            if (index == limit)
            {
                return Token(TK.Eof);
            }
            
            switch (content[index])
            {
                case '"':
                    return String();
                case '\'':
                    return Char();
                case '#':
                    return CapOrConstant();
                case '(':
                    index += 1;
                    return Token(nl ? TK.LParenNew : TK.LParen);
                case ')':
                    index += 1;
                    return Token(TK.RParen);
                case '[':
                    index += 1;
                    return Token(nl ? TK.LSquareNew : TK.LSquare);
                case ']':
                    index += 1;
                    return Token(TK.RSquare);
                case '{':
                    index += 1;
                    return Token(TK.LBrace);
                case '}':
                    index += 1;
                    return Token(TK.RBrace);
                case ',':
                    index += 1;
                    return Token(TK.Comma);
                case ':':
                    index += 1;
                    return Token(TK.Colon);
                case ';':
                    index += 1;
                    return Token(TK.Semi);
                case '?':
                    index += 1;
                    return Token(TK.Question);
                case '|':
                    index += 1;
                    return Token(TK.Pipe);
                case '^':
                    index += 1;
                    return Token(TK.Ephemeral);
                case '&':
                    index += 1;
                    return Token(TK.ISectType);

                case '.':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == '>')
                        {
                            index += 1;
                            return Token(TK.Chain);
                        }
                        if (content[index] == '.' && index + 1 < limit && content[index+1] == '.')
                        {
                            index += 2;
                            return Token(TK.Ellipsis);
                        }
                    }
                    return Token(TK.Dot);

                case '@':
                    index += 1;
                    if (index < limit && content[index] == '{')
                    {
                        index += 1;
                        return Token(TK.AtLBrace);
                    }
                    return Token(TK.At);

                case '+':
                    index += 1;
                    if (index < limit && content[index] == '~')
                    {
                        index += 1;
                        return Token(TK.PlusTilde);
                    }
                    return Token(TK.Plus);

                case '-':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == '~')
                        {
                            index += 1;
                            return Token(nl ? TK.MinusTildeNew : TK.MinusTilde);
                        }
                        if (content[index] == '>')
                        {
                            index += 1;
                            return Token(TK.Arrow);
                        }
                    }
                    return Token(nl ? TK.Minus : TK.MinusNew);

                case '/':
                    index += 1;
                    if (index < limit && content[index] == '~')
                    {
                        index += 1;
                        return Token(TK.DivideTilde);
                    }
                    return Token(TK.Divide);

                case '=':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == '>')
                        {
                            index += 1;
                            return Token(TK.DblArrow);
                        }
                        else if (content[index] == '=')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
                                return Token(TK.EqTilde);
                            }
                            return Token(TK.Eq);
                        }
                    }
                    return Token(TK.Assign);

                case '!':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == '=')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
                                return Token(TK.NeTilde);
                            }
                            return Token(TK.Ne);
                        }
                    }
                    return Token(TK.Aliased);

                case '<':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == ':')
                        {
                            index += 1;
                            return Token(TK.Subtype);
                        }
                        if (content[index] == '~')
                        {
                            index += 1;
                            return Token(TK.LtTilde);
                        }
                        if (content[index] == '=')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
                                return Token(TK.LeTilde);
                            }
                            return Token(TK.Le);
                        }
                        if (content[index] == '<')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
                                return Token(TK.LShiftTilde);
                            }
                            return Token(TK.LShift);
                        }
                    }
                    return Token(TK.Lt);

                case '>':
                    index += 1;
                    if (index < limit)
                    {
                        if (content[index] == '~')
                        {
                            index += 1;
                            return Token(TK.Gt);
                        }
                        if (content[index] == '=')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
                                return Token(TK.GeTilde);
                            }
                            return Token(TK.Ge);
                        }
                        if (content[index] == '>')
                        {
                            index += 1;
                            if (index < limit && content[index] == '~')
                            {
                                index += 1;
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
            do
            {
                index += 1;
            }
            while (IsLetterOrDigit_());

            while (content[index] == '\'')
            {
                index += 1;
            }

            var tk = Keywords.Classify(content.Substring(start, index - start));

            return Token(tk);
        }

        private Token CapOrConstant()
        {
            do
            {
                index += 1;
            }
            while (IsLetter());

            var tk = Keywords.Classify(content.Substring(start, index - start));

            if (tk == TK.Identifier)
            {
                index = start + 1;
                return Token(TK.Constant);
            }

            return Token(tk);
        }

        private Token Number()
        {
            if (content[index] == '0')
            {
                index += 1;
                if (index < limit)
                {
                    if (content[index] == 'x' || content[index] == 'X')
                    {
                        index += 1;
                        // Hex
                        if (index == limit || !IsHexDigit())
                        {
                            throw NoScan("incomplete hex number");
                        }
                        do
                        {
                            index += 1;
                        }
                        while (IsHexDigit());

                        return Token(TK.Int);
                    }
                }
            }
            else
            {
                index += 1;
            }

            while (index < limit && IsDigit())
            {
                index += 1;
            }

            if (IsLetter() || content[index] == '.')
            {
                throw NotYet("complicated number");
            }

            return Token(TK.Int);
        }

        private Token String()
        {
            Debug.Assert(StartsWith("\""));

            if (index + 3 <= limit && content[index+1] == '"' && content[index+2] == '"')
            {
                return DocString();
            }

            index += 1;
            while (index < limit)
            {
                if (content[index] == '"')
                {
                    break;
                }
                if (content[index] == '\\')
                {
                    MatchEscape("string literal");
                }
                else
                {
                    index += 1;
                }
            }
            if (index == limit)
            {
                throw NoScan("unterminated string literal");
            }
            Debug.Assert(content[index] == '"');
            index += 1;

            return Token(TK.String);
        }

        private Token Char()
        {
            Debug.Assert(content[index] == '\'');
            index += 1;
            if (index == limit)
            {
                throw NoScan("unterminated character literal");
            }
            if (content[index] == '\\')
            {
                MatchEscape("character literal");
            }
            else
            {
                index += 1;
            }

            if (index < limit && content[index] == '\'')
            {
                index += 1;
                return Token(TK.Char);
            }
            throw NoScan("unterminated character literal");
        }

        private void MatchEscape(string inWhat)
        {
            index += 1;

            if (index == limit)
            {
                throw NoScan($"incomplete escape sequence in {inWhat}");
            }

            Debug.Assert(index < limit);

            switch (content[index])
            {
                case '\"':
                case '\'':
                case '\\':
                case 'a':
                case 'b':
                case 'e':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                case 'v':
                    index += 1;
                    break;
                case 'x':
                    index += 1;
                    MatchHexN(2, inWhat);
                    break;
                case 'u':
                    index += 1;
                    MatchHexN(4, inWhat);
                    break;
                case 'U':
                    index += 1;
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
                    index += 1;
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

            index += 3;

            var done = false;
            while (!done && index + 3 <= limit)
            {
                if (content[index] == '"' && content[index+1] == '"' && content[index+2] == '"')
                {
                    index += 3;

                    while (index < limit && content[index] == '"')
                    {
                        index += 1;
                    }
                    done = true;
                    break;
                }
                index += 1;
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
            var ch = content[index];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';
        }

        private bool IsLetter_()
        {
            var ch = content[index];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
        }

        private bool IsLetterOrDigit()
        {
            var ch = content[index];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9';
        }

        private bool IsLetterOrDigit_()
        {
            var ch = content[index];

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9' || ch == '_';
        }

        private bool IsDigit()
        {
            var ch = content[index];

            return '0' <= ch && ch <= '9';
        }

        private bool IsHexDigit()
        {
            var ch = content[index];

            return '0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F';
        }

        private Token Token(TK kind)
        {
            return new Token(start, index - start, kind);
        }

        private bool StartsWith(string start)
        {
            return index + start.Length <= limit && content.Substring(index, start.Length) == start;
        }

        private bool Skip()
        {
            var done = false;
            var nl = false;
            while (index < limit && !done)
            {
                switch (content[index])
                {
                    case '\n':
                        nl = true;
                        index += 1;
                        break;
                    case '\t':
                    case '\r':
                    case ' ':
                        index += 1;
                        break;
                    case '/':
                        if (index + 1 < limit)
                        {
                            if (content[index + 1] == '/')
                            {
                                index += 2;
                                while (index < limit && content[index] != '\n')
                                {
                                    index += 1;
                                }
                                if (index < limit)
                                {
                                    nl = true;
                                }
                                break;
                            }
                            if (content[index + 1] == '*')
                            {
                                index += 2;
                                while (index + 1 < limit && (content[index] != '*' || content[index + 1] != '/'))
                                {
                                    nl = nl || content[index] == '\n';
                                    index += 1;
                                }
                                if (index + 1 < limit)
                                {
                                    index += 2;
                                }
                                else
                                {
                                    throw NoScan("unexpected EOF in multi line comment");
                                }
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

        protected NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        protected NoScanException NoScan(string message)
        {
            return new NoScanException(message);
        }

    }
}
