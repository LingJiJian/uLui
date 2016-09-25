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
			if (LGameConfig.GetInstance().isDebug)
            {
				LuaState.loaderDelegate = loadFileWithSuffix;
            }else
            {
				LuaState.loaderDelegate = loadLuaWithAb;
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

    protected byte[] loadLuaWithAb(string strFile)
    {
        string ext = LGameConfig.GetInstance().isEncrypt ? ".bytes" : ".txt";
        TextAsset asset = LLoadBundle.GetInstance ().LoadAsset<TextAsset>("@lua.ab", "@Lua/" + strFile + ext);
        if (asset == null) return null;
        //if (LGameConfig.GetInstance().isEncrypt)
        //    return LUtil.AESDecrypt(asset.bytes, LGameConfig.EncryptKey32, LGameConfig.EncryptKey16);
        //else
            return asset.bytes;
    }

    void complete()
    {
        if (LGameConfig.GetInstance().isShowFps)
        {
            LFPSView.Show();
        }

        if (!LGameConfig.GetInstance().isDebug) //生产环境
        {
			if (LGameConfig.GetInstance ().isHotFix) {
				GameObject obj = new GameObject ();
				obj.name = "ResUpdate";
				LResUpdate resUpdate = obj.AddComponent<LResUpdate> ();
				resUpdate.onCompleteHandler = () => {
					Destroy (obj);
                    LLoadBundle.GetInstance().LoadAllBundles(new string[] { "@lua.ab" }, () =>
                    {
                        _l.start("main");
                    });
                };
				resUpdate.checkUpdate ();
			} else {
				LLoadBundle.GetInstance ().LoadAllBundles (new string[] { "@lua.ab" },()=>
                {
					_l.start ("main");
				});
			}
        }
        else //PC端开发
        {
            _l.start("main");
        }
    }
}
