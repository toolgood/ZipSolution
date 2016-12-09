using System;

namespace SoMain.Common.SharpCompress.Common
{
    public class ArchiveException : Exception
    {
        public ArchiveException(string message)
            : base(message)
        {
        }
    }
}