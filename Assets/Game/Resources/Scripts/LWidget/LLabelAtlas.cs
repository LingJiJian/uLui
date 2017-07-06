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
    [SLua.CustomLuaClass]
    public class LLabelAtlas : MonoBehaviour
    {
        public string text;
        public string atlas;
        public string prefix;
        private Sprite[] sprites;
        Dictionary<string, Sprite> _spriteMap;
        List<LCacheElement> _cacheImg;
        public float lineSpacing;

		public TextAlignment align;

        public LLabelAtlas()
        {
			align = TextAlignment.Left;
            _cacheImg = new List<LCacheElement>();
            _spriteMap = new Dictionary<string, Sprite>();
        }

        void Start()
        {
            reload();
        }

        public void reload()
        {
            if (text != "")
            {
				sprites = LLoadBundle.GetInstance().GetSpritesByName(atlas,prefix);
                loadTexture();
                render();
            }
        }

        protected void loadTexture()
        {
            _spriteMap.Clear();
            for (int i = 0; i < sprites.Length; i++)
            {
                _spriteMap.Add(sprites[i].name, sprites[i]);
            }
        }

        protected void render()
        {
            //reset
			int _len = _cacheImg.Count;
            for (int i=0;i< _len; i++)
            {
                _cacheImg[i].isUse = false;
				_cacheImg [i].node.SetActive (false);
            }

            char[] chars = text.ToCharArray();
            int len = chars.Length;

			float preWidth = 0;
			float preHeight = 0;
			List<Image> imgs = new List<Image> ();
            for (int i = 0; i < len; i++ )
            {
                string key = chars[i].ToString();
				if (_spriteMap.ContainsKey(prefix+key))
                {
                    GameObject img = getCacheImage();
                    Image imgCom = img.GetComponent<Image>();
					imgCom.sprite = _spriteMap[prefix+key];
                    img.transform.SetParent(this.transform);
					img.SetActive (true);
                    img.transform.localScale = new Vector3(1, 1, 1);
					float space = i == len - 1 ? 0 : lineSpacing;
					preWidth += imgCom.sprite.textureRect.width + space;
					imgs.Add (imgCom);
                }
            }
           
			float offsetX = 0;
			RectTransform rtran = gameObject.GetComponent<RectTransform>();

			if (align == TextAlignment.Left) {
				for(int i =0;i<imgs.Count;i++){
					Image img = imgs[i];
					float space = i == imgs.Count - 1 ? 0 : lineSpacing;
					img.transform.localPosition = new Vector2 (offsetX,0);
					offsetX += img.sprite.textureRect.width + space; 
					preHeight = Mathf.Max (img.sprite.textureRect.height, preHeight);
				}
				rtran.pivot = Vector2.zero;
			} else if (align == TextAlignment.Center) {
				for(int i =0;i<imgs.Count;i++){
					Image img = imgs[i];
					float space = i == imgs.Count - 1 ? 0 : lineSpacing;
					img.transform.localPosition = new Vector2 (-preWidth / 2 + offsetX, 0);
					offsetX += img.sprite.textureRect.width + space;
					preHeight = Mathf.Max (img.sprite.textureRect.height, preHeight);
				}
				rtran.pivot = new Vector2(0.5f,0f);
			} else if (align == TextAlignment.Right) {
				for(int i =0;i<imgs.Count;i++){
					Image img = imgs[i];
					float space = i == imgs.Count - 1 ? 0 : lineSpacing;
					img.transform.localPosition = new Vector2 (-preWidth + offsetX, 0);
					offsetX += img.sprite.textureRect.width + space;
					preHeight = Mathf.Max (img.sprite.textureRect.height, preHeight);
				}
				rtran.pivot = new Vector2(1f,0f);
			}
			rtran.sizeDelta = new Vector2(preWidth, preHeight);
        }

        protected GameObject getCacheImage()
        {
            GameObject ret = null;
            int len = _cacheImg.Count;
            for (int i = 0; i < len; i++)
            {
                LCacheElement cacheElem = _cacheImg[i];
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
                rtran.anchorMax = new Vector2(0, 0);
                rtran.anchorMin = new Vector2(0, 0);

                LCacheElement cacheElem = new LCacheElement(ret);
                cacheElem.isUse = true;
                _cacheImg.Add(cacheElem);
            }
            return ret;
        } 
    }
}
