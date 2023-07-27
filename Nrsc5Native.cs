using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Nrsc5Sharp
{
    static unsafe class Nrsc5Native
    {
        private const string NRSC5_DLL = "libnrsc5.dll";
        private const int PTR_SIZE = 8;

        [StructLayout(LayoutKind.Explicit)]
        public struct nrsc5_sig_component_t
        {
            [FieldOffset(0)]
            public IntPtr next;

            [FieldOffset(PTR_SIZE)]
            public byte type;

            [FieldOffset(PTR_SIZE + 1)]
            public byte id;

            [FieldOffset(PTR_SIZE + 4)]
            public data_t data;

            [FieldOffset(PTR_SIZE + 4)]
            public audio_t audio;

            [StructLayout(LayoutKind.Sequential)]
            public struct data_t
            {
                public ushort port;
                public ushort service_data_type;
                public byte type;
                public uint mime;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct audio_t
            {
                public byte port;
                public byte type;
                public uint mime;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct nrsc5_sig_service_t
        {
            public nrsc5_sig_service_t* next;
            public byte type;
            public ushort number;
            public IntPtr name; //Ptr to string
            public nrsc5_sig_component_t* components;
        }

        public enum NRSC5_EVENT
        {
            NRSC5_EVENT_LOST_DEVICE,
            NRSC5_EVENT_IQ,
            NRSC5_EVENT_SYNC,
            NRSC5_EVENT_LOST_SYNC,
            NRSC5_EVENT_MER,
            NRSC5_EVENT_BER,
            NRSC5_EVENT_HDC,
            NRSC5_EVENT_AUDIO,
            NRSC5_EVENT_ID3,
            NRSC5_EVENT_SIG,
            NRSC5_EVENT_LOT,
            NRSC5_EVENT_SIS,
            NRSC5_EVENT_LOT_PROGRESS
        }

        public enum NRSC5_ACCESS
        {
            NRSC5_ACCESS_PUBLIC,
            NRSC5_ACCESS_RESTRICTED
        }

        public enum NRSC5_PROGRAM_TYPE
        {
            NRSC5_PROGRAM_TYPE_UNDEFINED = 0,
            NRSC5_PROGRAM_TYPE_NEWS = 1,
            NRSC5_PROGRAM_TYPE_INFORMATION = 2,
            NRSC5_PROGRAM_TYPE_SPORTS = 3,
            NRSC5_PROGRAM_TYPE_TALK = 4,
            NRSC5_PROGRAM_TYPE_ROCK = 5,
            NRSC5_PROGRAM_TYPE_CLASSIC_ROCK = 6,
            NRSC5_PROGRAM_TYPE_ADULT_HITS = 7,
            NRSC5_PROGRAM_TYPE_SOFT_ROCK = 8,
            NRSC5_PROGRAM_TYPE_TOP_40 = 9,
            NRSC5_PROGRAM_TYPE_COUNTRY = 10,
            NRSC5_PROGRAM_TYPE_OLDIES = 11,
            NRSC5_PROGRAM_TYPE_SOFT = 12,
            NRSC5_PROGRAM_TYPE_NOSTALGIA = 13,
            NRSC5_PROGRAM_TYPE_JAZZ = 14,
            NRSC5_PROGRAM_TYPE_CLASSICAL = 15,
            NRSC5_PROGRAM_TYPE_RHYTHM_AND_BLUES = 16,
            NRSC5_PROGRAM_TYPE_SOFT_RHYTHM_AND_BLUES = 17,
            NRSC5_PROGRAM_TYPE_FOREIGN_LANGUAGE = 18,
            NRSC5_PROGRAM_TYPE_RELIGIOUS_MUSIC = 19,
            NRSC5_PROGRAM_TYPE_RELIGIOUS_TALK = 20,
            NRSC5_PROGRAM_TYPE_PERSONALITY = 21,
            NRSC5_PROGRAM_TYPE_PUBLIC = 22,
            NRSC5_PROGRAM_TYPE_COLLEGE = 23,
            NRSC5_PROGRAM_TYPE_SPANISH_TALK = 24,
            NRSC5_PROGRAM_TYPE_SPANISH_MUSIC = 25,
            NRSC5_PROGRAM_TYPE_HIP_HOP = 26,
            NRSC5_PROGRAM_TYPE_WEATHER = 29,
            NRSC5_PROGRAM_TYPE_EMERGENCY_TEST = 30,
            NRSC5_PROGRAM_TYPE_EMERGENCY = 31,
            NRSC5_PROGRAM_TYPE_TRAFFIC = 65,
            NRSC5_PROGRAM_TYPE_SPECIAL_READING_SERVICES = 76
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct nrsc5_sis_asd_t
        {
            public IntPtr next; //Ptr to nrsc5_sis_asd_t
            public int program;
            public int access;
            public int type;
            public int sound_exp;
        }

        public enum NRSC5_SERVICE_DATA_TYPE
        {
            NRSC5_SERVICE_DATA_TYPE_NON_SPECIFIC = 0,
            NRSC5_SERVICE_DATA_TYPE_NEWS = 1,
            NRSC5_SERVICE_DATA_TYPE_SPORTS = 3,
            NRSC5_SERVICE_DATA_TYPE_WEATHER = 29,
            NRSC5_SERVICE_DATA_TYPE_EMERGENCY = 31,
            NRSC5_SERVICE_DATA_TYPE_TRAFFIC = 65,
            NRSC5_SERVICE_DATA_TYPE_IMAGE_MAPS = 66,
            NRSC5_SERVICE_DATA_TYPE_TEXT = 80,
            NRSC5_SERVICE_DATA_TYPE_ADVERTISING = 256,
            NRSC5_SERVICE_DATA_TYPE_FINANCIAL = 257,
            NRSC5_SERVICE_DATA_TYPE_STOCK_TICKER = 258,
            NRSC5_SERVICE_DATA_TYPE_NAVIGATION = 259,
            NRSC5_SERVICE_DATA_TYPE_ELECTRONIC_PROGRAM_GUIDE = 260,
            NRSC5_SERVICE_DATA_TYPE_AUDIO = 261,
            NRSC5_SERVICE_DATA_TYPE_PRIVATE_DATA_NETWORK = 262,
            NRSC5_SERVICE_DATA_TYPE_SERVICE_MAINTENANCE = 263,
            NRSC5_SERVICE_DATA_TYPE_HD_RADIO_SYSTEM_SERVICES = 264,
            NRSC5_SERVICE_DATA_TYPE_AUDIO_RELATED_DATA = 265
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct nrsc5_sis_dsd_t
        {
            public IntPtr next; //Ptr to nrsc5_sis_dsd_t
            public uint access;
            public uint type;
            public uint mime_type;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct nrsc5_event_t
        {
            const int OFFSET = 8;

            [FieldOffset(0)]
            public uint event_type;

            [FieldOffset(OFFSET)]
            public iq_t iq;

            [FieldOffset(OFFSET)]
            public ber_t ber;

            [FieldOffset(OFFSET)]
            public mer_t mer;

            [FieldOffset(OFFSET)]
            public hdc_t hdc;

            [FieldOffset(OFFSET)]
            public audio_t audio;

            [FieldOffset(OFFSET)]
            public id3_t id3;

            [FieldOffset(OFFSET)]
            public lot_t lot;

            [FieldOffset(OFFSET)]
            public sig_t sig;

            [FieldOffset(OFFSET)]
            public sis_t sis;

            [FieldOffset(OFFSET)]
            public lot_progress_t lot_progress;

            [StructLayout(LayoutKind.Sequential)]
            public struct iq_t
            {
                public IntPtr data;
                public IntPtr count; //integer
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ber_t
            {
                public float cber;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct mer_t
            {
                public float lower;
                public float upper;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct hdc_t
            {
                public uint program;
                public IntPtr data; // Bytes
                public IntPtr count; // Integer
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct audio_t
            {
                public uint program;
                public IntPtr data; // Int16s
                public IntPtr count; //Integer
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct id3_t
            {
                public uint program;
                public IntPtr title; //Ptr to string
                public IntPtr artist; //Ptr to string
                public IntPtr album; //Ptr to string
                public IntPtr genre; //Ptr to string
                public ufid_t ufid;
                public xhdr_t xhdr;

                public struct ufid_t
                {
                    public IntPtr owner; //Ptr to string
                    public IntPtr id; //Ptr to string
                }

                public struct xhdr_t
                {
                    public uint mime;
                    public int param;
                    public int lot;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct lot_t
            {
                public ushort port;
                public uint lot;
                public uint size;
                public uint mime;
                public IntPtr name; //Ptr to string
                public IntPtr data; //Ptr to bytes
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct sig_t
            {
                public nrsc5_sig_service_t* services;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct sis_t
            {
                public IntPtr country_code; //Ptr to string
                public int fcc_facility_id;
                public IntPtr name; //Ptr to string
                public IntPtr slogan; //Ptr to string
                public IntPtr message; //Ptr to string
                public IntPtr alert; //Ptr to string
                public float latitude;
                public float longitude;
                public int altitude;
                public IntPtr audio_services; //Ptr to nrsc5_sis_asd_t
                public IntPtr data_services; //Ptr to nrsc5_sis_dsd_t
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct lot_progress_t
            {
                public ushort port; // The port this fragment arrived in.
                public uint lot; // The LOT ID of this fragment.
                public uint seq; // Sequence number of this fragment, starting at 0.

                public byte* fragment_data; // Payload of this fragment.
                public uint fragment_size; // Size of this fragment's data and the "fragment_data" property.

                public char* lot_name; // Pointer to the name of the LOT. May not have been received yet, in which case this value will be NULL.
                public uint lot_mime; // MIME type hash for this LOT. May not have been received yet, in which case this value will be 0.
                public uint lot_size; // Size of this LOT in bytes. May not have been received yet, in which case this value will be 0.
            }
        }

        public delegate void nrsc5_callback_t(IntPtr evt /* nrsc5_event_t */, IntPtr opaque);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_get_version(IntPtr* version /* Ptr to ptr to string */);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_service_data_type_name(uint type, IntPtr* name /* Ptr to ptr to string */);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_program_type_name(uint type, IntPtr* name /* Ptr to ptr to string */);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_open(IntPtr* ctx/* Ptr to ptr to nrsc5_t */, int device_index);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_open_file(IntPtr* ctx/* Ptr to ptr to nrsc5_t */, IntPtr file /* Ptr to file handle */);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_open_pipe(IntPtr* ctx/* Ptr to ptr to nrsc5_t */);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_open_rtltcp(IntPtr* ctx/* Ptr to ptr to nrsc5_t */, int socket);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_close(IntPtr ctx/* Ptr to nrsc5_t */);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_start(IntPtr ctx/* Ptr to nrsc5_t */);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_stop(IntPtr ctx/* Ptr to nrsc5_t */);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_mode(IntPtr ctx/* Ptr to nrsc5_t */, int mode);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_bias_tee(IntPtr ctx/* Ptr to nrsc5_t */, int on);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_direct_sampling(IntPtr ctx/* Ptr to nrsc5_t */, int on);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_freq_correction(IntPtr ctx/* Ptr to nrsc5_t */, int ppm);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_get_frequency(IntPtr ctx/* Ptr to nrsc5_t */, float* freq);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_frequency(IntPtr ctx/* Ptr to nrsc5_t */, float freq);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_get_gain(IntPtr ctx/* Ptr to nrsc5_t */, float* gain);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_set_gain(IntPtr ctx/* Ptr to nrsc5_t */, float gain);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_set_auto_gain(IntPtr ctx/* Ptr to nrsc5_t */, int on);

        [DllImport(NRSC5_DLL)]
        public static extern void nrsc5_set_callback(IntPtr ctx/* Ptr to nrsc5_t */, nrsc5_callback_t cb, IntPtr opaque);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_pipe_samples_cu8(IntPtr ctx/* Ptr to nrsc5_t */, byte* samples, uint length);

        [DllImport(NRSC5_DLL)]
        public static extern int nrsc5_pipe_samples_cs16(IntPtr ctx/* Ptr to nrsc5_t */, short* samples, uint length);
    }
}
