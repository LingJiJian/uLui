using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Lui
{
    public class LFrameAnimation
    {
        protected int currentIndex = 0;
        protected Texture[] frameTex;
        public float fps = 15f;
        
        protected float time;
        protected int frameLenght;
        protected Rect rect;
        protected bool isPlaying = false;

        private string _path;
        public string path
        {
           set {
               _path = value;
               //load textures
               Object[] texObj = Resources.LoadAll(_path);
               frameTex = new Texture[texObj.Length];
               texObj.CopyTo(frameTex, 0);
               frameLenght = texObj.Length;

               rect = new Rect(0, 0, frameTex[0].width, frameTex[0].height);
           }
        }

        // Use this for initialization
        void Start()
        {

        }

        public LFrameAnimation(string path)
        {
            
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
            GUI.DrawTexture(rect, frameTex[currentIndex]);
            if (currentIndex < frameLenght - 1)
            {
                time += Time.deltaTime;
                if (time >= 1.0f / fps)
                {
                    currentIndex++;
                    time = 0;
                    if (currentIndex == frameLenght - 1)
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
            GUI.DrawTexture(rect, frameTex[currentIndex]);
        }

        public void pause()
        {
            isPlaying = false;
        }
    }
}
