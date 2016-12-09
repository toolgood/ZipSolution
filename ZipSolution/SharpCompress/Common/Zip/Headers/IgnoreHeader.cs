using System;
using System.IO;

namespace SoMain.Common.SharpCompress.Common.Zip.Headers
{
    internal class IgnoreHeader : ZipHeader
    {
        public IgnoreHeader(ZipHeaderType type)
            : base(type)
        {
        }

        internal override void Read(BinaryReader reader)
        {
        }

        internal override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}