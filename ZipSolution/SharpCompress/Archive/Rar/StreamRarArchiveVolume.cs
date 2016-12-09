using System.Collections.Generic;
using System.IO;
using SoMain.Common.SharpCompress.Common;
using SoMain.Common.SharpCompress.Common.Rar;
using SoMain.Common.SharpCompress.Common.Rar.Headers;
using SoMain.Common.SharpCompress.IO;

namespace SoMain.Common.SharpCompress.Archive.Rar
{
    internal class StreamRarArchiveVolume : RarVolume
    {
        internal StreamRarArchiveVolume(Stream stream, string password, Options options)
            : base(StreamingMode.Seekable, stream, password, options)
        {
        }

        internal override IEnumerable<RarFilePart> ReadFileParts()
        {
            return GetVolumeFileParts();
        }

        internal override RarFilePart CreateFilePart(FileHeader fileHeader, MarkHeader markHeader)
        {
            return new SeekableFilePart(markHeader, fileHeader, Stream, Password);
        }
    }
}