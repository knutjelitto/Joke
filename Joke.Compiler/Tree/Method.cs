using Joke.Front.Pony.ParseTree;
using System.Diagnostics;

namespace Joke.Compiler.Tree
{
    public class Method : IClassMember
    {
        public Method(PtMethod source, IClass container, MethodKind kind, string name)
        {
            Source = source;
            Container = container;
            Kind = kind;
            Name = name;
        }

        public string Name { get; }
        public PtMethod Source { get; }
        public IClass Container { get; }
        public MethodKind Kind { get; }
        PtNode ISourced.Source => Source;

        public static Method From(PtMethod source, IClass container)
        {
            return source.Kind switch
            {
                PtMethodKind.Fun => Fun(source, container),
                PtMethodKind.Be => Be(source, container),
                PtMethodKind.New => New(source, container),
                _ => throw new System.NotImplementedException(),
            };
        }

        public static Method Fun(PtMethod source, IClass container)
        {
            Debug.Assert(source.Kind == PtMethodKind.Fun);
            return new Method(source, container, MethodKind.Fun, source.Name.Value);
        }

        public static Method Be(PtMethod source, IClass container)
        {
            Debug.Assert(source.Kind == PtMethodKind.Be);
            return new Method(source, container, MethodKind.Be, source.Name.Value);
        }

        public static Method New(PtMethod source, IClass container)
        {
            Debug.Assert(source.Kind == PtMethodKind.New);
            return new Method(source, container, MethodKind.New, source.Name.Value);
        }
    }
}
