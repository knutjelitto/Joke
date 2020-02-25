using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        private static class First
        {
            static First()
            {
                Class = new PonyTokenSet(
                    TK.Class,
                    TK.Type,
                    TK.Interface,
                    TK.Trait,
                    TK.Primitive,
                    TK.Struct,
                    TK.Actor);
                Use = new PonyTokenSet(
                    TK.Use);
                Module = PonyTokenSet.Union(Use, Class);
                Atom = new PonyTokenSet(
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
                Prefix = new PonyTokenSet(
                    TK.Addressof,
                    TK.DigestOf,
                    TK.Not,
                    TK.Minus,
                    TK.MinusNew,
                    TK.MinusTilde,
                    TK.MinusTildeNew);
                Postfix = Atom;
                ParamPattern = PonyTokenSet.Union(Prefix, Postfix);
                Local = new PonyTokenSet(TK.Var, TK.Let, TK.Embed);
                Field = Local;
                Method = new PonyTokenSet(TK.Fun, TK.Be, TK.New);
                Pattern = PonyTokenSet.Union(ParamPattern, Local);
                Term = new PonyTokenSet(
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
                Jump = new PonyTokenSet(
                    TK.Return,
                    TK.Break,
                    TK.Continue,
                    TK.Error,
                    TK.CompileIntrinsic,
                    TK.CompileError);
                Infix = Term;
                Assignment = Infix;
                ExprSeq = Assignment;
                RawSeq = PonyTokenSet.Union(Jump, ExprSeq);
                Lambda = new PonyTokenSet(TK.LBrace, TK.AtLBrace);

                RecoverInModule = Module;
                RecoverInClass = PonyTokenSet.Union(RecoverInModule, Method, Field);
                RecoverNothing = new PonyTokenSet();
            }

            public static readonly PonyTokenSet Class;
            public static readonly PonyTokenSet Use;
            public static readonly PonyTokenSet Module;
            public static readonly PonyTokenSet Atom;
            public static readonly PonyTokenSet Prefix;
            public static readonly PonyTokenSet Postfix;
            public static readonly PonyTokenSet ParamPattern;
            public static readonly PonyTokenSet Pattern;
            public static readonly PonyTokenSet Infix;
            public static readonly PonyTokenSet Jump;
            public static readonly PonyTokenSet Local;
            public static readonly PonyTokenSet Field;
            public static readonly PonyTokenSet Method;
            public static readonly PonyTokenSet Term;
            public static readonly PonyTokenSet Assignment;
            public static readonly PonyTokenSet ExprSeq;
            public static readonly PonyTokenSet RawSeq;
            public static readonly PonyTokenSet Lambda;

            public static readonly PonyTokenSet RecoverInClass;
            public static readonly PonyTokenSet RecoverInModule;

            public static readonly PonyTokenSet RecoverNothing;
        }
    }
}
