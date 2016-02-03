using UnityEngine;
using System.Collections;
using SLua;

public sealed class GameMain : MonoBehaviour
{
    private LuaSvr _l;
    private int _progress = 0;

    public LuaSvr getLuaSvr()
    {
        return _l;
    }

    void Start()
    {
#if UNITY_5
        Application.logMessageReceived += this.log;
#else
		Application.RegisterLogCallback(this.log);
#endif
        _l = new LuaSvr();
        _l.init(tick, complete, LuaSvrFlag.LSF_BASIC);
    }

    void log(string cond, string trace, LogType lt)
    {
        Debug.Log(cond);
    }

    void tick(int p)
    {
        _progress = p;
    }

    void complete()
    {
        _l.start("Scripts/DemoLua/main");
    }
}
