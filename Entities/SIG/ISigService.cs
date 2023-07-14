using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities.SIG
{
    public interface ISigService
    {
        SigServiceType Type { get; }
        ushort Number { get; } // Channel number: 1,2,3,4
        string Name { get; } // Channel name, e.g. "MPS" or "SPS1"
        IReadOnlyList<ISigComponent> Components { get; }
    }
}
