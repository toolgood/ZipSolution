using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipSolution
{
    [Serializable]
    public class ZipInfo
    {
        public DateTime ZipTime { get; set; }
        public string ZipFileName { get; set; }
        public string FileCount { get; set; }
        public long FIleSize { get; set; }

        public List<ZipFileInfo> Files { get; set; }
    }
    [Serializable]
    public class ZipFileInfo
    {
        public string FilePath { get; set; }// 最后带/

        public bool IsFolder { get; set; }
        public bool IsDelete { get; set; }
        public List<ZipFileChange> Changes { get; set; } 
        public List<ZipFileInfo> SubFiles { get; set; }
    }
    [Serializable]
    public class ZipFileChange
    {
        public string FileSize { get; set; }
        public string FileMd5 { get; set; }
        public DateTime LastWriteTime { get; set; }

        public bool IsDelete { get; set; }
        public string StartZipFileName { get; set; }
        public string EndZipFileName { get; set; }
    }



}
