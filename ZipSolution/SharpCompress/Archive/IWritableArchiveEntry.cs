using System.IO;

namespace SoMain.Common.SharpCompress.Archive
{
    internal interface IWritableArchiveEntry
    {
        Stream Stream { get; }
    }
}