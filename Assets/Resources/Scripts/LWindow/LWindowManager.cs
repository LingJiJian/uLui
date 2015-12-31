using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum WindowHierarchy
{
    Normal,
    Message,
    Suspend
}

public enum WindowDispose
{
    Cache,
    Delay,
    Normal
}

public class LWindowManager : MonoBehaviour
{
    public delegate LWindowBase CreateWindowAction();
    public string loadPath;
    protected float recycleDuration;
    protected float disposeDuration;

    protected Dictionary<WindowHierarchy,List<LWindowBase>> runningWindows;
    protected Dictionary<WindowHierarchy, GameObject> hierarchys;
    protected Dictionary<string,CreateWindowAction> createActions;
    protected Dictionary<string, LWindowBase> cacheWindows;
    protected Dictionary<string, LWindowBase> delayDisposeWindows;
    protected Dictionary<LWindowBase, float> delayWindowsTimes;
    protected GameObject canvas;

    public LWindowManager()
    {
        loadPath = "Prefabs/{0}";

        runningWindows = new Dictionary<WindowHierarchy, List<LWindowBase>>();
        hierarchys = new Dictionary<WindowHierarchy, GameObject>();
        createActions = new Dictionary<string, CreateWindowAction>();
        cacheWindows = new Dictionary<string, LWindowBase>();
        delayDisposeWindows = new Dictionary<string, LWindowBase>();
        delayWindowsTimes = new Dictionary<LWindowBase, float>();

        recycleDuration = 2 ;
        disposeDuration = 1 ;

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
                layer.name = "Layer_"+eKey;
                layer.transform.SetParent(canvas.transform);
                layer.transform.localScale = new Vector3(1, 1, 1);

                RectTransform rtran = layer.GetComponent<RectTransform>();
                if (rtran == null)
                {
                    rtran = layer.AddComponent<RectTransform>();
                }
                rtran.pivot = new Vector2(0,0);
                rtran.anchorMin = new Vector2(0, 0);
                rtran.anchorMax = new Vector2(1, 1);
                
                hierarchys.Add((WindowHierarchy)int.Parse(eVal), layer);
            }
        }
        else
        {
            Debug.Log("can't find [Canvas]");
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
            Debug.Log(string.Format("Destroy Window [{0}]" , win.name));
        }
    }

    protected LWindowBase loadWindow(string name,Type t)
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
            GameObject res = Resources.Load(string.Format(loadPath, name)) as GameObject;
            GameObject obj = Instantiate(res) as GameObject;
            obj.name = name;
            obj.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().rect.size;
            ret = obj.GetComponent<LWindowBase>();
            if (ret == null)
            {
                ret = obj.AddComponent(t) as LWindowBase;
            }
            ret.name = name;
        }
        return ret;
    }

    public void runWindow(string name, Type type, WindowHierarchy e)
    {
        if (isRunning(name))
        {
            return;
        }
        LWindowBase win = loadWindow(name,type);

        if (win != null)
        {
            win.hierarchy = e;
            win.gameObject.transform.SetParent(hierarchys[e].transform);

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
