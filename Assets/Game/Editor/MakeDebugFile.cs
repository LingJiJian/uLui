using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

public class MakeDebugFile : Editor
{
    [MenuItem("Tools/Copy Debug Lua")]
    public static void Run() {

        string fromUrl = Application.streamingAssetsPath + "/Lua";
        string toUrl = Application.dataPath + "/Game/Resources/Lua";

        if (Directory.Exists(toUrl))
            Directory.Delete(toUrl, true);

        Directory.CreateDirectory(toUrl);

        CopyDirectory(fromUrl, toUrl);
        Debug.Log("copy finish:" + toUrl);
    }

    [MenuItem("Tools/Edit Atlas Suffix")]
    public static void EditAtlasSuffix()
    {
        string basePath = Application.dataPath + "/Game/Resources/Atlas";
        List<string> list = new List<string>();
        list.Add("tpsheet");
        list.Add("txt");
        forEachHandle(basePath, list, (string path) =>
        {
            string[] name_splits = path.Split('.');
            string ext = name_splits[name_splits.Length - 1];
            if(ext == "tpsheet")
            {
                File.Move(name_splits[0] + ".tpsheet", name_splits[0] + ".txt");
            }
            else
            {
                File.Move(name_splits[0] + ".txt", name_splits[0] + ".tpsheet");
            }
        });
        Debug.Log("Atlas后缀修改完成");
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
                if (file.IndexOf(".meta") > -1)
                    continue;
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                File.Move(destDirName + Path.GetFileName(file), Path.ChangeExtension(destDirName + Path.GetFileName(file), ".txt"));
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    

    public static void forEachHandle(string path, List<string> matchExts, UnityAction<string> handle)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string[] name_splits = filename.Split('.');
            string ext = name_splits[name_splits.Length - 1];
            if (matchExts.Contains(ext))
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
