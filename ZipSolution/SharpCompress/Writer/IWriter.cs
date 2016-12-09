using System;
using System.IO;
using SoMain.Common.SharpCompress.Common;

namespace SoMain.Common.SharpCompress.Writer
{
    public interface IWriter : IDisposable
    {
        ArchiveType WriterType { get; }
        void Write(string filename, Stream source, DateTime? modificationTime);
    }
}