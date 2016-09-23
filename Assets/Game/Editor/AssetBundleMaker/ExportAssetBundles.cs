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
				forEachHandle (basePath, null, (string filename) => {
					string assetPath = filename.Replace(Application.dataPath,"Assets");
					list.Add(assetPath);
				});
				AssetBundleBuild build = new AssetBundleBuild ();
				build.assetBundleName = path.Replace('.','_').Replace('/','-') +".ab";
				build.assetNames = list.ToArray();
				buildMap.Add (build);
			
			} else {
				Dictionary<string,List<string>> dic = new Dictionary<string, List<string>> ();

				forEachHandle (basePath, null, (string filename) => {
					
					string assetPath = filename.Replace(Application.dataPath,"Assets");

					string baseFile = Path.GetFileNameWithoutExtension(assetPath);
					List<string> list = null;
					dic.TryGetValue(baseFile,out list);
					if(list == null){
						list = new List<string>();
						dic.Add(baseFile,list);
					}
					list.Add(assetPath);
				});

				foreach (string baseFile in dic.Keys) {
					string abName = "";
					string _path = dic [baseFile] [0];
					if (dic [baseFile].Count > 1) { //mix
						string mixPath = Path.GetDirectoryName (_path) + "/" + Path.GetFileNameWithoutExtension (_path);
						abName = mixPath.Replace ('.', '_').Replace ('/', '-') + ".ab";
					} else {
						abName = _path.Replace ('.', '_').Replace ('/', '-') + ".ab";
					}


					AssetBundleBuild build = new AssetBundleBuild ();
					build.assetBundleName = abName.Substring(22);
					build.assetNames = dic[baseFile].ToArray();
					buildMap.Add(build);
				}
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

		forEachHandle(srcPath, new List<string>(){"meta"}, (string filename) =>
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
			if (filename.EndsWith ("meta") || string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(filename)))
				continue;

            string[] name_splits = filename.Split('.');
            string ext = name_splits[name_splits.Length - 1];
			if (matchExts == null) {
				handle.Invoke (filename);
			}else if(matchExts.Contains (ext)) {
				handle.Invoke (filename);
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
