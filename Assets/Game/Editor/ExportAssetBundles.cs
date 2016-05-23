using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

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

        AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

        var basePath = Application.dataPath + ExportConfigWindow.EXPORT_PREFABS_PATH.Substring(6);

        List<string> prefabs = new List<string>();
        forEachHandle(basePath, "prefab", (string filename) =>
        {
            prefabs.Add(ExportConfigWindow.EXPORT_PREFABS_PATH + filename.Replace(basePath, "").Replace(@"\","/"));
        });

        buildMap[0].assetBundleName = "prefabBundles";
        buildMap[0].assetNames = prefabs.ToArray();

        basePath = Application.dataPath + ExportConfigWindow.EXPORT_SCENE_PATH.Substring(6);

        List<string> scenes = new List<string>();
        forEachHandle(basePath, "unity", (string filename) =>
        {
            scenes.Add(ExportConfigWindow.EXPORT_SCENE_PATH + filename.Replace(basePath, "").Replace(@"\", "/"));
        });

        buildMap[1].assetBundleName = "sceneBundles";
        buildMap[1].assetNames = scenes.ToArray();

		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", buildMap,BuildAssetBundleOptions.ChunkBasedCompression, ExportConfigWindow.BUILD_TARGET);
        // BuildPipeline.BuildAssetBundles("Assets/StreamingAssets");
        AssetDatabase.Refresh();

        Debug.Log("AssetBundles 打包完成 位于：Assets/StreamingAssets");
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
        string srcPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar;
        string outPath = ExportConfigWindow.EXPORT_OUT_PATH + Path.DirectorySeparatorChar;

        forEachHandle(srcPath, "meta", (string filename) =>
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

    public static void forEachHandle(string path,string matchExt,UnityAction<string> handle)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string[] name_splits = filename.Split('.');
            string ext = name_splits[name_splits.Length-1];
            if (ext.Equals(matchExt))
            {
                handle.Invoke(filename);
            }
        }

        foreach (string dir in dirs)
        {
            forEachHandle(dir, matchExt, handle);
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
}
