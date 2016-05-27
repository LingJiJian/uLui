using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LFPSView : MonoBehaviour {

    private static LFPSView _instance;
    private GameObject _canvas;
    private Text _labelText;
    public float _updateInterval = 0.5f;//设定更新帧率的时间间隔为0.5秒  
    float _accum = .0f;//累积时间  
    int _frames = 0;//在_updateInterval时间内运行了多少帧  
    float _timeLeft;

    void init()
    {
        _canvas = GameObject.Find("Canvas");

        if (!_canvas)
        {
            Debug.LogWarning("can't find [Canvas]");
            return;
        }

        GameObject _labelObject = new GameObject();
        _labelObject.name = "LFPSView";
        _labelObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
        RectTransform rtran = _labelObject.GetComponent<RectTransform>();
        if (!rtran)
        {
            rtran = _labelObject.AddComponent<RectTransform>();
        }
        _labelText = _labelObject.AddComponent<Text>();
        _labelText.font = Font.CreateDynamicFontFromOSFont("Arial",15);
        rtran.pivot = new Vector2(0, 1);
        rtran.anchorMax = new Vector2(0, 1);
        rtran.anchorMin = new Vector2(0, 1);

        _labelObject.transform.SetParent(_canvas.transform);
        _labelObject.transform.position = new Vector3(0, 15, 0);

        _timeLeft = _updateInterval;
    }

    void Start()
    {
        init();
    }

    void OnLevelWasLoaded(int level)
    {
        init();
    }

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

    // Update is called once per frame  
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        //Time.timeScale可以控制Update 和LateUpdate 的执行速度,  
        //Time.deltaTime是以秒计算，完成最后一帧的时间  
        //相除即可得到相应的一帧所用的时间  
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;//帧数  

        if (_timeLeft <= 0)
        {
            float fps = _accum / _frames;
            //Debug.Log(_accum + "__" + _frames);  
            string fpsFormat = System.String.Format("{0:F2} FPS", fps);//保留两位小数  
            _labelText.text = fpsFormat;

            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }
    }
}
