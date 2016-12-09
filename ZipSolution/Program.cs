using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoMain.Common.SharpCompress;
using SoMain.Common.SharpCompress.Writer.Zip;
using System.IO;
using SoMain.Common.SharpCompress.Common;
using SoMain.Common.SharpCompress.Writer;
using System.Text.RegularExpressions;
using SoMain.Common.SharpCompress.Compressor.Deflate;

namespace ZipSolution
{
    class Program
    {
        static List<string> IgnoreExt = new List<string>()
            {
                "*.suo",
                "*.user",
                "*.vssscc",
                "*.vspscc",
                "*.scc",
                "upgradelog*.xml",
            };
        static List<string> IgnoreFolderName = new List<string>(){
            "_UpgradeReport_Files",
            "Backup",
            "TestResults",
            "ClientBin",
            "bin",
            "obj",
            "Debug",
            "Release",
            "x64",
            "ARM",
            "ipch",
            "packages",
            ".svn",
            "uploads",
            "upload",
            "$tf",
            ".vs",
            ".git",
            ".idea"
        };
        static void Main(string[] args)
        {
            var exeFileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var dir = System.Environment.CurrentDirectory;
            var userdir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var dirName = Path.GetFileName(dir);
            var solutionFileName = Directory.GetFiles(dir, ".sln", SearchOption.AllDirectories);
            foreach (var item in solutionFileName)
            {
                IgnoreExt.Add(Path.GetFileNameWithoutExtension(item) + ".sdf");
                IgnoreExt.Add(Path.GetFileNameWithoutExtension(item) + ".opensdf");
            }
            IgnoreExt.Add(exeFileName);

            var filename = Path.Combine(userdir, dirName + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + ".zip");
            var fs = File.Open(filename, FileMode.CreateNew);
            var ci = new CompressionInfo();
            ci.Type = CompressionType.LZMA;
            ci.DeflateCompressionLevel = CompressionLevel.BestCompression;
            using (ZipWriter w = new ZipWriter(fs, ci, ""))
            {
                var cDir = new DirectoryInfo(dir);
                ReadFolder(cDir, dir, w);
            }
            fs.Dispose();

            OpenFolder(filename);


        }
        static void ReadFolder(DirectoryInfo cDir, string mainDir, ZipWriter writer)
        {
            var dirs = cDir.GetDirectories();
            foreach (var dir in dirs)
            {
                if (IgnoreFolderName.Contains(dir.Name)) continue;
                ReadFolder(dir, mainDir, writer);
            }
            var files = cDir.GetFiles();
            foreach (var file in files)
            {
                bool Ignore = false;
                foreach (var item in IgnoreExt)
                {
                    var re = item.Replace(".", "\\.").Replace("*", ".*");
                    if (Regex.IsMatch(file.Name, re, RegexOptions.IgnoreCase)) Ignore = true;
                }
                if (Ignore) continue;


                var filepath = file.FullName.Substring(mainDir.Length).TrimStart(@"\/".ToArray());
                writer.Write(filepath, file);
            }


        }

        static void OpenFolder(string folderPath)
        {
            var ext = Path.GetExtension(folderPath);

            if (!File.Exists(folderPath)) return;
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = " /select," + folderPath;
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(folderPath));
                throw;
            }
        }

    }
}
