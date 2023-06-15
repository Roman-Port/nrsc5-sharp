using Nrsc5Sharp.Entities;
using Nrsc5Sharp.Enums;
using Nrsc5Sharp.Exceptions;
using System;
using System.Runtime.InteropServices;
using static Nrsc5Sharp.Nrsc5Native;

namespace Nrsc5Sharp
{
    public unsafe class Nrsc5 : NativeObject
    {
        private Nrsc5(IntPtr handle) : base(handle)
        {
            //Create callback and pin in memory
            callback = new Nrsc5Native.nrsc5_callback_t(ProcessEventStatic);

            //Set callback in library
            Nrsc5Native.nrsc5_set_callback(Handle, callback, SelfReference);
        }

        public const int SAMPLE_RATE = 1488375;
        public const int LOT_FRAGMENT_SIZE = 256;

        private readonly Nrsc5Native.nrsc5_callback_t callback;

        public delegate void LostDeviceEventArgs(Nrsc5 radio);
        public delegate void BerEventArgs(Nrsc5 radio);
        public delegate void MerEventArgs(Nrsc5 radio);
        public delegate void IqEventArgs(Nrsc5 radio);
        public delegate void HdcEventArgs(Nrsc5 radio);
        public delegate void AudioEventArgs(Nrsc5 radio);
        public delegate void SyncEventArgs(Nrsc5 radio);
        public delegate void LostSyncEventArgs(Nrsc5 radio);
        public delegate void Id3EventArgs(Nrsc5 radio, IId3EventInfo info);
        public delegate void SigEventArgs(Nrsc5 radio);
        public delegate void LotEventArgs(Nrsc5 radio, ILotCompletedInfo info);
        public delegate void SisEventArgs(Nrsc5 radio);
        public delegate void LotProgressEventArgs(Nrsc5 radio, ILotProgressInfo info);

        public event LostDeviceEventArgs OnLostDevice;
        public event BerEventArgs OnBer;
        public event MerEventArgs OnMer;
        public event IqEventArgs OnIq;
        public event HdcEventArgs OnHdc;
        public event AudioEventArgs OnAudio;
        public event SyncEventArgs OnSync;
        public event LostSyncEventArgs OnLostSync;
        public event Id3EventArgs OnId3;
        public event SigEventArgs OnSig;
        public event LotEventArgs OnLot;
        public event SisEventArgs OnSis;
        public event LotProgressEventArgs OnLotProgress;

        /// <summary>
        /// Opens the NRSC-5 library on an RTL-SDR.
        /// </summary>
        /// <param name="deviceIndex">RTL-SDR device index.</param>
        /// <returns></returns>
        public static Nrsc5 OpenRtlSdr(int deviceIndex)
        {
            //Initialize native library
            IntPtr handle = IntPtr.Zero;
            EnsureSuccess(Nrsc5Native.nrsc5_open(&handle, deviceIndex));

            return new Nrsc5(handle);
        }

        /// <summary>
        /// Opens the NRSC-5 library to pipe samples in directly.
        /// </summary>
        /// <returns></returns>
        public static Nrsc5 OpenPipe()
        {
            //Initialize native library
            IntPtr handle = IntPtr.Zero;
            EnsureSuccess(Nrsc5Native.nrsc5_open_pipe(&handle));

            return new Nrsc5(handle);
        }

        /// <summary>
        /// Native library version.
        /// </summary>
        public static string Version
        {
            get
            {
                IntPtr ver = IntPtr.Zero;
                Nrsc5Native.nrsc5_get_version(&ver);
                return Marshal.PtrToStringAnsi(ver);
            }
        }

        /// <summary>
        /// Tuner mode.
        /// </summary>
        public RadioMode Mode
        {
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_mode(Handle, (int)value));
        }

        /// <summary>
        /// Tuner Bias-T.
        /// </summary>
        public bool BiasTee
        {
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_bias_tee(Handle, value ? 1 : 0));
        }

        /// <summary>
        /// Tuner direct sampling.
        /// </summary>
        public bool DirectSampling
        {
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_direct_sampling(Handle, value ? 1 : 0));
        }

        /// <summary>
        /// Tuner frequency correction in ppm.
        /// </summary>
        public int FrequencyCorrection
        {
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_freq_correction(Handle, value));
        }

        /// <summary>
        /// Tuner frequency.
        /// </summary>
        public float Frequency
        {
            get
            {
                float freq = 0;
                Nrsc5Native.nrsc5_get_frequency(Handle, &freq);
                return freq;
            }
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_frequency(Handle, value));
        }

        /// <summary>
        /// Tuner gain.
        /// </summary>
        public float Gain
        {
            get
            {
                float gain = 0;
                Nrsc5Native.nrsc5_get_gain(Handle, &gain);
                return gain;
            }
            set => EnsureSuccess(Nrsc5Native.nrsc5_set_gain(Handle, value));
        }

        /// <summary>
        /// Tuner auto gain control.
        /// </summary>
        public bool AutoGain
        {
            set => Nrsc5Native.nrsc5_set_auto_gain(Handle, value ? 1 : 0);
        }

        /// <summary>
        /// Pipes Byte IQ samples in when in pipe mode.
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="length"></param>
        public void PipeSamples(byte* samples, uint length)
        {
            EnsureSuccess(Nrsc5Native.nrsc5_pipe_samples_cu8(Handle, samples, length));
        }

        /// <summary>
        /// Pipes Int16 IQ samples in when in pipe mode.
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="length"></param>
        public void PipeSamples(short* samples, uint length)
        {
            EnsureSuccess(Nrsc5Native.nrsc5_pipe_samples_cs16(Handle, samples, length));
        }

        /// <summary>
        /// Starts the demodulator.
        /// </summary>
        public void Start()
        {
            Nrsc5Native.nrsc5_start(Handle);
        }

        /// <summary>
        /// Stops the demodulator.
        /// </summary>
        public void Stop()
        {
            Nrsc5Native.nrsc5_stop(Handle);
        }

        public IAasFile GetLot(ushort port, ushort lot)
        {
            //Attempt to get native pointer
            nrsc5_aas_file_t* info = Nrsc5Native.nrsc5_get_lot(Handle, port, lot);
            if (info == null)
                return null;

            //Wrap
            return new Nrsc5EntityWrappers.AasFileImpl(info);
        }

        /// <summary>
        /// Gets the name of a specified service data type.
        /// </summary>
        /// <param name="type">The type ID.</param>
        /// <returns></returns>
        public static string GetServiceDataTypeName(uint type)
        {
            IntPtr name = IntPtr.Zero;
            Nrsc5Native.nrsc5_service_data_type_name(type, &name);
            return Marshal.PtrToStringAnsi(name);
        }

        /// <summary>
        /// Gets the name of a specified program type.
        /// </summary>
        /// <param name="type">The type ID.</param>
        /// <returns></returns>
        public static string GetProgramTypeName(uint type)
        {
            IntPtr name = IntPtr.Zero;
            Nrsc5Native.nrsc5_program_type_name(type, &name);
            return Marshal.PtrToStringAnsi(name);
        }

        /// <summary>
        /// Checks to make sure the library's response code is zero. Otherwise, throw an exception.
        /// </summary>
        /// <param name="code"></param>
        private static void EnsureSuccess(int code)
        {
            if (code != 0)
                throw new Nrsc5NativeException(code);
        }

        private static void ProcessEventStatic(IntPtr evt, IntPtr opaque)
        {
            //Resolve the opaque pointer to the instance of the managed wrapper
            Nrsc5 ctx = (Nrsc5)GCHandle.FromIntPtr(opaque).Target;

            //Handoff
            ctx.ProcessEvent((Nrsc5Native.nrsc5_event_t*)evt);
        }

        private void ProcessEvent(Nrsc5Native.nrsc5_event_t* evt)
        {
            switch ((Nrsc5Native.NRSC5_EVENT)evt->event_type)
            {
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_LOST_DEVICE:
                    OnLostDevice?.Invoke(this);
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_IQ:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_SYNC:
                    OnSync?.Invoke(this);
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_LOST_SYNC:
                    OnLostSync?.Invoke(this);
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_MER:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_BER:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_HDC:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_AUDIO:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_ID3:
                    OnId3?.Invoke(this, new Nrsc5EntityWrappers.Id3EventInfoImpl(&evt->id3));
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_SIG:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_LOT:
                    {
                        using (Nrsc5EntityWrappers.LotCompletedInfoImpl info = new Nrsc5EntityWrappers.LotCompletedInfoImpl(&evt->lot))
                            OnLot?.Invoke(this, info);
                        break;
                    }
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_SIS:
                    //TODO
                    break;
                case Nrsc5Native.NRSC5_EVENT.NRSC5_EVENT_LOT_PROGRESS:
                    {
                        using (Nrsc5EntityWrappers.LotProgressInfoImpl info = new Nrsc5EntityWrappers.LotProgressInfoImpl(&evt->lot_progress))
                            OnLotProgress?.Invoke(this, info);
                        break;
                    }
            }
        }

        public override void Dispose()
        {
            //Make sure the radio is stopped
            Stop();

            //Close the native library
            Nrsc5Native.nrsc5_close(Handle);

            base.Dispose();
        }
    }
}
