using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public interface ILotProgressInfo
    {
        /// <summary>
        /// The port this fragment arrived in.
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// The LOT ID of this fragment.
        /// </summary>
        uint Lot { get; }

        /// <summary>
        /// Sequence number of this fragment, starting at 0.
        /// </summary>
        uint Seq { get; }

        /// <summary>
        /// Payload of this fragment.
        /// </summary>
        byte[] FragmentData { get; }

        /// <summary>
        /// Size of FragmentData.
        /// </summary>
        uint FragmentSize { get; }

        /// <summary>
        /// Pointer to the name of the LOT. May not have been received yet, in which case this value will be NULL.
        /// </summary>
        string LotName { get; }

        /// <summary>
        /// MIME type hash for this LOT. May not have been received yet, in which case this value will be 0.
        /// </summary>
        uint LotMime { get; }

        /// <summary>
        /// Size of this LOT in bytes. May not have been received yet, in which case this value will be 0.
        /// </summary>
        uint LotSize { get; }
    }
}
