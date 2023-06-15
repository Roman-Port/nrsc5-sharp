using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public interface IId3EventInfo
    {
        uint Program { get; }
        string Title { get; }
        string Artist { get; }
        string Album { get; }
        string Genre { get; }

        string UfidOwner { get; }
        string UfidId { get; }

        uint XhdrMime { get; }
        int XhdrParam { get; }
        int XhdrLot { get; }

    }
}
