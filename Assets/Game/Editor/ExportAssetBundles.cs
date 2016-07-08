using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class ExportAssetBundles : Editor
{
    public static void Run()
    {
        CreateResources();
        CreateZipFile();
        CreateVersionFile();
    }

    static void CreateResources()
    {
        //// 选择的要保存的对象  4.x
        //Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        ////打包  
        //BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

        List< AssetBundleBuild> buildMap = new List<AssetBundleBuild>();

        var basePath = Application.dataPath + ExportConfigWindow.EXPORT_PREFABS_PATH.Substring(6);

        List<string> prefabs = new List<string>();
        List<string> exts = new List<string>();
        exts.Add("prefab");
        exts.Add("txt");
        exts.Add("ogg");
        exts.Add("png");
        exts.Add("wav");
        exts.Add("mp3");

        forEachHandle(basePath, exts, (string filename) =>
        {
            prefabs.Add(ExportConfigWindow.EXPORT_PREFABS_PATH + filename.Replace(basePath, "").Replace(@"\", "/"));
        });

        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = "prefabBundles";
        build.assetNames = prefabs.ToArray();
        buildMap.Add(build);

        basePath = Application.dataPath + ExportConfigWindow.EXPORT_SCENE_PATH.Substring(6);

        List<string> scenes = new List<string>();
        exts = new List<string>();
        exts.Add("unity");
        forEachHandle(basePath, exts, (string filename) =>
        {
            scenes.Add(ExportConfigWindow.EXPORT_SCENE_PATH + filename.Replace(basePath, "").Replace(@"\", "/"));
        });

        build = new AssetBundleBuild();
        build.assetBundleName = "sceneBundles";
        build.assetNames = scenes.ToArray();
        buildMap.Add(build);

        basePath = Application.dataPath + ExportConfigWindow.EXPORT_ATLAS_PATH.Substring(6);

        List<string> atlas = new List<string>();
        exts = new List<string>();
        exts.Add("png");
        forEachHandle(basePath, exts, (string filename) =>
        {
            atlas.Add(ExportConfigWindow.EXPORT_ATLAS_PATH + filename.Replace(basePath, "").Replace(@"\", "/").Split('.')[0]);
        });
        for(int i=0;i< atlas.Count; i++)
        {
            build = new AssetBundleBuild();
            string[] strs = atlas[i].Split('/');
            build.assetBundleName = atlas[i].Split('/')[strs.Length-1];
            build.assetNames = new string[] { atlas[i] + ".txt" , atlas[i] + ".png" };
            buildMap.Add(build);
        }

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, ExportConfigWindow.BUILD_TARGET);
        AssetDatabase.Refresh();

        Debug.Log("AssetBundles 打包完成 位于：Assets/StreamingAssets");
    }

    private static void processCommand(string command, string argument)
    {
        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo(command);
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = true;

        if (start.UseShellExecute)
        {
            start.RedirectStandardOutput = false;
            start.RedirectStandardError = false;
            start.RedirectStandardInput = false;
        }
        else
        {
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        }

        System.Diagnostics.Process p = System.Diagnostics.Process.Start(start);

        if (!start.UseShellExecute)
        {
            Debug.Log(p.StandardOutput);
            Debug.Log(p.StandardError);
        }

        p.WaitForExit();
        p.Close();
    }

    static void CreateVersionFile()
    {
        string resPath = Application.dataPath + Path.DirectorySeparatorChar;
        // 获取Res文件夹下所有文件的相对路径和MD5值  
        //string[] files = Directory.GetFiles(resPath, "*", SearchOption.AllDirectories);
        StringBuilder versions = new StringBuilder();

        //for (int i = 0, len = files.Length; i < len; i++)
        //{
        //    string filePath = files[i];
        //    string extension = filePath.Substring(files[i].LastIndexOf("."));
        //    if (extension == ".zip")
        //    {
        //        string relativePath = filePath.Replace(resPath, "").Replace("\\", "/");
        //        string md5 = ExportAssetBundles.MD5File(filePath);
        //        versions.Append(relativePath).Append(",").Append(md5).Append("\n");
        //    }
        //}
        string zipPath = resPath + LGameConfig.UPDATE_FILE_ZIP;
        if (!File.Exists(zipPath))
        {
            Debug.LogWarning("热更zip包不存在");
            return;
        }
        string md5 = ExportAssetBundles.MD5File(zipPath);
        versions.Append(LGameConfig.UPDATE_FILE_ZIP).Append(",").Append(md5);

        // 生成配置文件  
        FileStream stream = new FileStream(resPath + "version.ver", FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();

        Debug.Log(" 版本文件： " + resPath + "version.ver");
    }

    static void CreateZipFile()
    {
        //所有lua + assetbundle
        string srcPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar;
        string outPath = ExportConfigWindow.EXPORT_OUT_PATH + Path.DirectorySeparatorChar;
        List<string> ext = new List<string>();
        ext.Add("meta");
        forEachHandle(srcPath, ext, (string filename) =>
        {
            File.Delete(@filename);
        });

        if (!Directory.Exists(srcPath))
        {
            Directory.CreateDirectory(srcPath);
        }
        LUtil.PackFiles(outPath + LGameConfig.UPDATE_FILE_ZIP, srcPath);

        Debug.Log(" 热更zip包： " + outPath + LGameConfig.UPDATE_FILE_ZIP);
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

    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("md5file() fail, error:" + ex.Message);
        }
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
}
