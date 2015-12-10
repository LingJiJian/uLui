using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;

namespace Lui
{

public enum Type
{
    TEXT,
    IMAGE,
    ANIM,
    NEWLINE,
}

public enum AlignType
{
    DESIGN_CENTER,
    REAL_CENTER,
    LEFT_TOP,
}

class LRichElement : Object {

    public Type type { get; protected set; }
    public Color color { get; protected set; }
    public string data { get; protected set; }
}
/// <summary>
/// 文本元素
/// </summary>
class LRichElementText : LRichElement
{
    public string txt { get; protected set; }
    public bool isUnderLine { get; protected set; }
    public bool isOutLine { get; protected set; }
    public int fontSize { get; protected set; }

    public LRichElementText(Color color, string txt, int fontSize, bool isUnderLine, bool isOutLine, string data)
    {
        this.type = Type.TEXT;
        this.color = color;
        this.txt = txt;
        this.fontSize = fontSize;
        this.isUnderLine = isUnderLine;
        this.isOutLine = isOutLine;
        this.data = data;
    }
}

/// <summary>
/// 图片元素
/// </summary>
class LRichElementImage : LRichElement
{
    public string path { get; protected set; }

    public LRichElementImage( string path, string data )
    {
        this.type = Type.IMAGE;
        this.path = path;
        this.data = data;
    }
}

/// <summary>
/// 动画元素
/// </summary>
class LRichElementAnim : LRichElement
{
    public string path { get; protected set; }
    public float fs { get; protected set; }

    public LRichElementAnim(string path,float fp, string data)
    {
        this.type = Type.ANIM;
        this.path = path;
        this.data = data;
        this.fs = fs;
    }
}

/// <summary>
/// 换行元素
/// </summary>
class LRichElementNewline : LRichElement
{
    public LRichElementNewline()
    {
        this.type = Type.NEWLINE;
    }
}

/// <summary>
/// 缓存结构
/// </summary>
class LRichCacheElement : Object
{
    public bool isUse;
    public GameObject node;
    public LRichCacheElement(GameObject node)
    {
        this.node = node;
    }
}

/// <summary>
/// 渲染结构
/// </summary>
struct LRenderElement
{
    public Type type;
    public string strChar;
    public int width;
    public int height;
    public bool isOutLine;
    public bool isUnderLine;
    public Font font;
    public int fontSize;
    public Color color;
    public string data;
    public string path;
    public float fs;
    public bool isNewLine;
    public Vector2 pos;

    public LRenderElement Clone()
    {
        LRenderElement cloneOjb;
        cloneOjb.type = this.type;
        cloneOjb.strChar = this.strChar;
        cloneOjb.width = this.width;
        cloneOjb.height = this.height;
        cloneOjb.isOutLine = this.isOutLine;
        cloneOjb.isUnderLine = this.isUnderLine;
        cloneOjb.font = this.font;
        cloneOjb.fontSize = this.fontSize;
        cloneOjb.color = this.color;
        cloneOjb.data = this.data;
        cloneOjb.path = this.path;
        cloneOjb.fs = this.fs;
        cloneOjb.isNewLine = this.isNewLine;
        cloneOjb.pos = this.pos;
        return cloneOjb;
    }
}

public class LRichText : MonoBehaviour, IRichTextClickableProtocol
{
	
	public AlignType alignType;
	public int verticalSpace;
	public int maxLineWidth;
	public Font font;
    public int realLineHeight { get; protected set; }
    public int realLineWidth { get; protected set; }

    List<LRichElement> richElements;
    List<LRenderElement> elemRenderArr;
    List<LRichCacheElement> cacheLabElements;
    List<LRichCacheElement> cacheImgElements;

    public void removeAllElements()
    {
        foreach(LRichCacheElement lab in cacheLabElements)
        {
            lab.isUse = false;
            lab.node.transform.SetParent(null);
        }
        foreach (LRichCacheElement img in cacheImgElements)
        {
            img.isUse = false;
            img.node.transform.SetParent(null);
        }
    }

    public void insertElement(string txt, Color color,int fontSize, bool isUnderLine, bool isOutLine, Color outLineColor, string data)
    {
        richElements.Add(new LRichElementText(color, txt,fontSize, isUnderLine, isOutLine, data));
    }

    public void insertElement(string path, float fp, string data)
    {
        richElements.Add(new LRichElementAnim(path, fp, data));
    }

    public void insertElement(string path, string data)
    {
        richElements.Add(new LRichElementImage(path, data));
    }

    public void insertElement(int newline)
    {
        richElements.Add(new LRichElementNewline());
    }

    public LRichText()
    {
        this.alignType = AlignType.LEFT_TOP;
        this.verticalSpace = 0;
        this.maxLineWidth = 300;

        richElements = new List<LRichElement>();
        elemRenderArr = new List<LRenderElement>();
        cacheLabElements = new List<LRichCacheElement>();
        cacheImgElements = new List<LRichCacheElement>();

		
    }

    public void reloadData()
    {
        this.removeAllElements();

        foreach (LRichElement elem in richElements)
        {
            if (elem.type == Type.TEXT)
            {
                LRichElementText elemText = elem as LRichElementText;
                char[] _charArr = elemText.txt.ToCharArray();
                TextGenerator gen = new TextGenerator();

                foreach (char strChar in _charArr)
                {
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = Type.TEXT;
                    rendElem.strChar = strChar.ToString();
                    rendElem.isOutLine = elemText.isOutLine;
                    rendElem.isUnderLine = elemText.isUnderLine;
                    rendElem.font = this.font;
                    rendElem.fontSize = elemText.fontSize;
                    rendElem.data = elemText.data;
                    rendElem.color = elemText.color;

                    TextGenerationSettings setting = new TextGenerationSettings();
                    setting.font = this.font;
                    setting.fontSize = elemText.fontSize;
                    setting.lineSpacing = 1;
                    setting.scaleFactor = 1;
                    rendElem.width = (int)gen.GetPreferredWidth(rendElem.strChar, setting);
                    rendElem.height = (int)gen.GetPreferredHeight(rendElem.strChar, setting);
                    elemRenderArr.Add(rendElem);
                }
            }
            else if (elem.type == Type.IMAGE)
            {
                LRichElementImage elemImg = elem as LRichElementImage;
                LRenderElement rendElem = new LRenderElement();
                rendElem.type = Type.IMAGE;
                rendElem.path = elemImg.path;
                rendElem.data = elemImg.data;

                Texture tex = Resources.Load(rendElem.path) as Texture;
                rendElem.width = tex.width;
                rendElem.height = tex.height;
                elemRenderArr.Add(rendElem);
            }
            else if (elem.type == Type.ANIM)
            {
                LRichElementAnim elemAnim = elem as LRichElementAnim;
                LRenderElement rendElem = new LRenderElement();
                rendElem.type = Type.ANIM;
                rendElem.path = elemAnim.path;
                rendElem.data = elemAnim.data;
                rendElem.fs = elemAnim.fs;
                rendElem.data = elemAnim.data;
           
                Texture tex = Resources.Load(rendElem.path+"1.png") as Texture;
                rendElem.width = tex.width;
                rendElem.height = tex.height;
                elemRenderArr.Add(rendElem);
            }
            else if (elem.type == Type.NEWLINE)
            {
                LRenderElement rendElem = new LRenderElement();
                rendElem.isNewLine = true;
                elemRenderArr.Add(rendElem);
            }
        }

        formarRenderers();
    }

    protected void formarRenderers()
    {
        int oneLine = 0;
        int lines = 1;
        bool isReplaceInSpace = false;
        int len = elemRenderArr.Count;

        for (int i = 0; i < len; i++)
        {
            isReplaceInSpace = false;
            LRenderElement elem = elemRenderArr[i];
            if (elem.isNewLine) // new line
            {
                oneLine = 0;
                lines++;
                elem.width = 10;
                elem.height = 27;
                elem.pos = new Vector2(oneLine, -lines * 27);

            }
            else //other elements
            {
                if (oneLine + elem.width > maxLineWidth)
                {
                    if (elem.type == Type.TEXT)
                    {
                       if (isChinese(elem.strChar) || elem.strChar == " ")
                       {
                           oneLine = 0;
                           lines++;

                           elem.pos = new Vector2(oneLine, -lines * 27);
                           oneLine = elem.width;
                       }
                       else // en
                       {
                           int spaceIdx = 0;
                           int idx = i;
                           while (idx > 0)
                           {
                               idx--;
                               if (elemRenderArr[idx].strChar == " ")
                               {
                                   spaceIdx = idx;
                                   break;
                               }
                           }
                           // can't find space , force new line
                           if (spaceIdx == 0)
                           {
                               oneLine = 0;
                               lines++;
                               elem.pos = new Vector2(oneLine, -lines * 27);
                               oneLine = elem.width;
                           }
                           else
                           {
                               oneLine = 0;
                               lines++;
                               isReplaceInSpace = true; //reset cuting words position

                               for (int _i = spaceIdx +1; _i <= i; ++_i)
                               {
                                   LRenderElement _elem = elemRenderArr[_i];
                                   _elem.pos = new Vector2(oneLine, -lines * 27);
                                   oneLine += _elem.width;

                                   elemRenderArr[_i] = _elem;
                               }
                           }
                       }
                    }else if (elem.type == Type.ANIM || elem.type == Type.IMAGE)
                    {
                        lines++;
                        elem.pos = new Vector2(0, -lines * 27);
                        oneLine = elem.width;
                    }
                }
                else
                {
                    elem.pos = new Vector2(oneLine, -lines * 27);
                    oneLine += elem.width;
                }
            }
            if (isReplaceInSpace == false)
            {
                elemRenderArr[i] = elem;
            }
        }
        //sort all lines
        Dictionary<int,List<LRenderElement>> rendElemLineMap = new Dictionary<int,List<LRenderElement>>();
        List<int> lineKeyList = new List<int>();
        len = elemRenderArr.Count;
        for (int i = 0; i < len ; i++ )
        {
            LRenderElement elem = elemRenderArr[i];
            List<LRenderElement> lineList;
            
            if (!rendElemLineMap.ContainsKey((int)elem.pos.y))
            {
                lineList = new List<LRenderElement>();
                rendElemLineMap[(int)elem.pos.y] = lineList;
            }
            rendElemLineMap[(int)elem.pos.y].Add(elem);
        }
        // all lines in arr
        List<List<LRenderElement>> rendLineArrs = new List<List<LRenderElement>>();
        foreach (var item in rendElemLineMap)
        {
            lineKeyList.Add(-1 * item.Key);
        }
        lineKeyList.Sort();
        len = lineKeyList.Count;

        for (int i = 0; i < len; i++)
        {
            int posY = -1 * lineKeyList[i];
            string lineString = "";
            LRenderElement _lastEleme = rendElemLineMap[posY][0];
            LRenderElement _lastDiffStartEleme = rendElemLineMap[posY][0];
            if (rendElemLineMap[posY].Count > 0)
            {
                List<LRenderElement> lineElemArr = new List<LRenderElement>();

                foreach (LRenderElement elem in rendElemLineMap[posY])
                {
                    if (_lastEleme.type == Type.TEXT && elem.type == Type.TEXT)
                    {
                        if (_lastEleme.color == elem.color)
                        {
                            // the same color can mergin one string
                            lineString += elem.strChar;
                        }
                        else // diff color
                        {
                            if (_lastDiffStartEleme.type == Type.TEXT)
                            {
                                LRenderElement _newElem = _lastDiffStartEleme.Clone();
                                _newElem.strChar = lineString;
                                lineElemArr.Add(_newElem);

                                _lastDiffStartEleme = elem;
                                lineString = elem.strChar;
                            }
                        }
                    }
                    else if (elem.type == Type.IMAGE || elem.type == Type.ANIM || elem.type == Type.NEWLINE)
                    {
                        //interrupt
                        if (_lastDiffStartEleme.type == Type.TEXT)
                        {
                            LRenderElement _newEleme = _lastDiffStartEleme.Clone();
                            _newEleme.strChar = lineString;
                            lineString = "";
                            lineElemArr.Add(_newEleme);
                        }
                        lineElemArr.Add(elem);

                    }else if (_lastEleme.type != Type.TEXT)
                    {
                        //interrupt
                        _lastDiffStartEleme = elem;
                        if (elem.type == Type.TEXT)
                        {
                            lineString = elem.strChar;
                        }
                    }
                    _lastEleme = elem;
                }
                // the last elementText
                if (_lastDiffStartEleme.type == Type.TEXT)
                {
                    LRenderElement _newElem = _lastDiffStartEleme.Clone();
                    _newElem.strChar = lineString;
                    lineElemArr.Add(_newElem);
                }
                rendLineArrs.Add(lineElemArr);
            }
        }

        // offset position
        int _offsetLineY = 0;
        realLineHeight = 0;
        len = rendLineArrs.Count;
        for (int i = 0; i < len; i++ )
        {
            List<LRenderElement> _lines = rendLineArrs[i];
            int _lineHeight = 0;
            foreach (LRenderElement elem in _lines)
            {
                _lineHeight = Mathf.Max(_lineHeight, elem.height);
            }
            realLineHeight += _lineHeight;
            _offsetLineY += (_lineHeight - 27);

            for (int j = 0; j < _lines.Count; j++ )
            {
                LRenderElement elem = _lines[j];
                elem.pos = new Vector2(elem.pos.x, elem.pos.y - _offsetLineY);
                realLineHeight = Mathf.Max(realLineHeight, (int)Mathf.Abs(elem.pos.y));
            }
            rendLineArrs[i] = _lines;
        }
    
        // place all position
        realLineWidth = 0;
        GameObject obj = null;
        foreach (List<LRenderElement> _lines in rendLineArrs)
        {
            int _lineWidth = 0;
            foreach (LRenderElement elem in _lines)
            {
                if (elem.type != Type.NEWLINE)
                {
                    if (elem.type == Type.TEXT)
                    {
                        obj = getCacheLabel();
                        makeLabel(obj, elem);
                    }
                    else if (elem.type == Type.IMAGE)
                    {
                        obj = getCacheImage();
                        makeImage(obj, elem);
                    }
                    else if (elem.type == Type.ANIM)
                    {
                        obj = getCacheImage();
                        makeImage(obj, elem);
                    }
                    _lineWidth += elem.width;
                    obj.transform.SetParent(transform);
					obj.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y + realLineHeight);
                }
            }
            realLineWidth = Mathf.Max(_lineWidth, realLineWidth);
        }

        RectTransform rtran = this.GetComponent<RectTransform>();
        //align
        if (alignType == AlignType.DESIGN_CENTER)
        {
            rtran.anchoredPosition = new Vector2(0.5f, 0.5f);
            rtran.sizeDelta = new Vector2(maxLineWidth, realLineHeight);

        }else if (alignType == AlignType.REAL_CENTER)
        {
            rtran.anchoredPosition = new Vector2(0.5f, 0.5f);
            rtran.sizeDelta = new Vector2(realLineWidth, realLineHeight);

        }else if (alignType == AlignType.LEFT_TOP)
        {
            rtran.anchoredPosition = new Vector2(0, 1);
            rtran.sizeDelta = new Vector2(realLineWidth, realLineHeight);
        }
    }

   	void makeLabel(GameObject lab,LRenderElement elem)
    {
        Text comText = lab.GetComponent<Text>();
        if (comText != null)
        {
            comText.text = elem.strChar;
            comText.font = elem.font;
            comText.fontSize = elem.fontSize;
            comText.fontStyle = FontStyle.Normal;
            comText.color = elem.color;
        }

        if (elem.isUnderLine)
        {
            GameObject underLine = getCacheLabel();
            Text underText = underLine.GetComponent<Text>();
            underText.text = "_";
            underText.color = elem.color;
            underText.font = elem.font;
            underText.fontSize = elem.fontSize;
            underText.fontStyle = FontStyle.Normal;
            underLine.transform.localPosition = lab.transform.localPosition;
        }
    }

    void makeImage(GameObject img, LRenderElement elem)
    {
        Image comImage = img.GetComponent<Image>();
        if (comImage != null)
        {
            Texture2D tex = Resources.Load(elem.path) as Texture2D;
            comImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),Vector2.zero);
        }
    }

    void makeAnim(GameObject anim, LRenderElement elem)
    {
        LFrameAnimation comFram = anim.GetComponent<LFrameAnimation>();
        if (comFram != null)
        {
            comFram.path = elem.path;
            comFram.fps = elem.fs;
            comFram.play();
        }
    }

    protected GameObject getCacheLabel()
    {
        GameObject ret = null;
        int len = cacheLabElements.Count;
        for (int i = 0; i < len;i++ )
        {
            LRichCacheElement cacheElem = cacheLabElements[i];
            if (cacheElem.isUse == false)
            {
                cacheElem.isUse = true;
                ret = cacheElem.node;
                break;
            }
        }
        if (ret == null)
        {
            ret = new GameObject();
            Text comp = ret.AddComponent<Text>();
            ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			ret.GetComponent<RectTransform>().pivot = Vector2.zero;
            LRichCacheElement cacheElem = new LRichCacheElement(ret);
            cacheElem.isUse = true;
            cacheLabElements.Add(cacheElem);
        }
        return ret;
    }

    protected GameObject getCacheImage()
    {
        GameObject ret = null;
        int len = cacheLabElements.Count;
        for (int i = 0; i < len; i++)
        {
            LRichCacheElement cacheElem = cacheLabElements[i];
            if (cacheElem.isUse == false)
            {
                cacheElem.isUse = true;
                ret = cacheElem.node;
                break;
            }
        }
        if (ret == null)
        {
            ret = new GameObject();
            ret.AddComponent<Image>();
            LRichCacheElement cacheElem = new LRichCacheElement(ret);
            cacheElem.isUse = true;
            cacheLabElements.Add(cacheElem);
        }
        return ret;
    }

    protected bool isChinese(string text)
    {
        bool hasChinese = false;
        char[] c = text.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
            {
                hasChinese = true;
                break;
            }
        }
        return hasChinese;
    }

	// Use this for initialization
	void Start () {

		this.insertElement("hello world!!", Color.blue,20, false, false, Color.blue,"");
        this.insertElement("测试b!!", Color.red, 20, false, false, Color.blue, "");
		this.insertElement("测 试哈abc defghij哈!!", Color.green, 25, false, false, Color.blue, "");
	    this.insertElement("测试aaaaafffzz zzzzzzzz zzzzz fff哈哈 哈哈!!", Color.yellow, 20, false, false, Color.blue, "");
		this.reloadData ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

}