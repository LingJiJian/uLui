using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;
using System.IO;
using System;
using SLua;

[CustomLuaClassAttribute]
public class LResUpdate : MonoBehaviour
{
    public static readonly string VERSION_FILE = "version.ver";

    private Dictionary<string, string> LocalResVersion;
    private Dictionary<string, string> ServerResVersion;
    private List<string> NeedDownFiles;
    private bool NeedUpdateLocalVersionFile = false;
    private int doneCount;

    public UnityAction onCompleteHandler;
    public HandleUnzipProgress onUnzipProgressHandler;

    public static string LOCAL_RES_URL
    {
        get
        {
            if (!LGameConfig.GetInstance().isDebug && LGameConfig.GetInstance().isHotFix)
            {
                return LGameConfig.LOCAL_URL_PREFIX + Application.persistentDataPath + Path.DirectorySeparatorChar;
            }
            else {
#if UNITY_STANDALONE_WIN
				return "file://" + Application.dataPath + "/StreamingAssets/";  
#elif UNITY_ANDROID
                    return Application.streamingAssetsPath + Path.DirectorySeparatorChar;
#elif UNITY_IPHONE
                    return "file://"+Application.streamingAssetsPath+ Path.DirectorySeparatorChar;  
#else
                    return string.Empty;  
#endif
            }
        }
    }

    public static string LOCAL_RES_PATH
    {
        get
        {
            return Application.persistentDataPath + Path.DirectorySeparatorChar;
        }
    }

    public void checkUpdate()
    {
        Debug.Log("开始热更");
        //初始化  
        LocalResVersion = new Dictionary<string, string>();
        ServerResVersion = new Dictionary<string, string>();
        NeedDownFiles = new List<string>();

        Debug.Log("客户端ver:" + LOCAL_RES_URL + VERSION_FILE);
        //加载本地version配置  
        StartCoroutine(DownLoad(LOCAL_RES_URL + VERSION_FILE, delegate (WWW localVersion)
        {
            //保存本地的version  
            ParseVersionFile(localVersion.text, LocalResVersion);
            Debug.Log("服务端ver:" + LGameConfig.GetInstance().SERVER_RES_URL + Path.DirectorySeparatorChar + VERSION_FILE);
            //加载服务端version配置  
            StartCoroutine(this.DownLoad(LGameConfig.GetInstance().SERVER_RES_URL + Path.DirectorySeparatorChar + VERSION_FILE, delegate (WWW serverVersion)
            {
                //保存服务端version  
                ParseVersionFile(serverVersion.text, ServerResVersion);
                //计算出需要重新加载的资源  
                CompareVersion();
                //加载需要更新的资源  
                DownLoadRes();
            }));
        }));
    }

    //依次加载需要更新的资源  
    private void DownLoadRes()
    {
        if (NeedDownFiles.Count == 0)
        {
            UpdateLocalVersionFile();
            return;
        }

        string file = NeedDownFiles[0];
        NeedDownFiles.RemoveAt(0);

        StartCoroutine(this.DownLoad(LGameConfig.GetInstance().SERVER_RES_URL + Path.DirectorySeparatorChar + file, delegate (WWW w)
        {
            //将下载的资源替换本地就的资源  
            ReplaceLocalRes(file, w.bytes,()=> {
                DownLoadRes();
            });
        }));
    }

    private void ReplaceLocalRes(string fileName, byte[] data,UnityAction onComplete)
    {
        string filePath = LOCAL_RES_PATH + fileName;

        FileStream stream = new FileStream(filePath, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();

        //如果是更新包
        if (fileName == LGameConfig.UPDATE_FILE_ZIP)
        {
            //LUtil.UnpackFiles(filePath, LOCAL_RES_PATH);
            StartCoroutine(UnpackFiles(filePath, LOCAL_RES_PATH,() =>
            {
                File.Delete(filePath);
                onComplete.Invoke();
            }));
        }
        else
            onComplete.Invoke();

    }

    public IEnumerator UnpackFiles(string file, string dir, UnityAction onComplete)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        ZipInputStream s = new ZipInputStream(File.OpenRead(file));

        ZipEntry theEntry;
        while ((theEntry = s.GetNextEntry()) != null)
        {
            string directoryName = Path.GetDirectoryName(theEntry.Name);
            string fileName = Path.GetFileName(theEntry.Name);

            if (directoryName != string.Empty)
                Directory.CreateDirectory(dir + directoryName);

            if (fileName != string.Empty)
            {
                FileStream streamWriter = File.Create(dir + theEntry.Name);

                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = s.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        streamWriter.Write(data, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                doneCount++;
                if( onUnzipProgressHandler != null)
                    onUnzipProgressHandler.Invoke(doneCount);

                streamWriter.Close();
                yield return new WaitForEndOfFrame();
            }
        }
        onComplete.Invoke();
        try {
            s.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(LUtil.FormatException(e));
        }
    }

    //显示资源
    private /*IEnumerator*/ void Complate()
    {
        //using (WWW asset = new WWW(LOCAL_RES_URL + "newRes.assetbundle"))
        //{
        //    yield return asset;

        //    string code = asset.assetBundle.LoadAsset<TextAsset>("newLua").text;
        //    LuaSvr l = Game.GetInstance().getLuaSvr();
        //    l.luaState.doString(code);
        //    asset.Dispose();
        //}

        //using (WWW scene = new WWW(LOCAL_RES_URL + "newScene.unity3d"))
        //{
        //    yield return scene;
        //    AssetBundle b = scene.assetBundle; //不要注释这句!!!不然加载不了场景（坑到爆炸
        //    SceneManager.LoadScene("myScene");
        //    scene.Dispose();
        //}

        if (onCompleteHandler != null)
        {
            onCompleteHandler.Invoke();
        }
        Debug.Log("热更完成");
    }

    //更新本地的version配置  
    private void UpdateLocalVersionFile()
    {
        if (NeedUpdateLocalVersionFile)
        {
            StringBuilder versions = new StringBuilder();
            var e = ServerResVersion.GetEnumerator();
            while (e.MoveNext())
            {
                versions.Append(e.Current.Key).Append(",").Append(e.Current.Value).Append("\n");
            }

            FileStream stream = new FileStream(LOCAL_RES_PATH + VERSION_FILE, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();

            Debug.Log("更新版本号");
        }
        //加载显示对象  
        //StartCoroutine(Complate());
        Complate();
    }

    private void CompareVersion()
    {
        foreach (var version in ServerResVersion)
        {
            string fileName = version.Key;
            string serverMd5 = version.Value;

            //新增的资源  
            if (!LocalResVersion.ContainsKey(fileName))
            {
                Debug.Log("需更新：" + fileName);
                NeedDownFiles.Add(fileName);
            }
            else
            {
                //需要替换的资源  
                string localMd5;
                LocalResVersion.TryGetValue(fileName, out localMd5);
                if (!serverMd5.Equals(localMd5))
                {
                    Debug.Log("需更新：" + fileName);
                    NeedDownFiles.Add(fileName);
                }
            }
        }

        //本次有更新，同时更新本地的version.ver  
        NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
    }

    private void ParseVersionFile(string content, Dictionary<string, string> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        string[] items = content.Split(new char[] { '\n' });
		int itemsLen = items.Length;
        for(int i=0;i < itemsLen; i++ )
        {
            string[] info = items[i].Split(new char[] { ',' });
            if (info != null && info.Length == 2)
            {
                dict.Add(info[0], info[1]);
            }
        }

    }

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }

    public delegate void HandleFinishDownload(WWW www);
    public delegate void HandleUnzipProgress(int step); 
}