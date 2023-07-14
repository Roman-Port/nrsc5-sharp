using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities.SIG
{
    public interface ISigComponent
    {
        byte Id { get; } // Component identifier, 0, 1, 2,..
    }
}
