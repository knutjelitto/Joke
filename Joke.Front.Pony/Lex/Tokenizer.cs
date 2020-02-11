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
                case '(':
                    index += 1;
                    return Token(nl ? TK.LParenNew : TK.LParen);
                case ')':
                    index += 1;
                    return Token(TK.RParen);
                case ':':
                    index += 1;
                    return Token(TK.Colon);

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
                default:
                    if (IsLetter_())
                    {
                        return IdentifierOrKeyword();
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

            var tk = Keywords.Classify(content.Substring(start, index - start));

            return Token(tk);
        }

        private Token String()
        {
            Debug.Assert(StartsWith("\""));

            if (index + 3 <= limit && content[1] == '"' && content[2] == '"')
            {
                return DocString();
            }


            throw NotYet("string");
        }

        private Token DocString()
        {
            Debug.Assert(StartsWith("\"\"\""));

            index += 3;

            var done = false;
            while (!done && index + 3 <= limit)
            {
                if (content[0] == '"' && content[1] == '"' && content[2] == '"')
                {
                    index += 3;

                    while (index < limit && content[0] == '"')
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

            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_' || '0' <= ch && ch <= '9';
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
