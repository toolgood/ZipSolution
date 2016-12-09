using System.IO;

namespace SoMain.Common.SharpCompress.Common.Tar
{
    public class TarVolume : Volume
    {
        public TarVolume(Stream stream, Options options)
            : base(stream, options)
        {
        }
    }
}