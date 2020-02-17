using Joke.Front.Pony.Ast;
using System.Collections.Generic;

namespace Joke.Front.Pony
{
    public class Visitor
    {
        private readonly Reflect reflect = new Reflect();
        private readonly Node root;

        public Visitor(Node root)
        {
            this.root = root;
        }

        public void Visit()
        {
            Dispatch(root);
        }

        private void Visit(Node node)
        {
            throw new System.NotImplementedException($"node type `{node.GetType().Name}´ not implemented");
        }

        private void Dispatch(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                Dispatch(node);
            }
        }

        private void Dispatch(Node node)
        {
            Visit(node as dynamic);
        }

        protected virtual void VisitChildren(Node node)
        {
            foreach (var child in reflect.Or(node))
            {
                Dispatch(child);
            }
        }

        protected virtual void Visit(Module node) => VisitChildren(node);
        protected virtual void Visit(UseFfi node) => VisitChildren(node);
        protected virtual void Visit(FfiName node) => VisitChildren(node);
        protected virtual void Visit(Identifier node) => VisitChildren(node);
        protected virtual void Visit(TypeArguments node) => VisitChildren(node);
        protected virtual void Visit(TypeArgumentType node) => VisitChildren(node);
        protected virtual void Visit(NominalType node) => VisitChildren(node);
        protected virtual void Visit(EphemAlias node) => VisitChildren(node);
        protected virtual void Visit(Parameters node) => VisitChildren(node);
        protected virtual void Visit(RegularParameter node) => VisitChildren(node);
        protected virtual void Visit(ColonType node) => VisitChildren(node);
        protected virtual void Visit(Cap node) => VisitChildren(node);
        protected virtual void Visit(Class node) => VisitChildren(node);
        protected virtual void Visit(String node) => VisitChildren(node);
        protected virtual void Visit(Members node) => VisitChildren(node);
        protected virtual void Visit(Fields node) => VisitChildren(node);
        protected virtual void Visit(Methods node) => VisitChildren(node);
        protected virtual void Visit(Method node) => VisitChildren(node);
        protected virtual void Visit(Provides node) => VisitChildren(node);
        protected virtual void Visit(Body node) => VisitChildren(node);
        protected virtual void Visit(Ref node) => VisitChildren(node);
        protected virtual void Visit(TypeParameters node) => VisitChildren(node);
        protected virtual void Visit(TypeParameter node) => VisitChildren(node);
        protected virtual void Visit(GroupedType node) => VisitChildren(node);
        protected virtual void Visit(InfixType node) => VisitChildren(node);
        protected virtual void Visit(Postfix node) => VisitChildren(node);
        protected virtual void Visit(Dot node) => VisitChildren(node);
        protected virtual void Visit(Call node) => VisitChildren(node);
        protected virtual void Visit(Arguments node) => VisitChildren(node);
        protected virtual void Visit(Int node) => VisitChildren(node);
        protected virtual void Visit(Sequence node) => VisitChildren(node);
        protected virtual void Visit(Assignment node) => VisitChildren(node);
        protected virtual void Visit(Local node) => VisitChildren(node);
        protected virtual void Visit(GroupedExpression node) => VisitChildren(node);
        protected virtual void Visit(BinaryOp node) => VisitChildren(node);
        protected virtual void Visit(ThisLiteral node) => VisitChildren(node);
        protected virtual void Visit(Iff node) => VisitChildren(node);
        protected virtual void Visit(Then node) => VisitChildren(node);
        protected virtual void Visit(Else node) => VisitChildren(node);
        protected virtual void Visit(FfiCall node) => VisitChildren(node);
        protected virtual void Visit(PositionalArgument node) => VisitChildren(node);
        protected virtual void Visit(Bool node) => VisitChildren(node);
        protected virtual void Visit(Qualify node) => VisitChildren(node);
        protected virtual void Visit(Jump node) => VisitChildren(node);
        protected virtual void Visit(While node) => VisitChildren(node);
        protected virtual void Visit(Match node) => VisitChildren(node);
        protected virtual void Visit(Cases node) => VisitChildren(node);
        protected virtual void Visit(Case node) => VisitChildren(node);
        protected virtual void Visit(UnaryOp node) => VisitChildren(node);
        protected virtual void Visit(Recover node) => VisitChildren(node);
        protected virtual void Visit(Try node) => VisitChildren(node);
        protected virtual void Visit(Char node) => VisitChildren(node);
        protected virtual void Visit(Chain node) => VisitChildren(node);
        protected virtual void Visit(UseUri node) => VisitChildren(node);
        protected virtual void Visit(Field node) => VisitChildren(node);
        protected virtual void Visit(DefaultArg node) => VisitChildren(node);
        protected virtual void Visit(For node) => VisitChildren(node);
        protected virtual void Visit(IdsSingle node) => VisitChildren(node);
        protected virtual void Visit(DefaultType node) => VisitChildren(node);
        protected virtual void Visit(As node) => VisitChildren(node);
        protected virtual void Visit(ViewpointType node) => VisitChildren(node);
        protected virtual void Visit(ArrowType node) => VisitChildren(node);
        protected virtual void Visit(Consume node) => VisitChildren(node);
        protected virtual void Visit(SubType node) => VisitChildren(node);
        protected virtual void Visit(SemiExpression node) => VisitChildren(node);
        protected virtual void Visit(Object node) => VisitChildren(node);
        protected virtual void Visit(NamedArgument node) => VisitChildren(node);
        protected virtual void Visit(Lambda node) => VisitChildren(node);
        protected virtual void Visit(LambdaParameters node) => VisitChildren(node);
        protected virtual void Visit(LambdaParameter node) => VisitChildren(node);
        protected virtual void Visit(Tilde node) => VisitChildren(node);
        protected virtual void Visit(IdsMulti node) => VisitChildren(node);
        protected virtual void Visit(Array node) => VisitChildren(node);
        protected virtual void Visit(LambdaCaptures node) => VisitChildren(node);
        protected virtual void Visit(LambdaCaptureName node) => VisitChildren(node);
        protected virtual void Visit(EllipsisParameter node) => VisitChildren(node);
        protected virtual void Visit(LambdaType node) => VisitChildren(node);
        protected virtual void Visit(LambdaTypeParameters node) => VisitChildren(node);
        protected virtual void Visit(Guard node) => VisitChildren(node);
        protected virtual void Visit(ArrayType node) => VisitChildren(node);
        protected virtual void Visit(Float node) => VisitChildren(node);
        protected virtual void Visit(ThisType node) => VisitChildren(node);
        protected virtual void Visit(With node) => VisitChildren(node);
        protected virtual void Visit(WithElements node) => VisitChildren(node);
        protected virtual void Visit(WithElement node) => VisitChildren(node);
        protected virtual void Visit(Repeat node) => VisitChildren(node);
        protected virtual void Visit(Location node) => VisitChildren(node);
        protected virtual void Visit(Annotations node) => VisitChildren(node);
    }
}
