using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private static class First
        {
            static First()
            {
                Class = new TokenSet(
                    TK.Class,
                    TK.Type,
                    TK.Interface,
                    TK.Trait,
                    TK.Primitive,
                    TK.Struct,
                    TK.Actor);
                Use = new TokenSet(
                    TK.Use);
                Module = TokenSet.Union(Use, Class);
                Atom = new TokenSet(
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
                    TK.For);
                Prefix = new TokenSet(
                    TK.Addressof,
                    TK.DigestOf,
                    TK.Not,
                    TK.Minus,
                    TK.MinusNew,
                    TK.MinusTilde,
                    TK.MinusTildeNew);
                Postfix = Atom;
                ParamPattern = TokenSet.Union(Prefix, Postfix);
                Local = new TokenSet(TK.Var, TK.Let, TK.Embed);
                Field = Local;
                Method = new TokenSet(TK.Fun, TK.Be, TK.New);
                Pattern = TokenSet.Union(ParamPattern, Local);
                Term = new TokenSet(
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
                    TK.Constant)
                    .Union(Pattern);
                Jump = new TokenSet(
                    TK.Return,
                    TK.Break,
                    TK.Continue,
                    TK.Error,
                    TK.CompileIntrinsic,
                    TK.CompileError);
                Infix = Term;
                Assignment = Infix;
                ExprSeq = Assignment;
                RawSeq = TokenSet.Union(Jump, ExprSeq);
                Lambda = new TokenSet(TK.LBrace, TK.AtLBrace);
            }

            public static readonly TokenSet Class;
            public static readonly TokenSet Use;
            public static readonly TokenSet Module;
            public static readonly TokenSet Atom;
            public static readonly TokenSet Prefix;
            public static readonly TokenSet Postfix;
            public static readonly TokenSet ParamPattern;
            public static readonly TokenSet Pattern;
            public static readonly TokenSet Infix;
            public static readonly TokenSet Jump;
            public static readonly TokenSet Local;
            public static readonly TokenSet Field;
            public static readonly TokenSet Method;
            public static readonly TokenSet Term;
            public static readonly TokenSet Assignment;
            public static readonly TokenSet ExprSeq;
            public static readonly TokenSet RawSeq;
            public static readonly TokenSet Lambda;
        }
    }
}
