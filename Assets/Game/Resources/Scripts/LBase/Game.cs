using SLua;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[CustomLuaClassAttribute]
public class Game : MonoBehaviour
{
    private static LuaSvr _l;

    public UnityAction<int> onProgressHandler;

    void Start()
    {
        Application.targetFrameRate = LGameConfig.DEFAULT_FRAME_RATE;

        if (_l == null)
        {
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

        string dir = strFile.StartsWith("Config") ? LGameConfig.CONFIG_CATAGORY_LUA : LGameConfig.DATA_CATAGORY_LUA;
        string strLuaPath = dir + Path.DirectorySeparatorChar + strFile;
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
        TextAsset asset = null;
        if (strFile.StartsWith("Config"))
        {
            asset = LLoadBundle.GetInstance().LoadAsset<TextAsset>("@luaconfig.ab", "@LuaConfig/" + strFile + ext);
        }
        else
        {
            asset = LLoadBundle.GetInstance().LoadAsset<TextAsset>("@lua.ab", "@Lua/" + strFile + ext);
        }
        if (asset == null) return null;
        //if (LGameConfig.GetInstance().isEncrypt)
        //    return LUtil.AESDecrypt(asset.bytes, LGameConfig.EncryptKey32, LGameConfig.EncryptKey16);
//        else
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
                GameObject canvas = GameObject.Find("Canvas");
                Text lab_unzip = null;
                if (canvas.transform.Find("prog"))
                {
                    canvas.transform.Find("prog").gameObject.SetActive(true);
                    lab_unzip = canvas.transform.Find("prog/lab_unzip").GetComponent<Text>();
                }
                
				GameObject obj = new GameObject ();
				obj.name = "ResUpdate";
				LResUpdate resUpdate = obj.AddComponent<LResUpdate> ();
                resUpdate.onUnzipProgressHandler = (int step) =>{
                    if(lab_unzip)
                        lab_unzip.text = step.ToString();
                    Debug.Log(" unzip "+step);
                };
				resUpdate.onCompleteHandler = () => {
					Destroy (obj);
                    LLoadBundle.GetInstance().LoadAllBundles(new string[] { "@lua.ab","@luaconfig.ab" }, () =>
                    {
                        _l.start("main");
                    });
                };
				resUpdate.checkUpdate ();
			} else {
				LLoadBundle.GetInstance ().LoadAllBundles (new string[] { "@lua.ab","@luaconfig.ab" },()=>
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
