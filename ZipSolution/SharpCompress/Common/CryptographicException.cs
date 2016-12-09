using System;

namespace SoMain.Common.SharpCompress.Common
{
    public class CryptographicException : Exception
    {
        public CryptographicException(string message)
            : base(message)
        {
        }
    }
}