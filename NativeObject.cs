using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Nrsc5Sharp
{
    public abstract class NativeObject : IDisposable
    {
        public NativeObject(IntPtr handle)
        {
            this.handle = handle;
            gc = GCHandle.Alloc(this, GCHandleType.Normal);
        }

        private readonly IntPtr handle;
        private readonly GCHandle gc;
        private bool disposed;

        /// <summary>
        /// The address of this type that can be used to resolve it from an unmanaged source. Use this as an opqaue type when passing callback functions.
        /// </summary>
        protected IntPtr SelfReference => (IntPtr)gc;

        /// <summary>
        /// Handle of the native object.
        /// </summary>
        protected IntPtr Handle
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);
                return handle;
            }
        }

        public virtual void Dispose()
        {
            gc.Free();
            disposed = true;
        }
    }
}
