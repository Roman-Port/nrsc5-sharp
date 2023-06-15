using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public interface IAasFile
    {
        uint Timestamp { get; }
        string Name { get; }
        uint Mime { get; }
        ushort Lot { get; }
        uint Size { get; }
        byte[][] Fragments { get; }
    }
}
