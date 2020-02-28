using Joke.Front.Pony.ParseTree;
using Joke.Outside;
using System.Collections.Generic;

namespace Joke.Compiler.Tree
{
    public class Unit
    {
        public readonly List<IUse> Uses = new List<IUse>();

        public Unit(PtUnit source, FileRef unitFile)
        {
            Source = source;
            UnitFile = unitFile;
        }

        public PtUnit Source { get; }
        public FileRef UnitFile { get; }
    }
}
