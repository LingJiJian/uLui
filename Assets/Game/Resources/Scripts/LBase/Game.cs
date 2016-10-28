
using System.IO;
using UnityEngine;
using LuaInterface;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    private static LuaState _l;

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
            _l = new LuaState();
            if (!LGameConfig.GetInstance().isDebug)
                LuaState.loaderDelegate = loadLuaWithAb;
            else
                _l.AddSearchPath(Application.streamingAssetsPath + "/" + LGameConfig.DATA_CATAGORY_LUA);
            LuaBinder.Bind(_l);
            complete();
        }
        else
        {
            complete();
        }
    }

    public static LuaState GetLuaSvr()
    {
        return _l;
    }

    void log(string cond, string trace, LogType lt)
    {
        Debug.Log(cond);
    }

    protected byte[] loadLuaWithAb(string strFile)
    {
        string ext = LGameConfig.GetInstance().isEncrypt ? ".bytes" : ".txt";
        TextAsset asset = LLoadBundle.GetInstance ().LoadAsset<TextAsset>("@lua.ab", LGameConfig.DATA_CATAGORY_LUA+"/" + Path.GetFileNameWithoutExtension(strFile) + ext);
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
                        _l.Start();
                        _l.DoFile("main.lua");
                    });
                };
				resUpdate.checkUpdate ();
			} else {
				LLoadBundle.GetInstance ().LoadAllBundles (new string[] { "@lua.ab" },()=>
                {
                    _l.Start();
                    _l.DoFile("main.lua");
				});
			}
        }
        else //PC端开发
        {
            _l.Start();
            _l.DoFile("main.lua");
        }
    }
}
