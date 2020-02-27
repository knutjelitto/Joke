using Joke.Front.Pony.ParseTree;
using Joke.Outside;
using System.Diagnostics;
using System.Linq;

namespace Joke.Front.Pony.Visit
{
    public class TokenCoverageVisitor : Visitor
    {
        public readonly Integers Set = new Integers();

        protected override void VisitChildren(PtNode node)
        {
            var children = reflect.Or(node).ToList();

            if (children.Count == 0)
            {
                for (var i = node.Span.Start; i < node.Span.Next; ++i)
                {
                    Set.Add(i);
                }
                if (node is PtArguments ||
                    node is PtParameters ||
                    node is PtLambdaParameters ||
                    node is PtLambdaTypeParameters ||
                    node is PtFields ||
                    node is PtMethods ||
                    node is PtUnit)
                {
                }
                else
                {
                    var text = node.Span.ToString();
                    Debug.Assert(!string.IsNullOrWhiteSpace(text));
                    Debug.Assert(text != "(-)");
                    //System.Console.WriteLine($"{text}");
                }
            }

            base.VisitChildren(node);
        }
    }
}
