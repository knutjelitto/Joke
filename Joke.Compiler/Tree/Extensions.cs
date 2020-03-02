using Joke.Compiler.Compile;
using Joke.Front.Err;

namespace Joke.Compiler.Tree
{
    public static class Extensions
    {
        public static CompileContext Context(this Package package)
        {
            return package.Context;
        }

        public static CompileContext Context(this Unit unit)
        {
            return unit.Package.Context();
        }

        public static Errors Errors(this Package package)
        {
            return package.Context().Errors;
        }

        public static Errors Errors(this Unit unit)
        {
            return unit.Context().Errors;
        }

        public static void NoFieldAllowed(this Errors errors, ISourced sourced, string context)
        {
            errors.Help.Add(
                sourced.Source.Span,
                ErrNo.Err006,
                $"no field allowed in ``{context}´´");
        }

        public static void NoBehaviourAllowed(this Errors errors, ISourced sourced, string context)
        {
            errors.Help.Add(
                sourced.Source.Span,
                ErrNo.Err007,
                $"no behaviour allowed in ``{context}´´");
        }

        public static void NoMemberAllowed(this Errors errors, ISourced sourced, string context)
        {
            errors.Help.Add(
                sourced.Source.Span,
                ErrNo.Err008,
                $"no member allowed in ``{context}´´");
        }
    }
}
