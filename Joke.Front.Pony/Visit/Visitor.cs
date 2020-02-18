using Joke.Front.Pony.Ast;
using System.Collections.Generic;

namespace Joke.Front.Pony.Visit
{
    public class Visitor
    {
        protected readonly Reflect reflect = new Reflect();

        public Visitor()
        {
        }

        public void Visit(Node root)
        {
            Dispatch(root);
        }

        private void DoVisit(Node node)
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
            DoVisit(node as dynamic);
        }

        protected virtual void VisitChildren(Node node)
        {
            foreach (var child in reflect.Or(node))
            {
                Dispatch(child);
            }
        }

        protected virtual void DoVisit(Module node) => VisitChildren(node);
        protected virtual void DoVisit(UseFfi node) => VisitChildren(node);
        protected virtual void DoVisit(FfiName node) => VisitChildren(node);
        protected virtual void DoVisit(Identifier node) => VisitChildren(node);
        protected virtual void DoVisit(TypeArguments node) => VisitChildren(node);
        protected virtual void DoVisit(NominalType node) => VisitChildren(node);
        protected virtual void DoVisit(EphemAlias node) => VisitChildren(node);
        protected virtual void DoVisit(Parameters node) => VisitChildren(node);
        protected virtual void DoVisit(RegularParameter node) => VisitChildren(node);
        protected virtual void DoVisit(Cap node) => VisitChildren(node);
        protected virtual void DoVisit(Class node) => VisitChildren(node);
        protected virtual void DoVisit(String node) => VisitChildren(node);
        protected virtual void DoVisit(Members node) => VisitChildren(node);
        protected virtual void DoVisit(Fields node) => VisitChildren(node);
        protected virtual void DoVisit(Methods node) => VisitChildren(node);
        protected virtual void DoVisit(Method node) => VisitChildren(node);
        protected virtual void DoVisit(Ref node) => VisitChildren(node);
        protected virtual void DoVisit(TypeParameters node) => VisitChildren(node);
        protected virtual void DoVisit(TypeParameter node) => VisitChildren(node);
        protected virtual void DoVisit(GroupedType node) => VisitChildren(node);
        protected virtual void DoVisit(InfixType node) => VisitChildren(node);
        protected virtual void DoVisit(Postfix node) => VisitChildren(node);
        protected virtual void DoVisit(Dot node) => VisitChildren(node);
        protected virtual void DoVisit(Call node) => VisitChildren(node);
        protected virtual void DoVisit(Arguments node) => VisitChildren(node);
        protected virtual void DoVisit(Int node) => VisitChildren(node);
        protected virtual void DoVisit(Sequence node) => VisitChildren(node);
        protected virtual void DoVisit(Assignment node) => VisitChildren(node);
        protected virtual void DoVisit(Local node) => VisitChildren(node);
        protected virtual void DoVisit(GroupedExpression node) => VisitChildren(node);
        protected virtual void DoVisit(Binary node) => VisitChildren(node);
        protected virtual void DoVisit(ThisLiteral node) => VisitChildren(node);
        protected virtual void DoVisit(Iff node) => VisitChildren(node);
        protected virtual void DoVisit(Then node) => VisitChildren(node);
        protected virtual void DoVisit(Else node) => VisitChildren(node);
        protected virtual void DoVisit(FfiCall node) => VisitChildren(node);
        protected virtual void DoVisit(PositionalArgument node) => VisitChildren(node);
        protected virtual void DoVisit(Bool node) => VisitChildren(node);
        protected virtual void DoVisit(Qualify node) => VisitChildren(node);
        protected virtual void DoVisit(Jump node) => VisitChildren(node);
        protected virtual void DoVisit(While node) => VisitChildren(node);
        protected virtual void DoVisit(Match node) => VisitChildren(node);
        protected virtual void DoVisit(Cases node) => VisitChildren(node);
        protected virtual void DoVisit(Case node) => VisitChildren(node);
        protected virtual void DoVisit(Unary node) => VisitChildren(node);
        protected virtual void DoVisit(Recover node) => VisitChildren(node);
        protected virtual void DoVisit(Try node) => VisitChildren(node);
        protected virtual void DoVisit(Char node) => VisitChildren(node);
        protected virtual void DoVisit(Chain node) => VisitChildren(node);
        protected virtual void DoVisit(UseUri node) => VisitChildren(node);
        protected virtual void DoVisit(Field node) => VisitChildren(node);
        protected virtual void DoVisit(DefaultArg node) => VisitChildren(node);
        protected virtual void DoVisit(For node) => VisitChildren(node);
        protected virtual void DoVisit(IdsSingle node) => VisitChildren(node);
        protected virtual void DoVisit(As node) => VisitChildren(node);
        protected virtual void DoVisit(ViewpointType node) => VisitChildren(node);
        protected virtual void DoVisit(Consume node) => VisitChildren(node);
        protected virtual void DoVisit(SubType node) => VisitChildren(node);
        protected virtual void DoVisit(SemiExpression node) => VisitChildren(node);
        protected virtual void DoVisit(Object node) => VisitChildren(node);
        protected virtual void DoVisit(NamedArgument node) => VisitChildren(node);
        protected virtual void DoVisit(Lambda node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaParameters node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaParameter node) => VisitChildren(node);
        protected virtual void DoVisit(Tilde node) => VisitChildren(node);
        protected virtual void DoVisit(IdsMulti node) => VisitChildren(node);
        protected virtual void DoVisit(Array node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaCaptures node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaCaptureName node) => VisitChildren(node);
        protected virtual void DoVisit(EllipsisParameter node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaType node) => VisitChildren(node);
        protected virtual void DoVisit(LambdaTypeParameters node) => VisitChildren(node);
        protected virtual void DoVisit(Guard node) => VisitChildren(node);
        protected virtual void DoVisit(ArrayType node) => VisitChildren(node);
        protected virtual void DoVisit(Float node) => VisitChildren(node);
        protected virtual void DoVisit(ThisType node) => VisitChildren(node);
        protected virtual void DoVisit(With node) => VisitChildren(node);
        protected virtual void DoVisit(WithElements node) => VisitChildren(node);
        protected virtual void DoVisit(WithElement node) => VisitChildren(node);
        protected virtual void DoVisit(Repeat node) => VisitChildren(node);
        protected virtual void DoVisit(Location node) => VisitChildren(node);
        protected virtual void DoVisit(Annotations node) => VisitChildren(node);
    }
}
