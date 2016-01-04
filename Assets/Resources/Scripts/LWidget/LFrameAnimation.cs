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
	public class LFrameAnimation : MonoBehaviour
    {
        
        public float fps = 15f;
		public bool isPlayOnwake = false;
		public string path;

        protected Image comImage;
        protected float time;
        protected int frameLenght;
        protected bool isPlaying = false;
		protected int currentIndex = 0;
        protected Sprite[] spriteArr;

        // Use this for initialization
        void Start()
        {
            comImage = gameObject.GetComponent<Image>();

			if (isPlayOnwake) {
				loadTexture ();
				play ();
			}
        }

		public void loadTexture()
		{
			//load textures
			Object[] texObj = Resources.LoadAll(path,typeof(Sprite));
            frameLenght = texObj.Length;
            spriteArr = new Sprite[frameLenght];

            for (int i = 0; i < frameLenght; i++)
            {
				Sprite sp = texObj[i] as Sprite;
				spriteArr[i] = sp;
            }
		}

        void OnGUI()
        {
            if (isPlaying)
            {
                drawAnimation();
            }
        }

        // Update is called once per frame
        protected void drawAnimation()
        {
            comImage.sprite = spriteArr[currentIndex];

            if (currentIndex < frameLenght)
            {
                time += Time.deltaTime;
                if (time >= 1.0f / fps)
                {
					currentIndex++;
                    time = 0;
                    if (currentIndex == frameLenght)
                    {
                        currentIndex = 0;
                    }
                }
            }
        }

        public void play()
        {
            isPlaying = true;
        }

        public void stop()
        {
            isPlaying = false;
            currentIndex = 0;
            comImage.sprite = spriteArr[0];
        }

        public void pause()
        {
            isPlaying = false;
        }
    }
}
