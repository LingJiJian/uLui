using SLua;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CustomLuaClassAttribute]
public class Game : MonoBehaviour
{
    private static LuaSvr _l;

    public UnityAction<int> onProgressHandler;

    void Start()
    {
        Application.targetFrameRate = LGameConfig.DEFAULT_FRAME_RATE;

        if(_l == null)
        {
#if UNITY_5
        Application.logMessageReceived += this.log;
#else
		Application.RegisterLogCallback(this.log);
#endif
            if (LGameConfig.GetInstance().isPackLua)
            {
                LuaState.loaderDelegate = loadLuaFileDebug;
            }else
            {
                LuaState.loaderDelegate = loadFileWithSuffix;
            }

            _l = new LuaSvr();
            _l.init(tick, complete);
        }
        else
        {
            complete();
        }
    }

    public static LuaSvr GetLuaSvr()
    {
        return _l;
    }

    void log(string cond, string trace, LogType lt)
    {
        Debug.Log(cond);
    }

    void tick(int p)
    {
        if (onProgressHandler != null)
        {
            onProgressHandler.Invoke(p);
        }
    }

    protected byte[] loadFileWithSuffix(string strFile)
    {
        if (string.IsNullOrEmpty(strFile))
        {
            return null;
        }

        strFile.Replace(".", "/");
        strFile += LGameConfig.FILE_AFFIX_LUA;

        string strLuaPath = LGameConfig.DATA_CATAGORY_LUA + Path.DirectorySeparatorChar + strFile;
        string strFullPath = LGameConfig.GetInstance().GetLoadUrl(strLuaPath);
        // Read from file.
        LArchiveBinFile cArc = new LArchiveBinFile();
        if (!cArc.Open(strFullPath, FileMode.Open, FileAccess.Read))
        {
            return null;
        }

        if (!cArc.IsValid())
        {
            return null;
        }

        int nContentLength = (int)cArc.GetStream().Length;
        byte[] aContents = new byte[nContentLength];
        cArc.ReadBuffer(ref aContents, nContentLength);
        cArc.Close();

        return aContents;
    }

    protected byte[] loadLuaFileDebug(string strFile)
    {
        byte[] bytes;
        TextAsset asset = (TextAsset)Resources.Load(LGameConfig.DATA_CATAGORY_LUA + "/" + strFile);
        if (asset == null)
            return null;
        bytes = asset.bytes;
        return bytes;
    }

    void complete()
    {
        if (LGameConfig.GetInstance().isShowFps)
        {
            LFPSView.Show();
        }

        if (!LGameConfig.GetInstance().isDebug) //生产环境
        {
            GameObject obj = new GameObject();
            obj.name = "ResUpdate";
            LResUpdate resUpdate = obj.AddComponent<LResUpdate>();
            resUpdate.onCompleteHandler = () =>
            {
                Destroy(obj);
                _l.start("main");
            };
            resUpdate.checkUpdate();

        }
        else //PC端开发
        {
            _l.start("main");
        }
    }
}
