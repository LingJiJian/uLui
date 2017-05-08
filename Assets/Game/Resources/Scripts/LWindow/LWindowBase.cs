using UnityEngine;
using SLua;
using System.Collections;

[CustomLuaClass]
public class LWindowBase : LLuaBehaviourBase
{
    public WindowDispose disposeType;
    public WindowHierarchy hierarchy;

    public LWindowBase()
    {
        this.disposeType = WindowDispose.Delay;
    }

    [DoNotToLua]
    public virtual void Open(object[] list)
    {
        if (m_bReady)
        {
            m_cBehavior.OnWindowOpen(list);
        }
    }

    [DoNotToLua]
    public virtual void Close()
    {
        if (m_bReady)
        {
            m_cBehavior.OnWindowClose();
        }
    }
}

