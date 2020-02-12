using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    [Flags]
    public enum CapKind
    {
        Missing = 0,

        Iso = 1,
        Trn = 2,
        Ref = 4,
        Val = 8,
        Box = 16,
        Tag = 32,

        HashAny = Iso | Trn | Ref | Val | Box | Tag,
        HashRead = Ref | Val | Box,
        HashSend = Iso | Val | Tag,
        HashShare = Val | Tag,
        HashAlias = Ref | Val | Box | Tag,

    }
}
