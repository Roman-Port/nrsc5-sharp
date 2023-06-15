using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public interface ILotProgressInfo
    {
        ushort Port { get; }
        ushort Lot { get; }
        uint Seq { get; }
        IAasFile File { get; }
    }
}
