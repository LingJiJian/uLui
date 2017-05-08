using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using SLua;

[CustomLuaClass]
public enum WindowHierarchy
{
    Normal,
    Message,
    Suspend
}

[CustomLuaClass]
public enum WindowDispose
{
    Cache,
    Delay,
    Normal
}

/// <summary>
/// 窗体管理
/// </summary>
[CustomLuaClass]
public class LWindowManager : MonoBehaviour

{
    protected float recycleDuration;
    protected float disposeDuration;

    protected Dictionary<WindowHierarchy, List<LWindowBase>> runningWindows;
    protected Dictionary<WindowHierarchy, GameObject> hierarchys;
    protected Dictionary<string, LWindowBase> cacheWindows;
    protected Dictionary<string, LWindowBase> delayDisposeWindows;
    protected Dictionary<LWindowBase, float> delayWindowsTimes;
    public GameObject canvas { get; private set; }
    private static LWindowManager _instance;
    private AsyncOperation _sceneAsync;
    private uint _nowProcess;
    public UnityAction<float> onProgressAsyncScene;

    public LWindowManager()
    {
        runningWindows = new Dictionary<WindowHierarchy, List<LWindowBase>>();
        hierarchys = new Dictionary<WindowHierarchy, GameObject>();
        cacheWindows = new Dictionary<string, LWindowBase>();
        delayDisposeWindows = new Dictionary<string, LWindowBase>();
        delayWindowsTimes = new Dictionary<LWindowBase, float>();

        recycleDuration = 2;
        disposeDuration = 60;

        foreach (int item in Enum.GetValues(typeof(WindowHierarchy)))
        {
            string eVal = item.ToString();
            runningWindows.Add((WindowHierarchy)int.Parse(eVal), new List<LWindowBase>());
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    public static LWindowManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "LWindowManager";
            _instance = obj.AddComponent<LWindowManager>();
        }
        return _instance;
    }

    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            foreach (int item in Enum.GetValues(typeof(WindowHierarchy)))
            {
                string eKey = Enum.GetName(typeof(WindowHierarchy), item);
                string eVal = item.ToString();

                GameObject layer = new GameObject();
                layer.name = "Layer_" + eKey;
                layer.transform.SetParent(canvas.transform);
                layer.transform.localScale = new Vector3(1, 1, 1);
                layer.transform.localPosition = Vector3.zero;

                RectTransform rtran = layer.GetComponent<RectTransform>();
                if (rtran == null)
                {
                    rtran = layer.AddComponent<RectTransform>();
                }
                rtran.pivot = new Vector2(0.5f, 0.5f);
                rtran.anchorMin = new Vector2(0, 0);
                rtran.anchorMax = new Vector2(1, 1);
                rtran.offsetMax = new Vector2(0, 0);
                rtran.offsetMin = new Vector2(0, 0);

                hierarchys.Add((WindowHierarchy)int.Parse(eVal), layer);
            }
        }
        else
        {
            Debug.LogWarning("can't find [Canvas]");
        }

        InvokeRepeating("checkRecycle", 0, recycleDuration);
    }

    protected void checkRecycle()
    {
        List<LWindowBase> recycles = new List<LWindowBase>();
        foreach (var item in delayDisposeWindows)
        {
            if (!isRunning(item.Value.name) && Time.time - delayWindowsTimes[item.Value] >= disposeDuration)
            {
                recycles.Add(item.Value);
            }
        }
        while (true)
        {
            if (recycles.Count == 0)
                break;

            LWindowBase win = recycles[0];
            delayWindowsTimes.Remove(win);
            delayDisposeWindows.Remove(win.name);
            recycles.RemoveAt(0);
            Destroy(win.gameObject);
            Debug.Log(string.Format("Destroy Window [{0}]", win.name));
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(string name, UnityAction<float> onProgressFunc)
    {
        StartCoroutine(onLoadSceneAsync(name, onProgressFunc));
    }

    void Update()
    {
        if (_sceneAsync != null)
        {
            uint progress;
            if (_sceneAsync.progress < 0.9f)
            {
                progress = (uint)(_sceneAsync.progress * 100);
            }
            else
            {
                progress = 100;
            }
            if(_nowProcess < progress)
                _nowProcess++;
            if (onProgressAsyncScene != null)
                onProgressAsyncScene.Invoke(_nowProcess);
            if(_nowProcess == 100)
                _sceneAsync.allowSceneActivation = true;
        }
    }

    private IEnumerator onLoadSceneAsync(string name, UnityAction<float> onProgressFunc)
    {
        _nowProcess = 0;
        onProgressAsyncScene = onProgressFunc;
        _sceneAsync = SceneManager.LoadSceneAsync(name);
        _sceneAsync.allowSceneActivation = false;

        yield return _sceneAsync;
    }

    protected LWindowBase loadWindow(string name)
    {
        LWindowBase ret = null;
        if (cacheWindows.ContainsKey(name))
        {
            ret = cacheWindows[name];
        }
        else if (delayDisposeWindows.ContainsKey(name))
        {
            ret = delayDisposeWindows[name];
        }
        else
        {
			string abName = LGameConfig.GetABNameWithAtlasPath (name);
			GameObject res = LLoadBundle.GetInstance().LoadAsset<GameObject>(abName, name);
            GameObject obj = Instantiate(res);

            obj.name = name;
            obj.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().rect.size;
            ret = obj.GetComponent<LWindowBase>();
            if (ret) ret.name = obj.name;
        }
        return ret;
    }

    public void runWindow(string name, WindowHierarchy e, object[] list = null)
    {
        if (isRunning(name))
        {
            return;
        }
        LWindowBase win = loadWindow(name);

        if (win != null)
        {
            win.hierarchy = e;
            win.gameObject.transform.SetParent(hierarchys[e].transform);
            win.gameObject.transform.localScale = new Vector3(1, 1,1);
            win.gameObject.transform.localPosition = Vector3.zero;
            win.Open(list);

            runningWindows[e].Add(win);

            if (win.disposeType == WindowDispose.Delay)
            {
                delayDisposeWindows[name] = win;
                delayWindowsTimes[win] = Time.time;
            }
            else if (win.disposeType == WindowDispose.Cache)
            {
                cacheWindows.Add(name, win);
            }
        }
    }

    public LWindowBase seekWindow(string name)
    {
        LWindowBase ret = null;
        foreach (var item in runningWindows)
        {
            foreach (var iitem in item.Value)
            {
                if (iitem.name == name)
                {
                    ret = iitem;
                    break;
                }
            }
        }
        return ret;
    }

    public void popWindow(string name)
    {
        LWindowBase win = seekWindow(name);
        popWindow(win);
    }

    public void popWindow(LWindowBase win)
    {
        if (win != null)
        {
            runningWindows[win.hierarchy].Remove(win);
            win.Close();

            if (win.disposeType == WindowDispose.Cache)
            {
                win.gameObject.transform.SetParent(null);
            }
            else if (win.disposeType == WindowDispose.Normal)
            {
                cacheWindows.Remove(win.name);
                delayDisposeWindows.Remove(win.name);
                delayWindowsTimes.Remove(win);
                Destroy(win.gameObject);
                Debug.Log(string.Format("Destroy Window [{0}]", win.name));
            }
            else if (win.disposeType == WindowDispose.Delay)
            {
                win.gameObject.transform.SetParent(null);
            }
        }
    }

    public void popAllWindow()
    {
        foreach (int item in Enum.GetValues(typeof(WindowHierarchy)))
        {
            string eVal = item.ToString();
            popAllWindow((WindowHierarchy)int.Parse(eVal));
        }
    }

    public void popAllWindow(WindowHierarchy e)
    {
        while (true)
        {
            if (runningWindows[e].Count == 0)
                break;
            popWindow(runningWindows[e][0]);
        }
    }

    public void removeCachedWindow(string name)
    {
        popWindow(name);
        cacheWindows.Remove(name);
    }

    public void removeAllCachedWindow()
    {
        foreach (var item in cacheWindows)
        {
            popWindow(item.Value);
        }
        cacheWindows.Clear();
    }

    public bool isRunning(string name)
    {
        bool ret = false;
        foreach (var item in runningWindows)
        {
            foreach (var iitem in item.Value)
            {
                if (iitem.name == name)
                {
                    ret = true;
                    break;
                }
            }
        }
        return ret;
    }
}
