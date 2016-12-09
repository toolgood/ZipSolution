using System.IO;

namespace SoMain.Common.SharpCompress.Common
{
    public abstract class FilePart
    {
        internal abstract string FilePartName { get; }

        internal abstract Stream GetCompressedStream();
        internal abstract Stream GetRawStream();
    }
}