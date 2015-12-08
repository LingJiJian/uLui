using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public Font font { get; protected set; }
    public string txt { get; protected set; }
    public bool isUnderLine { get; protected set; }
    public bool isOutLine { get; protected set; }

    public LRichElementText( Color color,string txt,Font font,bool isUnderLine,bool isOutLine,string data )
    {
        this.type = Type.TEXT;
        this.color = color;
        this.txt = txt;
        this.font = font;
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
class TRichElementNewline : LRichElement
{
    public TRichElementNewline()
    {
        this.type = Type.NEWLINE;
    }
}

/// <summary>
/// 缓存结构
/// </summary>
class TRichCacheElement : Object
{
    public bool isUse;
    public GameObject node;
}

/// <summary>
/// 渲染结构
/// </summary>
struct LRenderElement
{
    public Type type;
    public string strChar;
    public float width;
    public float height;
    public bool isOutLine;
    public bool isUnderLine;
    public Font font;
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
        cloneOjb.color = this.color;
        cloneOjb.data = this.data;
        cloneOjb.path = this.path;
        cloneOjb.fp = this.fp;
        cloneOjb.isNewLine = this.isNewLine;
        cloneOjb.pos = this.pos;
        return cloneOjb;
    }
}

public class TRichText : MonoBehaviour, IRichTextClickableProtocol
{
    public AlignType alignType { get; protected set; }
    public float verticalSpace { get; protected set; }
    public float maxLineWidth { get; protected set; }
    public List<LRichElement> richElements { protected get; }
    public List<LRenderElement> elemRenderArr { protected get; }
    public List<TRichCacheElement> cacheLabElements { protected get; }
    public List<TRichCacheElement> cacheImgElements { protected get; }

    public void removeAllElements()
    {
        foreach(TRichCacheElement lab in cacheLabElements)
        {
            lab.isUse = false;
            lab.node.transform.SetParent(null);
        }
        foreach (TRichCacheElement img in cacheImgElements)
        {
            img.isUse = false;
            img.node.transform.SetParent(null);
        }
    }

    public void insertElement(string txt, Font font, Color color, bool isUnderLine, bool isOutLine, Color outLineColor, string data)
    {
        richElements.Add(new LRichElementText(color, txt, font, isUnderLine, isOutLine, data));
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
        richElements.Add(new TRichElementNewline());
    }

    public TRichText()
    {
        this.alignType = AlignType.LEFT_TOP;
        this.verticalSpace = 0;
        this.maxLineWidth = 300;
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
                    rendElem.font = elemText.font;
                    rendElem.data = elemText.data;
                    rendElem.color = elemText.color;

                    TextGenerationSettings setting = new TextGenerationSettings();
                    setting.font = elemText.font;
                    rendElem.width = gen.GetPreferredWidth(rendElem.strChar, setting);
                    rendElem.height = gen.GetPreferredHeight(rendElem.strChar, setting);
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
        float charWidth = 0;
        float oneLine = 0;
        int lines = 1;

    }

	// Use this for initialization
	void Start () {
        //TRichElement t = new TRichElement();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

}