using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using SLua;

[CustomLuaClassAttribute]
public class LLoadBundle : MonoBehaviour {

    private Dictionary<string,AssetBundle> bundles;
    private static LLoadBundle _instance;

    public AssetBundle GetBundleByName(string name)
    {
        return bundles[name];
    }

    private LLoadBundle()
    {
        bundles = new Dictionary<string, AssetBundle>();
    }

    public static LLoadBundle GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            DontDestroyOnLoad(obj);
            obj.name = "LLoadBundle";
            _instance = obj.AddComponent<LLoadBundle>();
        }
        return _instance;
    }

    public void LoadAllBundles(string[] bundle_names, UnityAction callFunc)
    {
        if (LGameConfig.GetInstance().isDebug) {
            callFunc.Invoke();
        }else{
            StartCoroutine(Load(bundle_names, callFunc));
        }
    }

    IEnumerator Load(string[] bundle_names,UnityAction callFunc)
    {
        int len = bundle_names.Length;

        for(int i = 0; i< len; i++)
        {
            string name = bundle_names[i];
            using (WWW asset = new WWW(LResUpdate.LOCAL_RES_URL + "/" + name))
            {
                yield return asset;

                bundles.Add(name,asset.assetBundle);
                asset.Dispose();

                if (i == len-1)
                {
                    callFunc();
                }
            }
        }
    }

    public Object LoadAsset(string bundleName, string assetName,System.Type assetType)
    {
        Object prefab = null;
        if (LGameConfig.GetInstance().isDebug)
        {
            prefab = Resources.Load(string.Format("Prefabs/{0}",assetName), assetType);
        }
        else
        {
            AssetBundle b = bundles[bundleName];
            if (b != null)
            {
                prefab = b.LoadAsset(string.Format(LGameConfig.ASSETBUNDLE_LOAD_FORMAT, assetName.ToLower()), assetType);
            }
        }
        return prefab;
    }

    public T[] LoadAllAsset<T>(string bundleName, string assetName) where T:Object
    {
        T[] prefabs = null;
        if (LGameConfig.GetInstance().isDebug)
        {
            prefabs = Resources.LoadAll<T>(string.Format("Prefabs/{0}", assetName));
        }
        else
        {
            AssetBundle b = bundles[bundleName];
            if (b != null)
            {
                prefabs = b.LoadAllAssets<T>();
            }
        }
        return prefabs;
    }
}
