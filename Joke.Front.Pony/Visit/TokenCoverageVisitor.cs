using Joke.Front.Pony.Ast;
using Joke.Outside;
using System.Diagnostics;
using System.Linq;

namespace Joke.Front.Pony.Visit
{
    public class TokenCoverageVisitor : Visitor
    {
        public readonly Integers Set = new Integers();

        protected override void VisitChildren(Node node)
        {
            var children = reflect.Or(node).ToList();

            if (children.Count == 0)
            {
                for (var i = node.Span.Start; i < node.Span.Next; ++i)
                {
                    Set.Add(i);
                }
                if (node is Arguments ||
                    node is Parameters ||
                    node is LambdaParameters ||
                    node is LambdaTypeParameters ||
                    node is Fields ||
                    node is Methods ||
                    node is File)
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
