using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class Helper
{
    public static string RunCmd(string command, string args, string workDir)
    {
        //例Process
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = command;           //确定程序名
        p.StartInfo.Arguments = args;    //确定程式命令行
        p.StartInfo.WorkingDirectory = workDir; //工作目录
        p.StartInfo.UseShellExecute = false;        //Shell的使用
        p.StartInfo.RedirectStandardInput = true;   //重定向输入
        p.StartInfo.RedirectStandardOutput = true; //重定向输出
        p.StartInfo.RedirectStandardError = true;   //重定向输出错误
        p.StartInfo.CreateNoWindow = true;          //设置置不显示示窗口
        p.Start();
        return p.StandardOutput.ReadToEnd();        //输出出流取得命令行结果果
    }

    public static void CopyDirectory(string sourceDirName, string destDirName)
    {
        try
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                //if (File.Exists(destDirName + Path.GetFileName(file)))
                //    continue;
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public static void forEachHandle(string path, List<string> matchExts, UnityAction<string> handle)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            if (filename.EndsWith("meta") || string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(filename)))
                continue;

            string[] name_splits = filename.Split('.');
            string ext = name_splits[name_splits.Length - 1];
            if (matchExts == null)
            {
                handle.Invoke(filename);
            }
            else if (matchExts.Contains(ext))
            {
                handle.Invoke(filename);
            }
        }

        foreach (string dir in dirs)
        {
            forEachHandle(dir, matchExts, handle);
        }
    }

    
}
