using System.IO;
using System.Linq;
using SoMain.Common.SharpCompress.Common;
using SoMain.Common.SharpCompress.Common.Tar;

namespace SoMain.Common.SharpCompress.Archive.Tar
{
    public class TarArchiveEntry : TarEntry, IArchiveEntry
    {
        internal TarArchiveEntry(TarArchive archive, TarFilePart part, CompressionType compressionType)
            : base(part, compressionType)
        {
            Archive = archive;
        }

        public virtual Stream OpenEntryStream()
        {
            return Parts.Single().GetCompressedStream();
        }

        #region IArchiveEntry Members
        public IArchive Archive { get; private set; }

        public bool IsComplete
        {
            get { return true; }
        }

        #endregion
    }
}