using System.IO;
using SoMain.Common.SharpCompress.Common.Rar;
using SoMain.Common.SharpCompress.Common.Rar.Headers;

namespace SoMain.Common.SharpCompress.Reader.Rar
{
    internal class NonSeekableStreamFilePart : RarFilePart
    {
        internal NonSeekableStreamFilePart(MarkHeader mh, FileHeader fh)
            : base(mh, fh)
        {
        }

        internal override Stream GetCompressedStream()
        {
            return FileHeader.PackedStream;
        }

        internal override string FilePartName
        {
            get { return "Unknown Stream - File Entry: " + FileHeader.FileName; }
        }
    }
}