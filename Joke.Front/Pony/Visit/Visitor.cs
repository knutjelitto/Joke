using Joke.Front.Pony.ParseTree;
using System.Collections.Generic;

namespace Joke.Front.Pony.Visit
{
    public class Visitor
    {
        protected readonly Reflect reflect = new Reflect();

        public Visitor()
        {
        }

        public void Visit(PtNode root)
        {
            Dispatch(root);
        }

        private void DoVisit(PtNode node)
        {
            throw new System.NotImplementedException($"node type `{node.GetType().Name}´ not implemented");
        }

        private void Dispatch(IEnumerable<PtNode> nodes)
        {
            foreach (var node in nodes)
            {
                Dispatch(node);
            }
        }

        private void Dispatch(PtNode node)
        {
            DoVisit(node as dynamic);
        }

        protected virtual void VisitChildren(PtNode node)
        {
            foreach (var child in reflect.Or(node))
            {
                Dispatch(child);
            }
        }

        protected virtual void DoVisit(PtUnit node) => VisitChildren(node);
        protected virtual void DoVisit(PtUseFfi node) => VisitChildren(node);
        protected virtual void DoVisit(PtFfiName node) => VisitChildren(node);
        protected virtual void DoVisit(PtTypeArguments node) => VisitChildren(node);
        protected virtual void DoVisit(PtNominalType node) => VisitChildren(node);
        protected virtual void DoVisit(PtEphemAlias node) => VisitChildren(node);
        protected virtual void DoVisit(PtParameters node) => VisitChildren(node);
        protected virtual void DoVisit(PtRegularParameter node) => VisitChildren(node);
        protected virtual void DoVisit(PtCap node) => VisitChildren(node);
        protected virtual void DoVisit(PtClass node) => VisitChildren(node);
        protected virtual void DoVisit(PtString node) => VisitChildren(node);
        protected virtual void DoVisit(PtMembers node) => VisitChildren(node);
        protected virtual void DoVisit(PtFields node) => VisitChildren(node);
        protected virtual void DoVisit(PtMethods node) => VisitChildren(node);
        protected virtual void DoVisit(PtMethod node) => VisitChildren(node);
        protected virtual void DoVisit(PtRef node) => VisitChildren(node);
        protected virtual void DoVisit(PtTypeParameters node) => VisitChildren(node);
        protected virtual void DoVisit(PtTypeParameter node) => VisitChildren(node);
        protected virtual void DoVisit(PtGroupedType node) => VisitChildren(node);
        protected virtual void DoVisit(PtInfixType node) => VisitChildren(node);
        protected virtual void DoVisit(PtPostfix node) => VisitChildren(node);
        protected virtual void DoVisit(PtDot node) => VisitChildren(node);
        protected virtual void DoVisit(PtCall node) => VisitChildren(node);
        protected virtual void DoVisit(PtArguments node) => VisitChildren(node);
        protected virtual void DoVisit(PtInt node) => VisitChildren(node);
        protected virtual void DoVisit(PtSequence node) => VisitChildren(node);
        protected virtual void DoVisit(PtAssignment node) => VisitChildren(node);
        protected virtual void DoVisit(PtLocal node) => VisitChildren(node);
        protected virtual void DoVisit(PtGroupedExpression node) => VisitChildren(node);
        protected virtual void DoVisit(PtBinary node) => VisitChildren(node);
        protected virtual void DoVisit(PtThisLiteral node) => VisitChildren(node);
        protected virtual void DoVisit(PtIff node) => VisitChildren(node);
        protected virtual void DoVisit(PtThen node) => VisitChildren(node);
        protected virtual void DoVisit(PtElse node) => VisitChildren(node);
        protected virtual void DoVisit(PtFfiCall node) => VisitChildren(node);
        protected virtual void DoVisit(PtPositionalArgument node) => VisitChildren(node);
        protected virtual void DoVisit(PtBool node) => VisitChildren(node);
        protected virtual void DoVisit(PtQualify node) => VisitChildren(node);
        protected virtual void DoVisit(PtJump node) => VisitChildren(node);
        protected virtual void DoVisit(PtWhile node) => VisitChildren(node);
        protected virtual void DoVisit(PtMatch node) => VisitChildren(node);
        protected virtual void DoVisit(PtCases node) => VisitChildren(node);
        protected virtual void DoVisit(PtCase node) => VisitChildren(node);
        protected virtual void DoVisit(PtUnary node) => VisitChildren(node);
        protected virtual void DoVisit(PtRecover node) => VisitChildren(node);
        protected virtual void DoVisit(PtTry node) => VisitChildren(node);
        protected virtual void DoVisit(PtChar node) => VisitChildren(node);
        protected virtual void DoVisit(PtChain node) => VisitChildren(node);
        protected virtual void DoVisit(PtUseUri node) => VisitChildren(node);
        protected virtual void DoVisit(PtField node) => VisitChildren(node);
        protected virtual void DoVisit(PtFor node) => VisitChildren(node);
        protected virtual void DoVisit(PtIdsSingle node) => VisitChildren(node);
        protected virtual void DoVisit(PtAs node) => VisitChildren(node);
        protected virtual void DoVisit(PtViewpointType node) => VisitChildren(node);
        protected virtual void DoVisit(PtConsume node) => VisitChildren(node);
        protected virtual void DoVisit(PtSubType node) => VisitChildren(node);
        protected virtual void DoVisit(PtSemiExpression node) => VisitChildren(node);
        protected virtual void DoVisit(PtObject node) => VisitChildren(node);
        protected virtual void DoVisit(PtNamedArgument node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambda node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaParameters node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaParameter node) => VisitChildren(node);
        protected virtual void DoVisit(PtTilde node) => VisitChildren(node);
        protected virtual void DoVisit(PtIdsMulti node) => VisitChildren(node);
        protected virtual void DoVisit(PtArray node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaCaptures node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaCaptureName node) => VisitChildren(node);
        protected virtual void DoVisit(PtEllipsisParameter node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaType node) => VisitChildren(node);
        protected virtual void DoVisit(PtLambdaTypeParameters node) => VisitChildren(node);
        protected virtual void DoVisit(PtGuard node) => VisitChildren(node);
        protected virtual void DoVisit(PtArrayType node) => VisitChildren(node);
        protected virtual void DoVisit(PtFloat node) => VisitChildren(node);
        protected virtual void DoVisit(PtThisType node) => VisitChildren(node);
        protected virtual void DoVisit(PtWith node) => VisitChildren(node);
        protected virtual void DoVisit(PtWithElements node) => VisitChildren(node);
        protected virtual void DoVisit(PtWithElement node) => VisitChildren(node);
        protected virtual void DoVisit(PtRepeat node) => VisitChildren(node);
        protected virtual void DoVisit(PtLocation node) => VisitChildren(node);
        protected virtual void DoVisit(PtAnnotations node) => VisitChildren(node);
        protected virtual void DoVisit(PtIdentifier node) => VisitChildren(node);
    }
}
