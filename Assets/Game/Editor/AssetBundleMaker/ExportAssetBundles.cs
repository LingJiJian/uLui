using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ExportAssetBundles : Editor
{
    public static void Run()
    {
		MakeDebugFile.CopyLuaTxt ();
		CreateAssetBundles();
        CreateZipFile();
        CreateVersionFile();
        AssetDatabase.Refresh();
    }

	public static void CreateAssetBundles(){

		AssetDatabase.DeleteAsset ("Assets/StreamingAssets/Ab");
		Directory.CreateDirectory (Application.streamingAssetsPath + "/Ab");

		string[] paths = new string[]{ "Audios","Prefabs","Atlas","@Lua","Scenes"};
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();

		foreach (string path in paths) {

			string basePath = Application.dataPath + "/Game/Resources/" + path;

			if (path.StartsWith ("@")) {
			
				List<string> list = new List<string> ();
                Helper.forEachHandle (basePath, null, (string filename) => {
					string assetPath = filename.Replace(Application.dataPath,"Assets");
					list.Add(assetPath);
				});
				AssetBundleBuild build = new AssetBundleBuild ();
				build.assetBundleName = path +".ab";
				build.assetNames = list.ToArray();
				buildMap.Add (build);
			
			} else {

                Helper.forEachHandle (basePath, null, (string filename) => {
					string assetPath = filename.Replace(Application.dataPath,"Assets");
                    string baseName = assetPath.Substring(22);
                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = baseName.Replace('.', '_').Replace(Path.DirectorySeparatorChar, '-') + ".ab";
                    build.assetNames = new string[] { assetPath };
                    buildMap.Add(build);
                });
			}
		}
		BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath+"/Ab", buildMap.ToArray (),
			BuildAssetBundleOptions.DeterministicAssetBundle |
			BuildAssetBundleOptions.DisableWriteTypeTree |
			BuildAssetBundleOptions.ChunkBasedCompression,ExportConfigWindow.BUILD_TARGET);
	}

    static void CreateVersionFile()
    {
		string resPath = ExportConfigWindow.EXPORT_OUT_PATH + Path.DirectorySeparatorChar;
        StringBuilder versions = new StringBuilder();

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
        string srcPath = Application.streamingAssetsPath + "/Ab";
        string outPath = ExportConfigWindow.EXPORT_OUT_PATH + Path.DirectorySeparatorChar;

		Helper.forEachHandle(srcPath, new List<string>(){"meta"}, (string filename) =>
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
