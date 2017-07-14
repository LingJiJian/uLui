using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;

public class LLoadBundle : MonoBehaviour
{
    private Dictionary<string, AssetBundle> bundles;
    private static LLoadBundle _instance;
    private Dictionary<string, Sprite[]> spritesCache;
    private Dictionary<string, Object> tplCache;

    public AssetBundle GetBundleByName(string name)
    {
        AssetBundle b;
        bundles.TryGetValue(name, out b);
        return b;
    }

    private LLoadBundle()
    {
        bundles = new Dictionary<string, AssetBundle>();
        spritesCache = new Dictionary<string, Sprite[]>();
        tplCache = new Dictionary<string , Object>();
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

    public void LoadAllBundles(string[] bundle_names, UnityAction callFunc,UnityAction<int> progressFunc=null)
    {
        if (LGameConfig.GetInstance().isDebug)
        {
            callFunc.Invoke();
        }
        else
        {
            StartCoroutine(Load(bundle_names, callFunc,progressFunc));
        }
    }

    IEnumerator Load(string[] bundle_names, UnityAction callFunc,UnityAction<int> progressFunc=null)
    {
        int len = bundle_names.Length;

        for (int i = 0; i < len; i++)
        {
            string name = bundle_names[i];
            if (!bundles.ContainsKey(name))
            {
                using (WWW asset = new WWW(LResUpdate.LOCAL_RES_URL + name))
                {
                    Debug.Log("bundle name:"+name);
                    yield return asset;

                    if (string.IsNullOrEmpty (asset.error)) {

                        bundles.Add(name, asset.assetBundle);
                        asset.Dispose();
                        if(progressFunc!=null)
                            progressFunc.Invoke(i+1);
                    }else{
                         Debug.Log("bundle error:"+name+" "+asset.error);
                    }
                }
            }

			if (i == len - 1)
			{
				callFunc();
			}
        }
    }

    public Object LoadAsset(string bundleName, string assetName)
    {   
        string key = string.Format("{0}_{1}",bundleName,assetName);
        if(tplCache.ContainsKey(key)){
            return tplCache[key];
        }else{
            Object tpl = LoadAsset<Object>(bundleName, assetName);
            if( tpl == null){
                Debug.LogWarning("asset not exist! "+bundleName+" "+assetName);
                return null;
            }
            tplCache.Add(key,tpl);
        }
        return tplCache[key];
    }

    public Object[] LoadAllAsset(string bundleName, string assetName)
    {
        return LoadAllAsset<Object>(bundleName, assetName);
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

    public Sprite[] GetSpritesByName(string bundlePath,string assetName)
    {   
		string key = bundlePath;
        if(spritesCache.ContainsKey(key))
        {
            return spritesCache[key];
        }else{

            if (LGameConfig.GetInstance().isDebug)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(bundlePath);
				spritesCache.Add(key,sprites);
            }
            else
            {
                string bundleName = LGameConfig.GetABNameWithAtlasPath(bundlePath.Split('.')[0] +".png");
                AssetBundle assetBundle = this.GetBundleByName(bundleName);
                if (assetBundle)
                {
                    Sprite[] sprites = assetBundle.LoadAllAssets<Sprite>();
					spritesCache.Add(key,sprites);
                }
            }

			List<Sprite> _arr = new List<Sprite>();
			Sprite[] _sprites = spritesCache[key];
			foreach (Sprite s in _sprites) {
				if (s.name.StartsWith(assetName))
				{
					_arr.Add(s);
				}
			}

			return _arr.ToArray();
        }
    }

    public Sprite GetSpriteByName(string bundlePath, string assetName)
    {
        Sprite[] sprites = GetSpritesByName(bundlePath, assetName);
        return sprites.Length > 0 ? sprites[0] : null;
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
