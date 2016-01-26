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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SLua;

namespace Lui
{
    /// <summary>
    /// 缓存结构
    /// </summary>
    class LCacheElement : Object
    {
        public bool isUse;
        public GameObject node;
        public LCacheElement(GameObject node)
        {
            this.node = node;
        }
    }

    /// <summary>
    /// 图集字
    /// </summary>
    [CustomLuaClassAttribute]
    public class LLabelAtlas : MonoBehaviour
    {
        public string text;
        public string path;
        protected Dictionary<string, Sprite> _spriteMap;
        protected List<LCacheElement> _cacheImg;

        public LLabelAtlas()
        {
            _spriteMap = new Dictionary<string, Sprite>();
            _cacheImg = new List<LCacheElement>();
        }

        void Start()
        {
            if (path != "" && text != "")
            {
                loadTexture();
                render();
            }
        }

        protected void loadTexture()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(path);
            for (int i = 0; i < sprites.Length; i++)
            {
                _spriteMap.Add(sprites[i].name, sprites[i]);
            }
        }

        protected void render()
        {
            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length;i++ )
            {
                //chars[i].ToString()
            }
        }

        protected GameObject getCacheImage(bool isFitSize)
        {
            GameObject ret = null;
            int len = _cacheImg.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheImg[i];
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
                _cacheImg.Add(cacheElem);
            }
            ContentSizeFitter fitCom = ret.GetComponent<ContentSizeFitter>();
            fitCom.enabled = isFitSize;
            return ret;
        } 
    }
}
