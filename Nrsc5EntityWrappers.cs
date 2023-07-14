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

        public class LotProgressInfoImpl : ILotProgressInfo, IDisposable
        {
            public LotProgressInfoImpl(Nrsc5Native.nrsc5_event_t.lot_progress_t* info)
            {
                port = info->port;
                lot = info->lot;
                seq = info->seq;
                file = (Nrsc5Native.nrsc5_aas_file_t*)info->file;
            }

            private readonly ushort port;
            private readonly ushort lot;
            private readonly uint seq;
            private Nrsc5Native.nrsc5_aas_file_t* file;

            public ushort Port => port;

            public ushort Lot => lot;

            public uint Seq => seq;

            public IAasFile File
            {
                get
                {
                    if (file == null)
                        throw new ObjectDisposedException(GetType().FullName);
                    return new AasFileImpl(file);
                }
            }

            public void Dispose()
            {
                //Mark pointers as invalid
                file = null;
            }
        }

        public class AasFileImpl : IAasFile
        {
            public AasFileImpl(Nrsc5Native.nrsc5_aas_file_t* info)
            {
                timestamp = info->timestamp;
                name = info->name == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(info->name);
                mime = info->mime;
                lot = info->lot;
                size = info->size;
                fragments = new byte[256][];
                for (int i = 0; i < fragments.Length; i++)
                {
                    if (info->fragments[i] != null)
                    {
                        fragments[i] = new byte[256];
                        Marshal.Copy((IntPtr)info->fragments[i], fragments[i], 0, fragments[i].Length);
                    }
                }
            }

            private readonly uint timestamp;
            private readonly string name;
            private readonly uint mime;
            private readonly ushort lot;
            private readonly uint size;
            private readonly byte[][] fragments;

            public uint Timestamp => timestamp;

            public string Name => name;

            public uint Mime => mime;

            public ushort Lot => lot;

            public uint Size => size;

            public byte[][] Fragments => fragments;
        }
    }
}
