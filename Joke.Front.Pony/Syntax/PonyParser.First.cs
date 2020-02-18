    using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private static class First
        {
            public static readonly TK[] Class = new TK[]
            {
                TK.Type,
                TK.Interface,
                TK.Trait,
                TK.Primitive,
                TK.Struct,
                TK.Class,
                TK.Actor
            };

            public static readonly TK[] RawSeq = new TK[]
            {
                TK.If,
                TK.Ifdef,
                TK.Iftype,
                TK.Match,
                TK.While,
                TK.Repeat,
                TK.For,
                TK.With,
                TK.Try,
                TK.Recover,
                TK.Consume,
                TK.Constant,
                TK.Var,
                TK.Let,
                TK.Embed,
                TK.Addressof,
                TK.DigestOf,
                TK.Not,
                TK.Minus,
                TK.MinusNew,
                TK.MinusTilde,
                TK.MinusTildeNew,
                TK.Identifier,
                TK.This,
                TK.String,
                TK.DocString,
                TK.Char,
                TK.Int,
                TK.Float,
                TK.True,
                TK.False,
                TK.LParen,
                TK.LParenNew,
                TK.LSquare,
                TK.LSquareNew,
                TK.Object,
                TK.LBrace,
                TK.AtLBrace,
                TK.At,
                TK.Location,
                TK.If,
                TK.While,
                TK.For,
        };
        }
    }
}
