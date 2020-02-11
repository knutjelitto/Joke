using System;
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

        private static Dictionary<string, TK> keywords = new Dictionary<string, TK>
        {
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
            { "digestof", TK.DigestOf },
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
            { "in", TK.In },
            { "interface", TK.Interface },
            { "is", TK.Is },
            { "isnt", TK.Isnt },
            { "iso", TK.Iso },
            { "lambda", TK.Lambda },
            { "let", TK.Let },
            { "match", TK.Match },
            { "new", TK.New },
            { "not", TK.Not },
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
        };
    }
}
