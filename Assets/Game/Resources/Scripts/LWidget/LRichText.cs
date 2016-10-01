/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com

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
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SLua;

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

    class LRichElement : Object
    {
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
        public Color outLineColor { get; protected set; }

        public LRichElementText(Color color, string txt, int fontSize, bool isUnderLine, bool isOutLine,Color outLineColor, string data)
        {
            this.type = RichType.TEXT;
            this.color = color;
            this.txt = txt;
            this.fontSize = fontSize;
            this.isUnderLine = isUnderLine;
            this.isOutLine = isOutLine;
            this.outLineColor = outLineColor;
            this.data = data;
        }
    }

    /// <summary>
    /// 图片元素
    /// </summary>
    class LRichElementImage : LRichElement
    {
        public string path { get; protected set; }

        public LRichElementImage(string path, string data)
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

        public LRichElementAnim(string path, float fs, string data)
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
        public Color outLineColor;
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
            cloneOjb.outLineColor = this.outLineColor;
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

		public bool isSameStyle(LRenderElement elem)
		{
			return (this.color 			== elem.color &&
				    this.isOutLine 		== elem.isOutLine &&
					this.isUnderLine 	== elem.isUnderLine &&
                    this.outLineColor   == elem.outLineColor &&
                    this.font 			== elem.font &&
					this.fontSize 		== elem.fontSize &&
					this.data 			== elem.data);
		}
    }

    /// <summary>
    /// 富文本
    /// </summary>
    [CustomLuaClassAttribute]
    public class LRichText : MonoBehaviour, IPointerClickHandler
    {
        public RichAlignType alignType;
        public int verticalSpace;
        public int maxLineWidth;
        public Font font;

        public UnityAction<string> onClickHandler;
        public int realLineHeight { get; protected set; }
        public int realLineWidth { get; protected set; }

        List<LRichElement> _richElements;
        List<LRenderElement> _elemRenderArr;
        List<LRichCacheElement> _cacheLabElements;
        List<LRichCacheElement> _cacheImgElements;
        List<LRichCacheElement> _cacheFramAnimElements;
        Dictionary<GameObject, string> _objectDataMap;
        //custom content parser setting
        public int defaultLabSize = 20;
        public string defaultLabColor = "#ff00ff";

        public void removeAllElements()
        {
            foreach (LRichCacheElement lab in _cacheLabElements)
            {
                lab.isUse = false;
                lab.node.gameObject.SetActive(false);
            }
            foreach (LRichCacheElement img in _cacheImgElements)
            {
                img.isUse = false;
                img.node.gameObject.SetActive(false);
            }

            foreach (LRichCacheElement anim in _cacheFramAnimElements)
            {
                anim.isUse = false;
                anim.node.gameObject.SetActive(false);
            }
            _elemRenderArr.Clear();
            _objectDataMap.Clear();
        }

        public void insertElement(string txt, Color color, int fontSize, bool isUnderLine, bool isOutLine, Color outLineColor, string data)
        {
            _richElements.Add(new LRichElementText(color, txt, fontSize, isUnderLine, isOutLine, outLineColor, data));
        }

        public void insertElement(string path, float fp, string data)
        {
            _richElements.Add(new LRichElementAnim(path, fp, data));
        }

        public void insertElement(string path, string data)
        {
            _richElements.Add(new LRichElementImage(path, data));
        }

        public void insertElement(int newline)
        {
            _richElements.Add(new LRichElementNewline());
        }

        public LRichText()
        {
            this.alignType = RichAlignType.LEFT_TOP;
            this.verticalSpace = 0;
            this.maxLineWidth = 300;

            _richElements = new List<LRichElement>();
            _elemRenderArr = new List<LRenderElement>();
            _cacheLabElements = new List<LRichCacheElement>();
            _cacheImgElements = new List<LRichCacheElement>();
            _cacheFramAnimElements = new List<LRichCacheElement>();
            _objectDataMap = new Dictionary<GameObject, string>();
        }

        public void reloadData()
        {
            this.removeAllElements();

            RectTransform rtran = this.GetComponent<RectTransform>();
            //align
            if (alignType == RichAlignType.DESIGN_CENTER)
            {
                rtran.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            }
            else if (alignType == RichAlignType.LEFT_TOP)
            {
                rtran.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
            }

            foreach (LRichElement elem in _richElements)
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
                        rendElem.outLineColor = elemText.outLineColor;
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
                        _elemRenderArr.Add(rendElem);
                    }
                }
                else if (elem.type == RichType.IMAGE)
                {
                    LRichElementImage elemImg = elem as LRichElementImage;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.IMAGE;
                    rendElem.path = elemImg.path;
                    rendElem.data = elemImg.data;

					string atlas = System.IO.Path.GetDirectoryName(rendElem.path);
					string spname = System.IO.Path.GetFileName(rendElem.path);

                    Sprite sp = LLoadBundle.GetInstance().GetSpriteByName(atlas, spname);
                    rendElem.width = (int)sp.rect.size.x;
                    rendElem.height = (int)sp.rect.size.y;
                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.ANIM)
                {
                    LRichElementAnim elemAnim = elem as LRichElementAnim;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.ANIM;
                    rendElem.path = elemAnim.path;
                    rendElem.data = elemAnim.data;
                    rendElem.fs = elemAnim.fs;

                    string atlas = System.IO.Path.GetDirectoryName(rendElem.path);
                    string spname = System.IO.Path.GetFileName(rendElem.path);

                    Sprite sp = LLoadBundle.GetInstance().GetSpriteByName(atlas, spname);
                    rendElem.width = (int)sp.rect.size.x;
                    rendElem.height = (int)sp.rect.size.y;
                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.NEWLINE)
                {
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.isNewLine = true;
                    _elemRenderArr.Add(rendElem);
                }
            }

            _richElements.Clear();

            formatRenderers();
        }

        protected void formatRenderers()
        {
            int oneLine = 0;
            int lines = 1;
            bool isReplaceInSpace = false;
            int len = _elemRenderArr.Count;

            for (int i = 0; i < len; i++)
            {
                isReplaceInSpace = false;
                LRenderElement elem = _elemRenderArr[i];
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
                                    if (_elemRenderArr[idx].strChar == " " &&
                                        _elemRenderArr[idx].pos.y == _elemRenderArr[i - 1].pos.y) // just for the same line
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

                                    for (int _i = spaceIdx + 1; _i <= i; ++_i)
                                    {
                                        LRenderElement _elem = _elemRenderArr[_i];
                                        _elem.pos = new Vector2(oneLine, -lines * 27);
                                        oneLine += _elem.width;

                                        _elemRenderArr[_i] = _elem;
                                    }
                                }
                            }
                        }
                        else if (elem.type == RichType.ANIM || elem.type == RichType.IMAGE)
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
                    _elemRenderArr[i] = elem;
                }
            }
            //sort all lines
            Dictionary<int, List<LRenderElement>> rendElemLineMap = new Dictionary<int, List<LRenderElement>>();
            List<int> lineKeyList = new List<int>();
            len = _elemRenderArr.Count;
            for (int i = 0; i < len; i++)
            {
                LRenderElement elem = _elemRenderArr[i];
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
							if (_lastEleme.isSameStyle(elem))
                            {
								// the same style can mergin one element
                                lineString += elem.strChar;
                            }
                            else // diff style
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

                        }
                        else if (_lastEleme.type != RichType.TEXT)
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
            for (int i = 0; i < len; i++)
            {
                List<LRenderElement> _lines = rendLineArrs[i];
                int _lineHeight = 0;
                foreach (LRenderElement elem in _lines)
                {
                    _lineHeight = Mathf.Max(_lineHeight, elem.height);
                }

                realLineHeight += _lineHeight;
                _offsetLineY += (_lineHeight - 27);

                int _len = _lines.Count;
                for (int j = 0; j < _len; j++)
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
                        obj.SetActive(true);
                        obj.transform.SetParent(transform);
                        obj.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y /*+ realLineHeight*/);
                        obj.transform.localScale = new Vector2(1, 1);
                        _objectDataMap[obj] = elem.data;
                    }
                }
                realLineWidth = Mathf.Max(_lineWidth, realLineWidth);
            }

            RectTransform rtran = this.GetComponent<RectTransform>();
            //align
            if (alignType == RichAlignType.DESIGN_CENTER)
            {
                rtran.sizeDelta = new Vector2(maxLineWidth, realLineHeight);

            }
            else if (alignType == RichAlignType.LEFT_TOP)
            {
                rtran.sizeDelta = new Vector2(realLineWidth, realLineHeight);
            }
        }

        void makeLabel(GameObject lab, LRenderElement elem)
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
            if (elem.isOutLine)
            {
                if (outline == null)
                {
                    outline = lab.AddComponent<Outline>();
                    outline.effectColor = elem.outLineColor;
                }
            }
            else {
                if (outline)
                {
                    Destroy(outline);
                }
            }

            if (elem.isUnderLine)
            {
                GameObject underLine = getCacheImage(false);
                Image underImg = underLine.GetComponent<Image>();
                underImg.color = elem.color;
                underImg.GetComponent<RectTransform>().sizeDelta = new Vector2(comText.preferredWidth, 1);
                underLine.SetActive(true);
                underLine.transform.SetParent(transform);
                underLine.transform.localScale = new Vector2(1, 1);
                underLine.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y);
            }
        }

        void makeImage(GameObject img, LRenderElement elem)
        {
            Image comImage = img.GetComponent<Image>();
            if (comImage != null)
            {
				string atlas = System.IO.Path.GetDirectoryName(elem.path);
				string spname = System.IO.Path.GetFileName(elem.path);
                Sprite sp = LLoadBundle.GetInstance().GetSpriteByName(atlas, spname);
                comImage.sprite = sp;
            }
        }

        void makeFramAnim(GameObject anim, LRenderElement elem)
        {
            LMovieClip comFram = anim.GetComponent<LMovieClip>();
            if (comFram != null)
            {
                comFram.path = elem.path;
                comFram.fps = elem.fs;
                comFram.loadTexture();
                comFram.play();
            }
        }

        protected GameObject getCacheLabel()
        {
            GameObject ret = null;
            int len = _cacheLabElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheLabElements[i];
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
                rtran.anchorMax = new Vector2(0, 1);
                rtran.anchorMin = new Vector2(0, 1);

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheLabElements.Add(cacheElem);
            }
            return ret;
        }

        protected GameObject getCacheImage(bool isFitSize)
        {
            GameObject ret = null;
            int len = _cacheImgElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheImgElements[i];
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
                _cacheImgElements.Add(cacheElem);
            }
            ContentSizeFitter fitCom = ret.GetComponent<ContentSizeFitter>();
            fitCom.enabled = isFitSize;
            return ret;
        }

        protected GameObject getCacheFramAnim()
        {
            GameObject ret = null;
            int len = _cacheFramAnimElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheFramAnimElements[i];
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

                ret.AddComponent<LMovieClip>();

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheFramAnimElements.Add(cacheElem);
            }
            return ret;
        }

        protected bool isChinese(string text)
        {
            bool hasChinese = false;
            char[] c = text.ToCharArray();
            int len = c.Length;
            for (int i = 0; i < len; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    hasChinese = true;
                    break;
                }
            }
            return hasChinese;
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (_objectDataMap.ContainsKey(data.pointerEnter))
            {
                if ((onClickHandler != null) && (_objectDataMap[data.pointerEnter] != ""))
                {
                    onClickHandler.Invoke(_objectDataMap[data.pointerEnter]);
                }
            }
        }
//---------------------parse rich element content from string----------------------------------------
        private string[] SaftSplite(string content, char separater)
        {
            List<string> arr = new List<string>();
            char[] charArr = content.ToCharArray();
            bool strFlag = false;
            List<char> line = new List<char>();
            for (int i =0;i< charArr.Length; i++)
            {
                if((charArr[i] == '"') && (charArr[i-1] != '\\')) //string start
                {
                    strFlag = !strFlag;
                }
                if(charArr[i] == separater)
                {
                    if (!strFlag)
                    {
                        arr.Add(new string(line.ToArray()));
                        line.Clear();
                    }
                    else
                    {
                        line.Add(charArr[i]);
                    }
                }
                else
                {
                    line.Add(charArr[i]);
                }
            }
            if(line.Count > 0)
            {
                arr.Add(new string(line.ToArray()));
            }
            return arr.ToArray();
        }

        public void parseRichElemString(string content, UnityAction<string, Dictionary<string, string>> handleFunc)
        {
            List<string> elemStrs = new List<string>();

            int startIndex = 0;
            elemStrs = executeParseRichElem(elemStrs, content, startIndex);

            int len = elemStrs.Count;
            for (int i = 0; i < len; i++)
            {
                string flag = elemStrs[i].Substring(0, elemStrs[i].IndexOf(" "));
                string paramStr = elemStrs[i].Substring(elemStrs[i].IndexOf(" ") + 1);
                string[] paramArr = SaftSplite(paramStr,' ');

                Dictionary<string, string> param = new Dictionary<string, string>();
                int paramArrLen = paramArr.Length;
                for (int j = 0; j < paramArrLen; j++)
                {
                    string[] paramObj = SaftSplite(paramArr[j], '=');
                    string left = paramObj[0].Trim();
                    string right = paramObj[1].Trim();

                    if (right.EndsWith("\"") && right.StartsWith("\""))
                    {
                        param.Add(left, right.Trim('"'));
                    }
                    else
                    {
                        param.Add(left, right);
                    }
                }
                handleFunc.Invoke(flag, param);
            }
        }

        private List<string> executeParseRichElem(List<string> result, string content, int startIndex)
        {
            bool hasMatch = false;
            int matchIndex = content.IndexOf("<", startIndex);
            if (matchIndex != -1)
            {
                result.Add(string.Format("lab txt=\"{0}\"", content.Substring(startIndex, matchIndex - startIndex))); //match head
                startIndex = matchIndex;

                matchIndex = content.IndexOf("/>", startIndex);
                if (matchIndex != -1)
                {
                    hasMatch = true;
                    result.Add(content.Substring(startIndex + 1, matchIndex - (startIndex + 1))); //match tail
                    startIndex = matchIndex + 2;
                }
            }

            if (hasMatch)
            {
                return executeParseRichElem(result, content, startIndex);
            }
            else
            {
                result.Add(string.Format("lab txt=\"{0}\"", content.Substring(startIndex, content.Length - startIndex)));
                return result;
            }
        }

        public void parseRichDefaultString(string content, UnityAction<string, Dictionary<string, string>> specHandleFunc=null)
        {
            parseRichElemString(content, (flag, param) =>
            {
                if (flag == "lab")
                {
                    this.insertElement(
                        param.ContainsKey("txt") ? param["txt"] : "",
                        LUtil.StringToColor(param.ContainsKey("color") ? param["color"] : defaultLabColor),
                        param.ContainsKey("size") ? System.Convert.ToInt32(param["size"]) : defaultLabSize,
                        param.ContainsKey("isUnderLine") ? System.Convert.ToBoolean(param["isUnderLine"]) : false,
                        param.ContainsKey("isOutLine") ? System.Convert.ToBoolean(param["isOutLine"]) : false,
                        LUtil.StringToColor(param.ContainsKey("outLineColor") ? param["outLineColor"] : "#000000"),
                        param.ContainsKey("data") ? param["data"] : ""
                        );
                }else if(flag == "img")
                {
                    this.insertElement(
                         param.ContainsKey("path") ? param["path"] : "",
                         param.ContainsKey("data") ? param["data"] : "");
                }else if(flag == "anim")
                {
                    this.insertElement(param["path"],
                         param.ContainsKey("fps") ? System.Convert.ToSingle(param["fps"]) : 15f,
                         param.ContainsKey("data") ? param["data"] : "");
                }else if(flag == "newline")
                {
                    this.insertElement(1);
                }
                else
                {
                    if(specHandleFunc != null) specHandleFunc.Invoke(flag, param);
                }
            });
            this.reloadData();
        }
    }

}