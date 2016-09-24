using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;
using SLua;

[CustomLuaClass]
public class LLoadBundle : MonoBehaviour
{
    private Dictionary<string, AssetBundle> bundles;
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
        if (LGameConfig.GetInstance().isDebug)
        {
            callFunc.Invoke();
        }
        else
        {
            StartCoroutine(Load(bundle_names, callFunc));
        }
    }

    IEnumerator Load(string[] bundle_names, UnityAction callFunc)
    {
        int len = bundle_names.Length;

        for (int i = 0; i < len; i++)
        {
            string name = bundle_names[i];
            if (!bundles.ContainsKey(name))
            {
                using (WWW asset = new WWW(LResUpdate.LOCAL_RES_URL + name))
                {
                    yield return asset;

                    bundles.Add(name, asset.assetBundle);
                    asset.Dispose();
                }
            }

			if (i == len - 1)
			{
				callFunc();
			}
        }
    }

    public Sprite LoadAtlas(string atlasName,out string txt)
    {
        Debug.Log("LoadAtlas:" + atlasName);
        txt = "";
        Sprite atlas = null;
        if (LGameConfig.GetInstance().isDebug)
        {
			atlas = Resources.Load<Sprite>(atlasName);
			txt = Resources.Load<TextAsset>(atlasName).text;
        }
        else
        {
            AssetBundle b;
			bundles.TryGetValue(LGameConfig.GetABNameWithAtlasPath(atlasName) , out b);
            if (b != null)
            {
				Debug.Log(string.Format(LGameConfig.ASSET_BASE_FORMAT, atlasName));
				atlas = b.LoadAsset<Sprite>(string.Format(LGameConfig.ASSET_BASE_FORMAT, atlasName)+".png");
				txt = b.LoadAsset<TextAsset>(string.Format(LGameConfig.ASSET_BASE_FORMAT, atlasName)+".txt").text;
            }
        }
        return atlas;
    }

	public T LoadAsset<T>(string bundleName, string assetName) where T : Object 
    {
        T prefab = null;
        if (LGameConfig.GetInstance().isDebug)
        {
            assetName = assetName.Split('.')[0];
			prefab = Resources.Load<T>(assetName);
        }
        else
        {
            AssetBundle b;
            bundles.TryGetValue(bundleName, out b);
            if (b != null)
            {
//				Debug.Log(string.Format(LGameConfig.ASSET_BASE_FORMAT, assetName));
				prefab = b.LoadAsset<T>(string.Format(LGameConfig.ASSET_BASE_FORMAT, assetName));
            }else{
                Debug.Log("bundle not exist! : "+bundleName);
            }
        }
        return prefab;
    }

    public T[] LoadAllAsset<T>(string bundleName, string assetName) where T : Object
    {
        T[] prefabs = null;
        if (LGameConfig.GetInstance().isDebug)
        {
            assetName = assetName.Split('.')[0];
            prefabs = Resources.LoadAll<T>(assetName);
        }
        else
        {
            AssetBundle b;
            bundles.TryGetValue(bundleName, out b);
            if (b != null)
            {
                prefabs = b.LoadAllAssets<T>();
            }
        }
        return prefabs;
    }

    public void UnloadBundles(string[] bundle_names)
    {
        for (int i = 0; i < bundle_names.Length; i++)
        {
            AssetBundle b;
            bundles.TryGetValue(bundle_names[i], out b);
            if (b != null)
            {
				b.Unload(true);
                bundles.Remove(bundle_names[i]);
            }
        }
    }
}
