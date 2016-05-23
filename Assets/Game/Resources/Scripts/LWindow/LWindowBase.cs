using UnityEngine;
using SLua;
using System.Collections;

[CustomLuaClassAttribute]
public class LWindowBase : LLuaBehaviourBase
{
    public WindowDispose disposeType;
    public WindowHierarchy hierarchy;

    public LWindowBase()
    {
        this.disposeType = WindowDispose.Delay;
    }

    public virtual void open(ArrayList list)
    {
        
    }

    public virtual void close()
    {

    }
}

