using System.Collections.Generic;

using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Visit;

namespace Joke
{
    public class FfiVisitor : Visitor
    {
        public HashSet<string> Ffis = new HashSet<string>();

        protected override void DoVisit(PtUseUri node)
        {
            Ffis.Add($"use {node.Uri}");
        }

        protected override void DoVisit(PtFfiCall node)
        {
            Ffis.Add($"call {node.Name}");
        }

        protected override void DoVisit(PtUseFfi node)
        {
            Ffis.Add($"use {node.FfiName}");
        }
    }
}
