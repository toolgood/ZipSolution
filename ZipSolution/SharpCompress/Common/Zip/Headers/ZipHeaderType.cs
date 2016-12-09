namespace SoMain.Common.SharpCompress.Common.Zip.Headers
{
    internal enum ZipHeaderType
    {
        Ignore,
        LocalEntry,
        DirectoryEntry,
        DirectoryEnd,
        Split,
    }
}