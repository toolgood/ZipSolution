using System.Collections.Generic;
using System.IO;
using SoMain.Common.SharpCompress.Common;
using SoMain.Common.SharpCompress.Common.Rar;
using SoMain.Common.SharpCompress.Common.Rar.Headers;
using SoMain.Common.SharpCompress.IO;

namespace SoMain.Common.SharpCompress.Reader.Rar
{
    public class RarReaderVolume : RarVolume
    {
        internal RarReaderVolume(Stream stream, string password, Options options)
            : base(StreamingMode.Streaming, stream, password, options)
        {
        }

        internal override RarFilePart CreateFilePart(FileHeader fileHeader, MarkHeader markHeader)
        {
            return new NonSeekableStreamFilePart(markHeader, fileHeader);
        }

        internal override IEnumerable<RarFilePart> ReadFileParts()
        {
            return GetVolumeFileParts();
        }
    }
}