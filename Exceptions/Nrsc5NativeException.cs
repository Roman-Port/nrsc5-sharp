using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Exceptions
{
    public class Nrsc5NativeException : Exception
    {
        public Nrsc5NativeException(int code) : base($"The nrsc5 library returned error code {code}.")
        {
            this.code = code;
        }

        private readonly int code;

        public int Nrsc5Code => code;
    }
}
