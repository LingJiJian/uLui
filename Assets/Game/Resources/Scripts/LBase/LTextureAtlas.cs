using UnityEngine;
using SLua;

using System.Collections.Generic;

/// <summary>
/// 子图信息 
/// </summary>
public class FrameInfo
{
    public string name;
    public Rect rect;
    public Vector2 vec2;

    public FrameInfo(string[] str)
    {
        if (str != null)
        {
            this.name = str[0];
            float x, y, w, h;
            x = float.Parse(str[1]);
            y = float.Parse(str[2]);
            w = float.Parse(str[3]);
            h = float.Parse(str[4]);
            this.rect = new Rect(x, y, w, h);
            float pivotX, pivotY;
            pivotX = float.Parse(str[5]);
            pivotY = float.Parse(str[6]);
            this.vec2 = new Vector2(pivotX, pivotY);
        }
    }
}

/// <summary>
/// 图集 
/// </summary>
[CustomLuaClass]
public class LTextureAtlas {

    private static LTextureAtlas _instance;
    
    private Dictionary<string, List<FrameInfo>> _atlasData;
    private Dictionary<string, Texture2D> _atlasTexture;
    private Dictionary<string, Sprite> _sprites;

    public static LTextureAtlas GetInstance()
    {
        if(_instance == null)
        {
            _instance = new LTextureAtlas();
        }
        return _instance;
    }

    public LTextureAtlas()
    {
        _atlasData = new Dictionary<string, List<FrameInfo>>();
        _atlasTexture = new Dictionary<string, Texture2D>();
        _sprites = new Dictionary<string, Sprite>();
    }

    public void LoadData(string atlasName)
    {
        string atlasTxt;
        Texture2D tex2d = LLoadBundle.GetInstance().LoadAtlas(atlasName, out atlasTxt).texture;
        string key = atlasName;
        if (_atlasTexture.ContainsKey(key)) return;

        _atlasTexture.Add(key, tex2d);

        string[] lineArray = atlasTxt.Split(new char[] { '\n' });

        List<FrameInfo> frameInfos = new List<FrameInfo>();
        for (int i = 0; i < lineArray.Length; i++)
        {
            if((lineArray[i].IndexOf('#') == -1 ) && (lineArray[i].IndexOf(':') == -1))
            {
                lineArray[i] = lineArray[i].Replace("\r", "");
                if((lineArray[i] != ""))
                {
                    string[] strArray = lineArray[i].Split(new char[] { ';' });
                    FrameInfo frameInfo = new FrameInfo(strArray);
                    frameInfos.Add(frameInfo);
                    Sprite sp = Sprite.Create(tex2d, frameInfo.rect, frameInfo.vec2);
                    sp.name = frameInfo.name;
                    _sprites.Add(atlasName +"_"+ frameInfo.name, sp);
                }
            }
        }
        _atlasData.Add(key, frameInfos);
    }

    public Sprite getSprite(string atlasName, string name)
    {
        if(name.IndexOf('.') != -1)
            name = name.Split('.')[0];

        Sprite sp;
        _sprites.TryGetValue(atlasName + "_" + name, out sp);
        return sp;
    }

    public Sprite[] getSprites(string atlasName)
    {
        List<Sprite> sps = new List<Sprite>();
        List<FrameInfo> list = _atlasData[atlasName];
        for (int i = 0; i < list.Count; i++)
        {
            sps.Add(_sprites[atlasName + "_" + list[i].name]);
        }
        return sps.ToArray();
    }

    public void RemoveTexture(string atlasName)
    {
        string key = atlasName;
        if (_atlasData.ContainsKey(key))
        {
            LLoadBundle.GetInstance().UnloadBundles(new string[] { atlasName });

            List<FrameInfo> list = _atlasData[key];
            for(int i = 0; i < list.Count; i++)
            {
                _sprites.Remove(atlasName + "_" + list[i].name);
            }
            _atlasData.Remove(key);
            _atlasTexture.Remove(key);
        }
    }
}
