using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

public class MakeDebugFile : Editor
{
    public static string luajitPath = Application.dataPath + "/../build/luajit-2.0.4/src";

    //[MenuItem("Tools/Copy")]
    public static void CopyLuaTxt() {

        string fromUrl = Application.streamingAssetsPath + "/@Lua";
        string toUrl = Application.dataPath + "/Game/Resources/@Lua";

		AssetDatabase.DeleteAsset ("Assets/Game/Resources/@Lua");
        Directory.CreateDirectory(toUrl);

		CopyAndChangeDirectory(fromUrl, toUrl,new List<string>{".lua"});
        AssetDatabase.Refresh();
        Debug.Log("copy finish:" + toUrl);
    }

    //[MenuItem("Tools/test")]
    //public static void Test()
    //{
    //    TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/test.txt");

    //    byte[] encrypts = LUtil.AESEncrypt(textAsset.bytes,LGameConfig.EncryptKey32,"1234567890123456");
    //    byte[] content = LUtil.AESDecrypt(encrypts, LGameConfig.EncryptKey32, "1234567890123456");

    //    Debug.Log(System.Text.Encoding.UTF8.GetString(content));
    //}

    [MenuItem("Tools/Edit Atlas Suffix")]
    public static void EditAtlasSuffix()
    {
        string basePath = Application.dataPath + "/Game/Resources/Atlas";
        Helper.forEachHandle(basePath, new List<string>{"txt","tpsheet"}, (string path) =>
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
        AssetDatabase.Refresh();
        Debug.Log("Atlas后缀修改完成");
    }

	public static void CopyAndChangeDirectory(string sourceDirName, string destDirName,List<string> exts)
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
				if (file.EndsWith("meta"))
                    continue;

                string destFileName = destDirName + Path.GetFileName(file);
                if (exts.Contains(Path.GetExtension(file)))
                {
                    if (LGameConfig.GetInstance().isEncrypt)
                    {
                        destFileName = destDirName + Path.GetFileNameWithoutExtension(file) + ".bytes";
                        //luajit
                        Helper.RunCmd(luajitPath + "/luajit.exe", string.Format("-b {0} {1}", file, destFileName), luajitPath);
                        ////encrypt

                        //FileStream fr = new FileStream(destFileName, FileMode.Open);
                        //byte[] encryptStrs = new byte[fr.Length];
                        //fr.Read(encryptStrs, 0, encryptStrs.Length);
                        //fr.Close();

                        //FileStream fs = new FileStream(destFileName, FileMode.Create);
                        //fs.Write(encryptStrs, 0, encryptStrs.Length);
                        //fs.Flush();
                        //fs.Close();
                    }
                    else
                    {
                        destFileName = destDirName + Path.GetFileNameWithoutExtension(file) + ".txt";
                        File.Copy(file, destFileName, true);
                    }
                }
                else
                {
                    File.Copy(file, destFileName, true);
                }
                File.SetAttributes(destFileName, FileAttributes.Normal);
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
				CopyAndChangeDirectory(dir, destDirName + Path.GetFileName(dir),exts);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

}
