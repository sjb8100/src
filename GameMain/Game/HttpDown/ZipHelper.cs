using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

public class ZipHelper
{
    public static bool UnZip(string fileToUnZip, string zipedFolder, string password)
    {
        bool result = true;
        FileStream fs = null;
        ZipInputStream zipStream = null;
        ZipEntry ent = null;
        string fileName;

        if (!File.Exists(fileToUnZip))
        {
            UnityEngine.Debug.Log("解压的文件不存在");
            return false;
        }
        if (!Directory.Exists(zipedFolder))
        {
            UnityEngine.Debug.Log("解压的目录不存在,创建目录");
            Directory.CreateDirectory(zipedFolder);
        }
        try
        {
            zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));

            if (zipStream == null)
            {
                string strLog = string.Format("打开文件失败.{0}", fileToUnZip);
                UnityEngine.Debug.Log(strLog);
            }

            if (!string.IsNullOrEmpty(password))
                zipStream.Password = password;

            while ((ent = zipStream.GetNextEntry()) != null)
            {
                if (!string.IsNullOrEmpty(ent.Name))
                {
                    fileName = Path.Combine(zipedFolder, ent.Name);
                    if (fileName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(fileName);
                        continue;
                    }

                    if (File.Exists(fileName))
                    {
                        //string eee = string.Format("文件存在:{0}", fileName);
                        //UnityEngine.Debug.Log(eee);
                        continue;
                    }

                    //string strLog = string.Format("解压文件:{0}", fileName);
                    //UnityEngine.Debug.Log(strLog);

                    fs = File.Create(fileName);

                    int size = 2048;
                    byte[] data = new byte[size];
                    while (true)
                    {
                        size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                            fs.Write(data, 0, data.Length);
                        else
                            break;
                    }
                }
            }
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
            if (zipStream != null)
            {
                zipStream.Close();
                zipStream.Dispose();
            }
            if (ent != null)
            {
                ent = null;
            }
            GC.Collect();
            GC.Collect(1);
        }
        return result;
    }

    public static bool UnZip(string fileToUnZip, string zipedFolder)
    {
        bool result = UnZip(fileToUnZip, zipedFolder, null);
        return result;
    }
}

