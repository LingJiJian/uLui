using UnityEngine;
using System.IO;
using System.Xml;
using SLua;

[CustomLuaClassAttribute]
public class LGameConfig
{
    // The config file path.
    public static readonly string CONFIG_FILE = "config";
    // The lua data folder name.
    public static readonly string DATA_CATAGORY_LUA = "@Lua";

	public static readonly string ASSETBUNDLE_PATH = "Ab/";
	public static readonly string ASSETBUNDLE_AFFIX = ".ab";
    // The lua file affix.
    public static readonly string FILE_AFFIX_LUA = ".lua";
    // The lua files zip name.
    public static readonly string UPDATE_FILE_ZIP = "data.zip";
    // asset load base format
	public static readonly string ASSET_BASE_FORMAT = "Assets/Game/Resources/{0}";
    // 32 bytes encrypt key
    public static string EncryptKey32 = "12345678901234567890123456789012";
    // 16 bytes encrypt key
    public static string EncryptKey16 = "1234567890123456";
    // is activate debug
    public bool isDebug = true;
    // is pack lua files in app
    public bool isHotFix = true;
    // is show frame rate
    public bool isShowFps = true;
    // is use luajit & encode
    public bool isEncrypt = true;
    // remote server resource url
    public string SERVER_RES_URL = "";
    // game default target frame rate
    public static int DEFAULT_FRAME_RATE = 60;

    // The local file url prefix. (For assetbundle.)
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    public static readonly string LOCAL_URL_PREFIX = "file:///";
#else
	public static readonly string LOCAL_URL_PREFIX = "file://";
#endif

    // The asset path in persistent asset path.
    private string m_strPersistAssetPath = string.Empty;

    // The asset path in streaming asset path.
    private string m_strStreamAssetPath = string.Empty;

    // The asset path in caching path.
    private string m_strCachingAssetPath = string.Empty;

    // The global instance.
    private static LGameConfig m_cInstance = null;

    /**
     * Constructor.
     * 
     * @param void.
     * @return void.
     */
    private LGameConfig()
    {
        LoadConfig();
    }

    /**
     * Destructor.
     * 
     * @param void.
     * @return void.
     */
    ~LGameConfig()
    {
        m_cInstance = null;
    }

    public static LGameConfig GetInstance()
    {
        if (null == m_cInstance)
        {
            m_cInstance = new LGameConfig();
        }
        return m_cInstance;
    }

    // Get persistent assets path.
    public string PersistentAssetsPath
    {
        get
        {
            if (string.IsNullOrEmpty(m_strPersistAssetPath))
            {
                m_strPersistAssetPath = Application.persistentDataPath + Path.DirectorySeparatorChar;
            }

            return m_strPersistAssetPath;
        }
    }

    // Get streaming assets path.
    public string StreamingAssetsPath
    {
        get
        {
            if (string.IsNullOrEmpty(m_strStreamAssetPath))
            {
                m_strStreamAssetPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar;
            }

            return m_strStreamAssetPath;
        }
    }

    // Get caching assets path.
    public string CachingAssetsPath
    {
        get
        {
            if (string.IsNullOrEmpty(m_strCachingAssetPath))
            {
                m_strCachingAssetPath = Application.temporaryCachePath + Path.DirectorySeparatorChar;
            }

            return m_strCachingAssetPath;
        }
    }

    /**
     * Get the final load url.
     * 
     * @param string strPathName - The path name of the file with dir except the base url.
     * @return string - The final full url load string.
     */
    public string GetLoadUrl(string strPathName)
    {
        string strFilePath = PersistentAssetsPath + strPathName;

        if (File.Exists(strFilePath) && (!this.isDebug))
        {
            return strFilePath;
        }
        else
        {
            strFilePath = StreamingAssetsPath + strPathName;
            return strFilePath;
        }
    }

    /**
     * Get the final load url for directory.
     * 
     * @param string strPathName - The path dir name of the file with dir except the base url.
     * @return string - The final full url load string for the path dir.
     */
    public string GetLoadUrlForDir(string strPathName)
    {
        string strFilePath = PersistentAssetsPath + strPathName;
        if (Directory.Exists(strFilePath))
        {
            return strFilePath;
        }
        else
        {
            strFilePath = StreamingAssetsPath + strPathName;
            return strFilePath;
        }
    }

	public static string GetABNameWithAtlasPath(string path){
		return string.Format("{0}{1}", path.Replace ('/', '-').Replace ('.', '_').ToLower(),ASSETBUNDLE_AFFIX);
	}

    private void LoadConfig()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(CONFIG_FILE);
        if (textAsset)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(textAsset.text);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  

            XmlNodeList debugs = rootElem.GetElementsByTagName("Debug");
            isDebug = debugs[0].InnerText == "1";

            XmlNodeList resUrls = rootElem.GetElementsByTagName("ResUrl");
            SERVER_RES_URL = resUrls[0].InnerText;

			XmlNodeList hotFix = rootElem.GetElementsByTagName("HotFix");
			isHotFix = hotFix[0].InnerText == "1";

            XmlNodeList showFps = rootElem.GetElementsByTagName("ShowFps");
            isShowFps = showFps[0].InnerText == "1";

            XmlNodeList encrypt = rootElem.GetElementsByTagName("Encrypt");
            isEncrypt = encrypt[0].InnerText == "1";
            
        }
    }
}
