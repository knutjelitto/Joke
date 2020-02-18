using System.Collections.Generic;

using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Visit;

namespace Joke
{
    public class FfiVisitor : Visitor
    {
        public HashSet<string> Ffis = new HashSet<string>();

        protected override void DoVisit(UseUri node)
        {
            Ffis.Add($"use {node.Uri}");
        }

        protected override void DoVisit(FfiCall node)
        {
            Ffis.Add($"call {node.Name}");
        }

        protected override void DoVisit(UseFfi node)
        {
            Ffis.Add($"use {node.FfiName}");
        }
    }
}
