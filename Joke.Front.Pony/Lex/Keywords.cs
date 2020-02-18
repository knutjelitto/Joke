using System.Collections.Generic;

namespace Joke.Front.Pony.Lex
{
    public class Keywords
    {
        public static TK Classify(string maybeKeyword)
        {
            if (keywords.TryGetValue(maybeKeyword, out var tk))
            {
                return tk;
            }

            return TK.Identifier;
        }

        public static string String(TK kind)
        {
            if (invers == null)
            {
                invers = new Dictionary<TK, string>();
                foreach (var pair in keywords)
                {
                    invers.Add(pair.Value, pair.Key);
                }

                invers.Add(TK.LParenNew, invers[TK.LParen]);
                invers.Add(TK.LSquareNew, invers[TK.LSquare]);
                invers.Add(TK.MinusNew, invers[TK.Minus]);
                invers.Add(TK.MinusTildeNew, invers[TK.MinusTilde]);
            }

            return invers[kind];
        }

        private static Dictionary<TK, string>? invers = null;

        private static Dictionary<string, TK> keywords = new Dictionary<string, TK>
        {
            { "not", TK.Not },
            { "and", TK.And },
            { "or", TK.Or },
            { "xor", TK.Xor },

            { "true", TK.True },
            { "false", TK.False },

            { "addressof", TK.Addressof },
            { "digestof", TK.DigestOf },
            { "__loc", TK.Location },

            { "actor", TK.Actor },
            { "as", TK.As },
            { "be", TK.Be },
            { "box", TK.Box},
            { "break", TK.Break },
            { "class", TK.Class },
            { "compile_error", TK.CompileError },
            { "compile_intrinsic", TK.CompileIntrinsic },
            { "continue", TK.Continue },
            { "consume", TK.Consume },
            { "do", TK.Do },
            { "else", TK.Else },
            { "elseif", TK.Elseif },
            { "embed", TK.Embed },
            { "end", TK.End },
            { "error", TK.Error },
            { "for", TK.For },
            { "fun", TK.Fun },
            { "if", TK.If },
            { "ifdef", TK.Ifdef },
            { "iftype", TK.Iftype },
            { "in", TK.In },
            { "interface", TK.Interface },
            { "is", TK.Is },
            { "isnt", TK.Isnt },
            { "iso", TK.Iso },
            { "lambda", TK.Lambda },
            { "let", TK.Let },
            { "match", TK.Match },
            { "new", TK.New },
            { "object", TK.Object },
            { "primitive", TK.Primitive },
            { "recover", TK.Recover },
            { "ref", TK.Ref },
            { "repeat", TK.Repeat },
            { "return", TK.Return },
            { "struct", TK.Struct },
            { "tag", TK.Tag },
            { "then", TK.Then },
            { "this", TK.This },
            { "trait", TK.Trait },
            { "trn", TK.Trn },
            { "try", TK.Try },
            { "type", TK.Type },
            { "until", TK.Until },
            { "use", TK.Use },
            { "var", TK.Var },
            { "val", TK.Val },
            { "where", TK.Where },
            { "while", TK.While },
            { "with", TK.With },
            { "#read", TK.CapRead },
            { "#send", TK.CapSend },
            { "#share", TK.CapShare },
            { "#alias", TK.CapAlias },
            { "#any", TK.CapAny },

            // add punctuation/operator for inverse lookup
            { "{", TK.LBrace },
            { "}", TK.RBrace },
            { "(", TK.LParen },
            { ")", TK.RParen },
            { "[", TK.LSquare },
            { "]", TK.RSquare },
            { ",", TK.Comma },
            { "~", TK.Tilde },
            { ":", TK.Colon },
            { ";", TK.Semi },
            { "?", TK.Question },
            { "|", TK.Pipe },
            { "^", TK.Ephemeral },
            { "&", TK.ISectType },
            { "#", TK.Constant },
            { "\\", TK.Backslash },
            { "@", TK.At },
            { "@{", TK.AtLBrace },
            { ".", TK.Dot },
            { ".>", TK.Chain },
            { "...", TK.Ellipsis },
            { "=", TK.Assign },
            { "=>", TK.DblArrow },
            { "==", TK.Eq },
            { "==~", TK.EqTilde },
            { "!", TK.Aliased },
            { "!=", TK.Ne },
            { "!=~", TK.NeTilde },
            { "<", TK.Lt },
            { "<:", TK.Subtype },
            { "<~", TK.LtTilde },
            { "<=", TK.Le },
            { "<=~", TK.LeTilde },
            { "<<", TK.LShift },
            { "<<~", TK.LShiftTilde },
            { ">", TK.Gt },
            { ">~", TK.GtTilde },
            { ">=", TK.Ge },
            { ">=~", TK.GeTilde },
            { ">>", TK.RShift },
            { ">>~", TK.RShiftTilde },
            { "+", TK.Plus },
            { "+~", TK.PlusTilde },
            { "-", TK.Minus },
            { "-~", TK.MinusTilde },
            { "->", TK.Arrow },
            { "/", TK.Divide },
            { "/~", TK.DivideTilde },
            { "*", TK.Multiply },
            { "*~", TK.MultiplyTilde },
            { "%", TK.Rem },
            { "%~", TK.RemTilde },
            { "%%", TK.Mod },
            { "%%~", TK.ModTilde },
        };
    }
}
