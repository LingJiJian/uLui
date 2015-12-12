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

        protected float time;
        protected int frameLenght;
        public Rect rect;
        protected bool isPlaying = false;
		protected int currentIndex = 0;
		protected Texture[] frameTex;

		public LFrameAnimation(){

		}

        // Use this for initialization
        void Start()
        {
			if (isPlayOnwake) {
				loadTexture ();
				play ();
			}
				
        }

		public void loadTexture()
		{
			//load textures
			Object[] texObj = Resources.LoadAll(path);
			frameTex = new Texture[texObj.Length];
			texObj.CopyTo(frameTex, 0);
			frameLenght = texObj.Length;
			
			RectTransform rtran = GetComponent<RectTransform> ();
			//rtran.pivot = Vector2.zero;
			//rtran.anchorMax = new Vector2(0, 0);
			//rtran.anchorMin = new Vector2(0, 0);
			rtran.sizeDelta = new Vector2 (frameTex [0].width, frameTex [0].height);
			refreshDrawPosition (transform.position);
		}

		public void refreshDrawPosition(Vector2 pos)
		{
			Debug.Log ("===>" + pos.x + " " + pos.y);
			rect = new Rect (pos.x,pos.y, frameTex [0].width, frameTex [0].height);
		}
        void OnGUI()
        {
            if (isPlaying)
            {
                drawAnimation();
				Debug.Log("fffbbbb");
            }
        }

        // Update is called once per frame
        protected void drawAnimation()
        {
			Debug.Log (" rect " + rect.position.ToString());
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
