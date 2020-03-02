using Joke.Front.Pony.ParseTree;
using System.Diagnostics;

namespace Joke.Compiler.Tree
{
    public class Field : IClassMember
    {
        public Field(PtField source, IClass container, FieldKind kind, string name)
        {
            Source = source;
            Container = container;
            Kind = kind;
            Name = name;
        }

        public string Name { get; }
        public PtField Source { get; }
        public IClass Container { get; }
        public FieldKind Kind { get; }
        PtNode ISourced.Source => Source;

        public static Field From(PtField source, IClass container)
        {
            return source.Kind switch
            {
                PtFieldKind.Var => Var(source, container),
                PtFieldKind.Let => Let(source, container),
                PtFieldKind.Embed => Embed(source, container),
                _ => throw new System.NotImplementedException(),
            };
        }

        public static Field Var(PtField source, IClass container)
        {
            Debug.Assert(source.Kind == PtFieldKind.Var);
            return new Field(source, container, FieldKind.Var, source.Name.Value);
        }

        public static Field Let(PtField source, IClass container)
        {
            Debug.Assert(source.Kind == PtFieldKind.Let);
            return new Field(source, container, FieldKind.Let, source.Name.Value);
        }

        public static Field Embed(PtField source, IClass container)
        {
            Debug.Assert(source.Kind == PtFieldKind.Embed);
            return new Field(source, container, FieldKind.Embed, source.Name.Value);
        }
    }
}