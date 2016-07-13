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
    public static readonly string DATA_CATAGORY_LUA = "Lua";
    // The lua file affix.
    public static readonly string FILE_AFFIX_LUA = ".lua";
    // The lua files zip name.
    public static readonly string UPDATE_FILE_ZIP = "data.zip";
    // assetbundle load asset's format
    public static readonly string ASSETBUNDLE_LOAD_FORMAT = "Assets/Game/Resources/Prefabs/{0}";
    // assetbundle load atlas's format
    public static readonly string ASSETBUNDLE_ATLAS_FORMAT = "Assets/Game/Resources/Atlas/{0}";
    // game windows assetbundle's name
    public static readonly string PREFAB_BUNDLE = "prefabbundles";
    // is activate debug
    public bool isDebug = true;
    // is pack lua files in app
    public bool isPackLua = true;
    // is show frame rate
    public bool isShowFps = true;
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

            XmlNodeList packLua = rootElem.GetElementsByTagName("PackLua");
            isPackLua = isDebug ? packLua[0].InnerText == "1" : false;

            XmlNodeList showFps = rootElem.GetElementsByTagName("ShowFps");
            isShowFps = showFps[0].InnerText == "1";

        }
    }
}
