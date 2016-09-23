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
    /// 序列帧动画
    /// </summary>
    [CustomLuaClassAttribute]
	public class LMovieClip : MonoBehaviour
    {
        public float fps = 15f;
		public bool isPlayOnwake = false;
		public string path;

        protected Image _comImage;
        protected float _time;
        protected int _frameLenght;
        protected bool _isPlaying = false;
		protected int _currentIndex = 0;
        protected Sprite[] _spriteArr;

        // Use this for initialization
        void Start()
        {
            _comImage = gameObject.GetComponent<Image>();

			if (isPlayOnwake) {
				loadTexture ();
				play ();
			}
        }

		public void loadTexture()
		{
            //load textures
            //_spriteArr = LLoadBundle.GetInstance().LoadAllAsset<Sprite>(LGameConfig.ASSETBUNDLE_LOAD_FORMAT, path);
            LTextureAtlas.GetInstance().LoadData(path);
            _spriteArr = LTextureAtlas.GetInstance().getSprites(path);
            _frameLenght = _spriteArr.Length;
		}

        void Update()
        {
            if (_isPlaying)
            {
                drawAnimation();
            }
        }

        // Update is called once per frame
        protected void drawAnimation()
        {
            _comImage.sprite = _spriteArr[_currentIndex];

            if (_currentIndex < _frameLenght)
            {
                _time += Time.deltaTime;
                if (_time >= 1.0f / fps)
                {
					_currentIndex++;
                    _time = 0;
                    if (_currentIndex == _frameLenght)
                    {
                        _currentIndex = 0;
                    }
                }
            }
        }

        public void play()
        {
            _isPlaying = true;
        }

        public void stop()
        {
            _isPlaying = false;
            _currentIndex = 0;
            _comImage.sprite = _spriteArr[0];
        }

        public void pause()
        {
            _isPlaying = false;
        }
    }
}
