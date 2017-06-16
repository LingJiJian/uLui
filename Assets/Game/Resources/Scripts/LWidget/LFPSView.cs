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
using UnityEngine.UI;
using UnityEngine.Profiling;
using System.Collections;

public class LFPSView : MonoBehaviour
{
	private static LFPSView _instance;

	public static LFPSView Show()
	{
		if (_instance == null)
		{
			GameObject obj = new GameObject();
			obj.name = "LFPSView";
			_instance = obj.AddComponent<LFPSView>();
			DontDestroyOnLoad(obj);
		}
		return _instance;
	}

	void Start()
	{
		timeleft = updateInterval;
        OnMemoryGUI = true;
	}
	void Update()
	{
		UpdateUsed();
		UpdateFPS();
	}
	//Memory
	private string sUserMemory;
	private string s;
	public bool OnMemoryGUI;
	private uint MonoUsedM;
	private uint AllMemory;
	[Range(0, 100)]
	public int MaxMonoUsedM = 50;
	[Range(0, 400)]
	public int MaxAllMemory = 200;
	void UpdateUsed()
	{
		sUserMemory = "";
		MonoUsedM = Profiler.GetMonoUsedSize() / 1000000;
		AllMemory = Profiler.GetTotalAllocatedMemory() / 1000000;


		sUserMemory += "used: " + MonoUsedM + "M ";
		sUserMemory += "all: " + AllMemory + "M ";
		sUserMemory += "left: " + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "M ";


		// s = "";
		// s += " MonoHeap:" + Profiler.GetMonoHeapSize() / 1000 + "k";
		// s += " MonoUsed:" + Profiler.GetMonoUsedSize() / 1000 + "k";
		// s += " Allocated:" + Profiler.GetTotalAllocatedMemory() / 1000 + "k";
		// s += " Reserved:" + Profiler.GetTotalReservedMemory() / 1000 + "k";
		// s += " UnusedReserved:" + Profiler.GetTotalUnusedReservedMemory() / 1000 + "k";
		// s += " UsedHeap:" + Profiler.usedHeapSize / 1000 + "k";
	}


	//FPS
	float updateInterval = 0.5f;
	private float accum = 0.0f;
	private float frames = 0;
	private float timeleft;
	private float fps;
	private string FPSAAA;
	[Range(0, 150)]
	public int MaxFPS;
	void UpdateFPS()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;


		if (timeleft <= 0.0)
		{
			fps = accum / frames;
			FPSAAA = "fps: " + fps.ToString("f2") + " ";
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
	void OnGUI()
	{
		if (OnMemoryGUI) {

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 30;

			GUI.color = new Color (1, 1, 1);
			GUI.Label (new Rect (0, 0, 200, 60), FPSAAA + sUserMemory,style);
		}
	}
}
