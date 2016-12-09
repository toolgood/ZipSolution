using System;
#if !PORTABLE && !NETFX_CORE
using System.IO;
#endif

namespace SoMain.Common.SharpCompress.Common
{
    public interface IVolume : IDisposable
    {
    }
}