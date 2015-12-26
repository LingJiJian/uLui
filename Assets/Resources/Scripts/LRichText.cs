/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com
http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lui
{

public enum RichType
{
    TEXT,
    IMAGE,
    ANIM,
    NEWLINE,
}

public enum RichAlignType
{
    DESIGN_CENTER,
    LEFT_TOP,
}

class LRichElement : Object {

    public RichType type { get; protected set; }
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
        this.type = RichType.TEXT;
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
        this.type = RichType.IMAGE;
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

    public LRichElementAnim(string path,float fs, string data)
    {
        this.type = RichType.ANIM;
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
        this.type = RichType.NEWLINE;
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
    public RichType type;
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

public class LRichText : MonoBehaviour, IPointerClickHandler
{
	public RichAlignType alignType;
	public int verticalSpace;
	public int maxLineWidth;
	public Font font;

    private UnityAction<string> onClickHandler;
    public int realLineHeight { get; protected set; }
    public int realLineWidth { get; protected set; }

    List<LRichElement> richElements;
    List<LRenderElement> elemRenderArr;
    List<LRichCacheElement> cacheLabElements;
    List<LRichCacheElement> cacheImgElements;
	List<LRichCacheElement> cacheFramAnimElements;
    Dictionary<GameObject, string> objectDataMap;

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

        foreach (LRichCacheElement anim in cacheFramAnimElements)
        {
            anim.isUse = false;
            anim.node.transform.SetParent(null);
        }
        elemRenderArr.Clear();
        objectDataMap.Clear();
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
        this.alignType = RichAlignType.LEFT_TOP;
        this.verticalSpace = 0;
        this.maxLineWidth = 300;

        richElements = new List<LRichElement>();
        elemRenderArr = new List<LRenderElement>();
        cacheLabElements = new List<LRichCacheElement>();
        cacheImgElements = new List<LRichCacheElement>();
		cacheFramAnimElements = new List<LRichCacheElement> ();
        objectDataMap = new Dictionary<GameObject, string>();
    }
   
    public void reloadData()
    {
        this.removeAllElements();

			RectTransform rtran = this.GetComponent<RectTransform>();
			//align
			if (alignType == RichAlignType.DESIGN_CENTER)
			{
				rtran.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

			}else if (alignType == RichAlignType.LEFT_TOP)
			{
				rtran.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
			}

        foreach (LRichElement elem in richElements)
        {
            if (elem.type == RichType.TEXT)
            {
                LRichElementText elemText = elem as LRichElementText;
                char[] _charArr = elemText.txt.ToCharArray();
                TextGenerator gen = new TextGenerator();

                foreach (char strChar in _charArr)
                {
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.TEXT;
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
					setting.verticalOverflow = VerticalWrapMode.Overflow;
					setting.horizontalOverflow = HorizontalWrapMode.Overflow;

                    rendElem.width = (int)gen.GetPreferredWidth(rendElem.strChar, setting);
                    rendElem.height = (int)gen.GetPreferredHeight(rendElem.strChar, setting);
                    elemRenderArr.Add(rendElem);
                }
            }
            else if (elem.type == RichType.IMAGE)
            {
                LRichElementImage elemImg = elem as LRichElementImage;
                LRenderElement rendElem = new LRenderElement();
                rendElem.type = RichType.IMAGE;
                rendElem.path = elemImg.path;
                rendElem.data = elemImg.data;

				Sprite sp = Resources.Load(rendElem.path,typeof(Sprite)) as Sprite;
				rendElem.width = sp.texture.width;
				rendElem.height = sp.texture.height;
                elemRenderArr.Add(rendElem);
            }
            else if (elem.type == RichType.ANIM)
            {
                LRichElementAnim elemAnim = elem as LRichElementAnim;
                LRenderElement rendElem = new LRenderElement();
                rendElem.type = RichType.ANIM;
                rendElem.path = elemAnim.path;
                rendElem.data = elemAnim.data;
                rendElem.fs = elemAnim.fs;
		
				Sprite sp = Resources.Load(rendElem.path+"/1",typeof(Sprite)) as Sprite;
				rendElem.width = sp.texture.width;
				rendElem.height = sp.texture.height;
                elemRenderArr.Add(rendElem);
            }
            else if (elem.type == RichType.NEWLINE)
            {
                LRenderElement rendElem = new LRenderElement();
                rendElem.isNewLine = true;
                elemRenderArr.Add(rendElem);
            }
        }

        richElements.Clear();

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
                    if (elem.type == RichType.TEXT)
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
                               if (elemRenderArr[idx].strChar == " " && 
                                   elemRenderArr[idx].pos.y == elemRenderArr[i-1].pos.y ) // just for the same line
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
                    }else if (elem.type == RichType.ANIM || elem.type == RichType.IMAGE)
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
                    if (_lastEleme.type == RichType.TEXT && elem.type == RichType.TEXT)
                    {
                        if (_lastEleme.color == elem.color)
                        {
                            // the same color can mergin one string
                            lineString += elem.strChar;
                        }
                        else // diff color
                        {
                            if (_lastDiffStartEleme.type == RichType.TEXT)
                            {
                                LRenderElement _newElem = _lastDiffStartEleme.Clone();
                                _newElem.strChar = lineString;
                                lineElemArr.Add(_newElem);

                                _lastDiffStartEleme = elem;
                                lineString = elem.strChar;
                            }
                        }
                    }
                    else if (elem.type == RichType.IMAGE || elem.type == RichType.ANIM || elem.type == RichType.NEWLINE)
                    {
                        //interrupt
                        if (_lastDiffStartEleme.type == RichType.TEXT)
                        {
                            LRenderElement _newEleme = _lastDiffStartEleme.Clone();
                            _newEleme.strChar = lineString;
                            lineString = "";
                            lineElemArr.Add(_newEleme);
                        }
                        lineElemArr.Add(elem);

                    }else if (_lastEleme.type != RichType.TEXT)
                    {
                        //interrupt
                        _lastDiffStartEleme = elem;
                        if (elem.type == RichType.TEXT)
                        {
                            lineString = elem.strChar;
                        }
                    }
                    _lastEleme = elem;
                }
                // the last elementText
                if (_lastDiffStartEleme.type == RichType.TEXT)
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
				_lines[j] = elem;
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
                if (elem.type != RichType.NEWLINE)
                {
                    if (elem.type == RichType.TEXT)
                    {
                        obj = getCacheLabel();
                        makeLabel(obj, elem);
						_lineWidth += (int)obj.GetComponent<Text>().preferredWidth;
                    }
                    else if (elem.type == RichType.IMAGE)
                    {
                        obj = getCacheImage(true);
                        makeImage(obj, elem);
                        _lineWidth += (int)obj.GetComponent<Image>().preferredWidth;
                    }
                    else if (elem.type == RichType.ANIM)
                    {
                        obj = getCacheFramAnim();
                        makeFramAnim(obj, elem);
						_lineWidth += elem.width;
					}
						obj.transform.SetParent(transform);
                    obj.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y /*+ realLineHeight*/);
                    objectDataMap[obj] = elem.data;
                }
            }
            realLineWidth = Mathf.Max(_lineWidth, realLineWidth);
        }

        RectTransform rtran = this.GetComponent<RectTransform>();
        //align
        if (alignType == RichAlignType.DESIGN_CENTER)
        {
			rtran.sizeDelta = new Vector2(maxLineWidth, realLineHeight);

        }else if (alignType == RichAlignType.LEFT_TOP)
        {
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

		Outline outline = lab.GetComponent<Outline>();
		if (elem.isOutLine) {
			if(outline == null){
				outline = lab.AddComponent<Outline>();
			}
		} else {
			if(outline){
				Destroy(outline);
			}
		}

		if (elem.isUnderLine)
        {
            GameObject underLine = getCacheImage(false);
            Image underImg = underLine.GetComponent<Image>();
            underImg.color = elem.color;
            underImg.GetComponent<RectTransform>().sizeDelta = new Vector2(comText.preferredWidth, 1);
            underLine.transform.SetParent(transform);
            underLine.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y);
        }
    }

    void makeImage(GameObject img, LRenderElement elem)
    {
        Image comImage = img.GetComponent<Image>();
        if (comImage != null)
        {
			Sprite sp = Resources.Load(elem.path,typeof(Sprite)) as Sprite;
			comImage.sprite = sp;
        }
    }

	void makeFramAnim(GameObject anim, LRenderElement elem)
    {
        LFrameAnimation comFram = anim.GetComponent<LFrameAnimation>();
        if (comFram != null)
        {
            comFram.path = elem.path;
			comFram.fps = elem.fs;
			comFram.loadTexture ();
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
            ret.AddComponent<Text>();
            ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			
			RectTransform rtran = ret.GetComponent<RectTransform>();
			rtran.pivot = Vector2.zero;
			rtran.anchorMax =new Vector2(0,1);
			rtran.anchorMin = new Vector2(0,1);

            LRichCacheElement cacheElem = new LRichCacheElement(ret);
            cacheElem.isUse = true;
            cacheLabElements.Add(cacheElem);
        }
        return ret;
    }

    protected GameObject getCacheImage(bool isFitSize)
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
				ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform rtran = ret.GetComponent<RectTransform>();
            rtran.pivot = Vector2.zero;
            rtran.anchorMax = new Vector2(0, 1);
            rtran.anchorMin = new Vector2(0, 1);
            
            LRichCacheElement cacheElem = new LRichCacheElement(ret);
            cacheElem.isUse = true;
            cacheLabElements.Add(cacheElem);
        }
		ContentSizeFitter fitCom = ret.GetComponent<ContentSizeFitter>();
		fitCom.enabled = isFitSize;
        return ret;
    }

	protected GameObject getCacheFramAnim()
	{
		GameObject ret = null;
		int len = cacheFramAnimElements.Count;
		for (int i = 0; i < len;i++ )
		{
			LRichCacheElement cacheElem = cacheFramAnimElements[i];
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
            ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform rtran = ret.GetComponent<RectTransform>();
            rtran.pivot = Vector2.zero;
            rtran.anchorMax = new Vector2(0, 1);
            rtran.anchorMin = new Vector2(0, 1);

			ret.AddComponent<LFrameAnimation>();
			
			LRichCacheElement cacheElem = new LRichCacheElement(ret);
			cacheElem.isUse = true;
			cacheFramAnimElements.Add(cacheElem);
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
        
		this.insertElement("hello world!!", Color.blue,25, true, false, Color.blue,"数据");
		this.insertElement("测试文本内容!!", Color.red, 15, false, true, Color.blue, "");
		this.insertElement("Image/face01",5f,"");
		this.insertElement("The article comes from the point of the examination", Color.green, 15, true, false, Color.blue, "");
		this.insertElement("Image/face02/1","");
		this.insertElement (1);
	    this.insertElement("outline and newline", Color.yellow, 20, false, true, Color.blue, "");
        
        this.reloadData ();
	}

    public void setClickHandler(UnityAction<string> action)
    {
		onClickHandler = action;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (objectDataMap.ContainsKey(data.pointerEnter))
        {
            if ((onClickHandler !=null) && (objectDataMap[data.pointerEnter] != ""))
            {
                onClickHandler.Invoke(objectDataMap[data.pointerEnter]);
            }
        }
        
    }
}

}