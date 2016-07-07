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

	public void LoadData(string bundleName, string atlasName)
    {
        Texture2D tex2d = LLoadBundle.GetInstance().LoadAsset(bundleName, atlasName, typeof(Texture2D)) as Texture2D;
        string key = string.Format("{0}_{1}", bundleName, atlasName);
        if (_atlasTexture.ContainsKey(key)) return;

        _atlasTexture.Add(key, tex2d);

        TextAsset text = LLoadBundle.GetInstance().LoadAsset(bundleName, atlasName, typeof(TextAsset)) as TextAsset;
        string[] lineArray = text.text.Split(new char[] { '\n' });

        List<FrameInfo> frameInfos = new List<FrameInfo>();
        for (int i = 0; i < lineArray.Length; i++)
        {
            if((lineArray[i][0] != '#') && (lineArray[i][0] != ':'))
            {
                lineArray[i] = lineArray[i].Replace("\r", "");
                if((lineArray[i] != ""))
                {
                    string[] strArray = lineArray[i].Split(new char[] { ';' });
                    FrameInfo frameInfo = new FrameInfo(strArray);
                    frameInfos.Add(frameInfo);
                    _sprites.Add(frameInfo.name, Sprite.Create(tex2d, frameInfo.rect, frameInfo.vec2));
                }
            }
        }
        _atlasData.Add(key, frameInfos);
    }

    public Sprite getSprite(string name)
    {
        Sprite sp;
        _sprites.TryGetValue(name, out sp);
        return sp;
    }
}
