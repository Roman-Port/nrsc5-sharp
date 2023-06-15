using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public interface ILotCompletedInfo
    {
        ushort Port { get; }
        uint Lot { get; }
        uint Size { get; }
        uint Mime { get; }
        string Name { get; }
        byte[] Data { get; }
    }
}
