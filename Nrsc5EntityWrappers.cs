using Nrsc5Sharp.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Nrsc5Sharp
{
    internal unsafe class Nrsc5EntityWrappers
    {
        public static string StringOrNull(IntPtr str)
        {
            if (str == IntPtr.Zero)
                return null;
            else
                return Marshal.PtrToStringAnsi(str);
        }

        public class DisposableWrapper : IDisposable
        {
            public DisposableWrapper()
            {
                isValid = true;
            }

            private bool isValid;

            protected void EnsurePointersValid()
            {
                if (!isValid)
                    throw new ObjectDisposedException(GetType().FullName);
            }

            public void Dispose()
            {
                isValid = false;
            }
        }

        public class Id3EventInfoImpl : IId3EventInfo
        {
            public Id3EventInfoImpl(Nrsc5Native.nrsc5_event_t.id3_t* info)
            {
                program = info->program;
                title = StringOrNull(info->title);
                artist = StringOrNull(info->artist);
                album = StringOrNull(info->album);
                genre = StringOrNull(info->genre);
                ufidOwner = StringOrNull(info->ufid.owner);
                ufidId = StringOrNull(info->ufid.id);
                xhdrMime = info->xhdr.mime;
                xhdrParam = info->xhdr.param;
                xhdrLot = info->xhdr.lot;
            }

            private readonly uint program;
            private readonly string title;
            private readonly string artist;
            private readonly string album;
            private readonly string genre;
            private readonly string ufidOwner;
            private readonly string ufidId;
            private readonly uint xhdrMime;
            private readonly int xhdrParam;
            private readonly int xhdrLot;

            public uint Program => program;

            public string Title => title;

            public string Artist => artist;

            public string Album => album;

            public string Genre => genre;

            public string UfidOwner => ufidOwner;

            public string UfidId => ufidId;

            public uint XhdrMime => xhdrMime;

            public int XhdrParam => xhdrParam;

            public int XhdrLot => xhdrLot;

            public override bool Equals(object obj)
            {
                return obj is IId3EventInfo c &&
                    Program == c.Program &&
                    Title == c.Title &&
                    Artist == c.Artist &&
                    Album == c.Album &&
                    Genre == c.Genre &&
                    UfidOwner == c.UfidOwner &&
                    UfidId == c.UfidId &&
                    XhdrMime == c.XhdrMime &&
                    XhdrParam == c.XhdrParam &&
                    XhdrLot == c.XhdrLot;
            }
        }

        public class LotCompletedInfoImpl : ILotCompletedInfo, IDisposable
        {
            public LotCompletedInfoImpl(Nrsc5Native.nrsc5_event_t.lot_t* info)
            {
                port = info->port;
                lot = info->lot;
                size = info->size;
                mime = info->mime;
                name = Marshal.PtrToStringAnsi(info->name);
                data = info->data;
            }

            private readonly ushort port;
            private readonly uint lot;
            private readonly uint size;
            private readonly uint mime;
            private readonly string name;
            private IntPtr data;

            public ushort Port => port;

            public uint Lot => lot;

            public uint Size => size;

            public uint Mime => mime;

            public string Name => name;

            public byte[] Data
            {
                get
                {
                    //Check if disposed
                    if (data == IntPtr.Zero)
                        throw new ObjectDisposedException(GetType().Name);

                    //Allocate array and copy data into managed buffer
                    byte[] payload = new byte[size];
                    Marshal.Copy(data, payload, 0, (int)size);
                    return payload;
                }
            }

            public void Dispose()
            {
                //Mark as disposed
                data = IntPtr.Zero;
            }
        }

        public class LotProgressInfoImpl : DisposableWrapper, ILotProgressInfo
        {
            public LotProgressInfoImpl(Nrsc5Native.nrsc5_event_t.lot_progress_t* info)
            {
                this.info = *info;
            }

            private readonly Nrsc5Native.nrsc5_event_t.lot_progress_t info;

            /// <summary>
            /// The port this fragment arrived in.
            /// </summary>
            public ushort Port => info.port;

            /// <summary>
            /// The LOT ID of this fragment.
            /// </summary>
            public uint Lot => info.lot;

            /// <summary>
            /// Sequence number of this fragment, starting at 0.
            /// </summary>
            public uint Seq => info.seq;

            /// <summary>
            /// Payload of this fragment.
            /// </summary>
            public byte[] FragmentData
            {
                get
                {
                    EnsurePointersValid();
                    byte[] data = new byte[info.fragment_size];
                    Marshal.Copy((IntPtr)info.fragment_data, data, 0, data.Length);
                    return data;
                }
            }

            /// <summary>
            /// Size of FragmentData.
            /// </summary>
            public uint FragmentSize => info.fragment_size;

            /// <summary>
            /// Pointer to the name of the LOT. May not have been received yet, in which case this value will be NULL.
            /// </summary>
            public string LotName
            {
                get
                {
                    EnsurePointersValid();
                    return StringOrNull((IntPtr)info.lot_name);
                }
            }

            /// <summary>
            /// MIME type hash for this LOT. May not have been received yet, in which case this value will be 0.
            /// </summary>
            public uint LotMime => info.lot_mime;

            /// <summary>
            /// Size of this LOT in bytes. May not have been received yet, in which case this value will be 0.
            /// </summary>
            public uint LotSize => info.lot_size;
        }
    }
}
