using UnityEngine;
using System.Collections;

public class LWindowBase : MonoBehaviour
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

