using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

public class CopyDebugLua : Editor
{
    [MenuItem("Tools/Copy Debug Lua")]
    public static void Run() {

        string fromUrl = Application.streamingAssetsPath;
        string toUrl = Application.dataPath + "/Game/Resources";

        if(File.Exists(toUrl + "/Lua"))
            Directory.Delete(toUrl + "/Lua",true);
        CopyDirectory(fromUrl, toUrl);
        Debug.Log("copy finish:" + toUrl);
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

}
