namespace SoMain.Common.SharpCompress.Common
{
    public class IncompleteArchiveException : ArchiveException
    {
        public IncompleteArchiveException(string message)
            : base(message)
        {
        }
    }
}