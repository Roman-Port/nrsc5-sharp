using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities.SIG
{
    public interface ISigAudioComponent : ISigComponent
    {
        ushort Port { get; } // Distinguishes packets for this service
        byte Type { get; } // 0 for stream,  1 for packet, 3 for LOT
        Nrsc5MimeType Mime { get; } //Content
    }
}
