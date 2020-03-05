using System.Collections.Generic;

namespace Joke.Joke.Decoding
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

                invers.Add(TK.String, "string");
                invers.Add(TK.DocString, "doc-string");
                invers.Add(TK.Identifier, "identifier");
                invers.Add(TK.Integer, "integer");
                invers.Add(TK.Char, "character");
                invers.Add(TK.Float, "float");
            }

            return invers[kind];
        }

        private static Dictionary<TK, string>? invers = null;

        private static Dictionary<string, TK> keywords = new Dictionary<string, TK>
        {
            { "actor", TK.Actor },
            { "addressof", TK.Addressof },
            { "and", TK.And },
            { "as", TK.As },
            { "be", TK.Be },
            { "break", TK.Break },
            { "class", TK.Class },
            { "compile_error", TK.CompileError },
            { "compile_intrinsic", TK.CompileIntrinsic },
            { "continue", TK.Continue },
            { "digestof", TK.Digestof },
            { "do", TK.Do },
            { "else", TK.Else },
            { "elseif", TK.Elseif },
            { "embed", TK.Embed },
            { "end", TK.End },
            { "error", TK.Error },
            { "extern", TK.Extern },
            { "false", TK.False },
            { "for", TK.For },
            { "fun", TK.Fun },
            { "if", TK.If },
            { "in", TK.In },
            { "interface", TK.Interface },
            { "is", TK.Is },
            { "isnt", TK.Isnt },
            { "lambda", TK.Lambda },
            { "let", TK.Let },
            { "__loc", TK.Loc },
            { "match", TK.Match },
            { "namespace", TK.Namespace },
            { "new", TK.New },
            { "not", TK.Not },
            { "object", TK.Object },
            { "or", TK.Or },
            { "primitive", TK.Primitive },
            { "repeat", TK.Repeat },
            { "return", TK.Return },
            { "struct", TK.Struct },
            { "then", TK.Then },
            { "this", TK.This },
            { "trait", TK.Trait },
            { "true", TK.True },
            { "try", TK.Try },
            { "type", TK.Type },
            { "until", TK.Until },
            { "use", TK.Use },
            { "var", TK.Var },
            { "when", TK.When },
            { "where", TK.Where },
            { "while", TK.While },
            { "with", TK.With },
            { "xor", TK.Xor },

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
            { "^", TK.Hat },
            { "&", TK.Amper },
            { "#", TK.Hash },
            { "@", TK.At },
            { "@{", TK.AtLBrace },
            { ".", TK.Dot },
            { ".>", TK.Chain },
            { "...", TK.Ellipsis },
            { "=", TK.Assign },
            { "=>", TK.DblArrow },
            { "==", TK.Eq },
            { "!", TK.Exclamation },
            { "!=", TK.Ne },
            { "<", TK.Lt },
            { "<:", TK.Subtype },
            { "<=", TK.Le },
            { "<<", TK.LShift },
            { ">", TK.Gt },
            { ">=", TK.Ge },
            { ">>", TK.RShift },
            { "+", TK.Plus },
            { "-", TK.Minus },
            { "->", TK.Arrow },
            { "/", TK.Divide },
            { "*", TK.Multiply },
            { "%", TK.Rem },
            { "%%", TK.Mod },
        };
    }
}
