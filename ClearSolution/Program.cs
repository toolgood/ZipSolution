using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClearSolution
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
                "package-lock.json",
                "*.map",
                 "*.sdf",
                 "*.opensdf",
                 "*.log",
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
            ".idea",
            "node_modules",
            //"dist"
        };

        static void Main(string[] args)
        {
            var dir = System.Environment.CurrentDirectory;
            var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            foreach (var file in files) {
                bool IsDelete = false;
                foreach (var item in IgnoreExt) {
                    var re = item.Replace(".", "\\.").Replace("*", ".*");
                    if (Regex.IsMatch(file, re, RegexOptions.IgnoreCase)) IsDelete = true;
                }
                if (IsDelete) {
                    try {
                        File.Delete(file);
                    } catch (Exception e) {
                    }
                }
            }
            var folders = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
            folders = folders.OrderByDescending(q => q.Length).ToArray();
            foreach (var folder in folders) {
                var f = folder.Substring(dir.Length);
                bool IsDelete = false;
                foreach (var item in IgnoreFolderName) {
                    var re =@"\\"+ item.Replace(".", "\\.").Replace("*", ".*")+@"(\\|$)";
                    if (Regex.IsMatch(f, re, RegexOptions.IgnoreCase)) IsDelete = true;
                }
                if (IsDelete) {
                    try {
                        Directory.Delete(folder, true);
                    } catch (Exception e) {
                    }
                }
            }
        }
 

    }
}
