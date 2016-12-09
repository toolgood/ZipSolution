using System.IO;
using SoMain.Common.SharpCompress.Common.Tar.Headers;
using SoMain.Common.SharpCompress.IO;

namespace SoMain.Common.SharpCompress.Common.Tar
{
    internal class TarFilePart : FilePart
    {
        private readonly Stream seekableStream;

        internal TarFilePart(TarHeader header, Stream seekableStream)
        {
            this.seekableStream = seekableStream;
            Header = header;
        }

        internal TarHeader Header { get; private set; }

        internal override string FilePartName
        {
            get { return Header.Name; }
        }

        internal override Stream GetCompressedStream()
        {
            if (seekableStream != null)
            {
                seekableStream.Position = Header.DataStartPosition.Value;
                return new ReadOnlySubStream(seekableStream, Header.Size);
            }
            return Header.PackedStream;
        }

        internal override Stream GetRawStream()
        {
            return null;
        }
    }
}