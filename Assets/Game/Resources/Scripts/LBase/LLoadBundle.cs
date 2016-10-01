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
        AssetBundle b;
        bundles.TryGetValue(name, out b);
        return b;
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

    public Sprite[] GetSpritesByName(string bundlePath,string atlasName)
    {
        List<Sprite> arr = new List<Sprite>();
        if (LGameConfig.GetInstance().isDebug)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(bundlePath);
            foreach (Sprite s in sprites)
            {
                if (string.IsNullOrEmpty(atlasName) || s.name.StartsWith(atlasName))
                {
                    arr.Add(s);
                }
            }
        }
        else
        {
            string bundleName = LGameConfig.GetABNameWithAtlasPath(bundlePath.Split('.')[0] +".png");
            AssetBundle assetBundle = this.GetBundleByName(bundleName);
            if (assetBundle)
            {
                Sprite[] sprites = assetBundle.LoadAllAssets<Sprite>();
                foreach (Sprite s in sprites)
                {
                    if (string.IsNullOrEmpty(atlasName) || s.name.StartsWith(atlasName))
                    {
                        arr.Add(s);
                    }
                }
            }
        }
        return arr.ToArray();
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
