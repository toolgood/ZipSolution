using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using Ionic.Zip;
using UpdateWebsite.Datas;
using System.IO;
using System.Text.RegularExpressions;

namespace UpdateWebsite
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WebsiteInfo> websites;
            try
            {
                websites = ReadWebsiteInfos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return;
            }
            //默认 备份
            if ((args.Length == 0 || (args.Length == 1 && args[0].ToLower() == "update")) && websites.Count == 1)
            {
                FileBackup(websites[0]);
                UpdateFiles(websites[0]);
                return;
            }
            // 恢复
            if (args.Length == 1 && args[0].ToLower() == "restore" && websites.Count == 1)
            {
                RestoreFile(websites[0]);
                return;
            }
            if (args.Length == 1 && websites.Count == 1)
            {
                Console.WriteLine("参数错误，请使用下面的命令行");
                Console.WriteLine("UpdateWebsite [update|restore]");
            }
            if (args.Length == 1 )
            {
                Console.WriteLine("缺少参数，当前website.json内有多个项目，请使用下面的命令行");
                Console.WriteLine("UpdateWebsite [update|restore] [code]");
            }
            if (args[0].ToLower() != "update" && args[0].ToLower() != "restore")
            {
                Console.WriteLine("第一位参数错误，请使用下面的命令行");
                Console.WriteLine("UpdateWebsite [update|restore] [code]");
            }
            var website = websites.FirstOrDefault(q => q.Code == args[1]);
            if (website == null)
            {
                Console.WriteLine("项目CODE不存在");
                return;
            }
            if (args[0].ToLower() == "update")
            {
                FileBackup(website);
                UpdateFiles(website);
            }
            else
            {
                RestoreFile(website);
                return;
            }
        }

        static void RestoreFile(WebsiteInfo websiteInfo)
        {
            var zipFile = GetRestoreFile(websiteInfo);
            if (zipFile == null)
            {
                Console.WriteLine("无备份文件..");

                return;
            }
            Console.WriteLine("开始恢复...");

            var dir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(zipFile));
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            using (ZipFile zip = new ZipFile(zipFile, Encoding.Default))
            {
                zip.ExtractAll(dir);
            }
            var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            var app_offline = Path.Combine(websiteInfo.WebsiteFolder, "app_offline.htm");
            File.Create(app_offline).Close();
            foreach (var srcFile in files)
            {
                var tarFile = srcFile.Replace(dir, websiteInfo.WebsiteFolder);
                Directory.CreateDirectory(Path.GetDirectoryName(tarFile));
                while (true)
                {
                    try
                    {
                        File.Copy(srcFile, tarFile, true);
                        break;
                    }
                    catch (Exception) { }
                    System.Threading.Thread.Sleep(10);
                }
            }
            File.Delete(app_offline);
            Directory.Delete(dir,true);
            Console.WriteLine("恢复成功...");
        }

        static string GetRestoreFile(WebsiteInfo websiteInfo)
        {
            var files = Directory.GetFiles(websiteInfo.BackupFolder, websiteInfo.Name + "_*.zip", SearchOption.AllDirectories).ToList();
            if (files.Count == 0)
            {
                return null;
            }
            files = files.OrderByDescending(q => q).ToList();
            return files[0];
        }

        static void UpdateFiles(WebsiteInfo websiteInfo)
        {
            List<string> files = new List<string>();
            var cDir = new DirectoryInfo(websiteInfo.PreReleaseFolder);
            if (cDir.Exists == false)
            {
                Console.WriteLine("预发布文件不存在");
                return;
            }
            GetUpdateFile(websiteInfo, cDir, files);

            Console.WriteLine("开始更新...");
            var app_offline = Path.Combine(websiteInfo.WebsiteFolder, "app_offline.htm");
            File.Create(app_offline).Close();
            foreach (var srcFile in files)
            {
                var tarFile = srcFile.Replace(websiteInfo.PreReleaseFolder, websiteInfo.WebsiteFolder);
                Directory.CreateDirectory(Path.GetDirectoryName(tarFile));
                while (true)
                {
                    try
                    {
                        File.Copy(srcFile, tarFile, true);
                        break;
                    }
                    catch (Exception) { }
                    System.Threading.Thread.Sleep(10);
                }
            }
            File.Delete(app_offline);
            Console.WriteLine("更新成功...");
        }
        static void GetUpdateFile(WebsiteInfo websiteInfo, DirectoryInfo cDir, List<string> outFiles)
        {
            var mainDir = websiteInfo.WebsiteFolder;
            var dirs = cDir.GetDirectories();
            foreach (var dir in dirs)
            {
                if (websiteInfo.BackupExclude.FolderName.Contains(dir.Name)) continue;
                GetUpdateFile(websiteInfo, dir, outFiles);
            }
            var files = cDir.GetFiles();
            foreach (var file in files)
            {
                bool Ignore = false;
                foreach (var item in websiteInfo.BackupExclude.FileName)
                {
                    var re = "^" + item.Replace(".", "\\.").Replace("*", ".*") + "$";
                    if (Regex.IsMatch(file.Name, re, RegexOptions.IgnoreCase)) Ignore = true;
                }
                if (Ignore) continue;
                outFiles.Add(file.FullName);
            }
        }

        static void FileBackup(WebsiteInfo websiteInfo)
        {
            if (websiteInfo.UseBackup.ToLower() == "true")
            {
                var backupFileName = "";
                if (websiteInfo.BackupRate.ToLower() == "day")
                {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd") + ".zip");
                }
                else if (websiteInfo.BackupRate.ToLower() == "hour")
                {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd_HH") + ".zip");
                }
                else if (websiteInfo.BackupRate.ToLower() == "minute")
                {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd_HHmm") + ".zip");
                }
                else
                {
                    Console.WriteLine("备份频率有误，不进行备份");
                    return;
                }

                if (File.Exists(backupFileName))
                {
                    Console.WriteLine("已备份，不进行备份");
                    return;
                }
                Console.WriteLine("开始备份...");
                Directory.CreateDirectory(Path.GetDirectoryName(backupFileName));
                using (ZipFile zip = new ZipFile(Encoding.Default))
                {
                    var cDir = new DirectoryInfo(websiteInfo.WebsiteFolder);
                    ReadFolder(websiteInfo, cDir, zip);
                    var fs = File.Open(backupFileName, FileMode.CreateNew);
                    zip.Save(fs);
                    fs.Dispose();
                }
                Console.WriteLine("备份成功...");
            }
            else
            {
                Console.WriteLine("不进行备份");
                return;
            }
        }

        static void ReadFolder(WebsiteInfo websiteInfo, DirectoryInfo cDir, ZipFile writer)
        {
            var mainDir = websiteInfo.WebsiteFolder;
            var dirs = cDir.GetDirectories();
            foreach (var dir in dirs)
            {
                if (websiteInfo.BackupExclude.FolderName.Contains(dir.Name)) continue;
                ReadFolder(websiteInfo, dir, writer);
            }
            var files = cDir.GetFiles();
            foreach (var file in files)
            {
                bool Ignore = false;
                foreach (var item in websiteInfo.BackupExclude.FileName)
                {
                    var re = "^" + item.Replace(".", "\\.").Replace("*", ".*") + "$";
                    if (Regex.IsMatch(file.Name, re, RegexOptions.IgnoreCase)) Ignore = true;
                }
                if (Ignore) continue;

                var filepath = file.FullName.Substring(mainDir.Length).TrimStart(@"\/".ToArray());

                var fs = File.ReadAllBytes(file.FullName);
                writer.AddEntry(filepath, fs);
            }
        }

        static List<WebsiteInfo> ReadWebsiteInfos()
        {
            if (File.Exists("website.json") == false)
            {
                CreateJson();
            }
            var json = File.ReadAllText("website.json");
            return JsonMapper.ToObject<List<WebsiteInfo>>(json);
        }

        static void CreateJson()
        {
            var json = @"[
  {
    ""Name"": ""项目名称，备份名称使用这个"",
    ""Code"": ""项目编号,执行时使用这个"",
    ""WebsiteFolder"": ""项目文件夹"",
    ""PreReleaseFolder"": ""预发布文件夹"",
    ""BackupFolder"": ""备份文件夹"",
    ""UseBackup"": ""是否使用备份功能，可选： true false"",
    ""BackupRate"": ""备份频率，可选：day  hour minute "",
    ""UpdateExclude"": {
      ""FileName"": [ ""更新时，要排除的文件,支持 * 作为通配符 "" ],
      ""FolderName"": [ ""更新时，要排除的文件夹"" ]
    },
    ""BackupExclude"": {
      ""FileName"": [ ""备份时，要排除的文件,支持 * 作为通配符"" ],
      ""FolderName"": [ ""备份时，要排除的文件夹"" ]
    }
  }
]";
            File.WriteAllText("website.json", json);
        }

    }
}
