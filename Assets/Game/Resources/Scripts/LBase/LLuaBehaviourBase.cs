using UnityEngine;
using SLua;

public class LLuaBehaviourBase : MonoBehaviour {

    public string className;
    // Is ready or not.
    private bool m_bReady = false;

    // The lua behavior.
    private LLuaBehaviourInterface m_cBehavior = new LLuaBehaviourInterface();

    // The awake method.
    void Awake()
    {
        if (className == string.Empty)
        {
            Debug.LogWarning("lua class name Invalid");
            return;
        }
        // Directly creat a lua class instance to associate with this monobehavior.
        if (!CreateClassInstance(className) || !m_bReady)
        {
            return;
        }

        m_cBehavior.Awake();
    }

    // Use this for initialization
    void Start()
    {
        if (m_bReady)
        {
            m_cBehavior.Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bReady)
        {
            m_cBehavior.Update();
        }
    }

    // The destroy event.
    void OnDestroy()
    {
        if (m_bReady)
        {
            m_cBehavior.OnDestroy();
        }
    }

    void LateUpdate()
    {
        if (m_bReady)
        {
            m_cBehavior.LateUpdate();
        }
    }

    void FixedUpdate()
    {
        if (m_bReady)
        {
            m_cBehavior.FixedUpdate();
        }
    }

    void OnAnimatorIK(int nLayerIndex)
    {
        if (m_bReady)
        {
            m_cBehavior.OnAnimatorIK(nLayerIndex);
        }
    }

    void OnAnimatorMove()
    {
        if (m_bReady)
        {
            m_cBehavior.OnAnimatorMove();
        }
    }

    void OnApplicationFocus(bool bFocusStatus)
    {
        if (m_bReady)
        {
            m_cBehavior.OnApplicationFocus(bFocusStatus);
        }
    }

    void OnApplicationPause(bool bPauseStatus)
    {
        if (m_bReady)
        {
            m_cBehavior.OnApplicationPause(bPauseStatus);
        }
    }

    void OnApplicationQuit()
    {
        if (m_bReady)
        {
            m_cBehavior.OnApplicationQuit();
        }
    }

    void OnBecameInvisible()
    {
        if (m_bReady)
        {
            m_cBehavior.OnBecameInvisible();
        }
    }

    void OnBecameVisible()
    {
        if (m_bReady)
        {
            m_cBehavior.OnBecameVisible();
        }
    }

    void OnCollisionEnter(Collision cCollision)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionEnter(cCollision);
        }
    }

    void OnCollisionEnter2D(Collision2D cCollision2D)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionEnter2D(cCollision2D);
        }
    }

    void OnCollisionExit(Collision cCollisionInfo)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionExit(cCollisionInfo);
        }
    }

    void OnCollisionExit2D(Collision2D cCollision2DInfo)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionExit2D(cCollision2DInfo);
        }
    }

    void OnCollisionStay(Collision cCollisionInfo)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionStay(cCollisionInfo);
        }
    }

    void OnCollisionStay2D(Collision2D cCollision2DInfo)
    {
        if (m_bReady)
        {
            m_cBehavior.OnCollisionStay2D(cCollision2DInfo);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit cHit)
    {
        if (m_bReady)
        {
            m_cBehavior.OnControllerColliderHit(cHit);
        }
    }

    void OnDisable()
    {
        if(m_bReady)
        {
            m_cBehavior.OnDisable();
        }
    }

    void OnEnable()
    {
        if (m_bReady)
        {
            m_cBehavior.OnEnable();
        }
    }

    void OnJointBreak(float fBreakForce)
    {
        if (m_bReady)
        {
            m_cBehavior.OnJointBreak(fBreakForce);
        }
    }

    void OnLevelWasLoaded(int nLevel)
    {
        if (m_bReady)
        {
            m_cBehavior.OnLevelWasLoaded(nLevel);
        }
    }

    void OnMouseDown()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseDown();
        }
    }

    void OnMouseDrag()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseDrag();
        }
    }

    void OnMouseEnter()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseEnter();
        }
    }

    void OnMouseExit()
    {
        if(m_bReady)
        {
            m_cBehavior.OnMouseExit();
        }
    }

    void OnMouseOver()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseOver();
        }
    }

    void OnMouseUp()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseUp();
        }
    }

    void OnMouseUpAsButton()
    {
        if (m_bReady)
        {
            m_cBehavior.OnMouseUpAsButton();
        }
    }

    void OnParticleCollision(GameObject cOtherObj)
    {
        if (m_bReady)
        {
            m_cBehavior.OnParticleCollision(cOtherObj);
        }
    }

    void OnPostRender()
    {
        if (m_bReady)
        {
            m_cBehavior.OnPostRender();
        }
    }

    void OnPreCull()
    {
        if (m_bReady)
        {
            m_cBehavior.OnPreCull();
        }
    }

    void OnPreRender()
    {
        if (m_bReady)
        {
            m_cBehavior.OnPreRender();
        }
    }

    void OnRenderImage(RenderTexture cSrc, RenderTexture cDst)
    {
        if(m_bReady)
        {
            m_cBehavior.OnRenderImage(cSrc, cDst);
        }
    }

    void OnRenderObject()
    {
        if(m_bReady)
        {
            m_cBehavior.OnRenderObject();
        }
    }

    void OnTransformChildrenChanged()
    {
        if(m_bReady)
        {
            m_cBehavior.OnTransformChildrenChanged();
        }
    }

    void OnTransformParentChanged()
    {
        if (m_bReady)
        {
            m_cBehavior.OnTransformParentChanged();
        }
    }

    void OnTriggerEnter(Collider cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerEnter(cOther);
        }
    }

    void OnTriggerEnter2D(Collider2D cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerEnter2D(cOther);
        }
    }

    void OnTriggerExit(Collider cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerExit(cOther);
        }
    }

    void OnTriggerExit2D(Collider2D cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerExit2D(cOther);
        }
    }

    void OnTriggerStay(Collider cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerStay(cOther);
        }
    }

    void OnTriggerStay2D(Collider2D cOther)
    {
        if (m_bReady)
        {
            m_cBehavior.OnTriggerStay2D(cOther);
        }
    }

    void OnValidate()
    {
        if (m_bReady)
        {
            m_cBehavior.OnValidate();
        }
    }

    void OnWillRenderObject()
    {
        if(m_bReady)
        {
            m_cBehavior.OnWillRenderObject();
        }
    }

    void OnEventAnim()
    {
        if (m_bReady)
        {
            m_cBehavior.OnEventAnim();
        }
    }

    void OnEventAnimInt(int i)
    {
        if (m_bReady)
        {
            m_cBehavior.OnEventAnimInt(i);
        }
    }

    void OnEventAnimFloat(float f)
    {
        if (m_bReady)
        {
            m_cBehavior.OnEventAnimFloat(f);
        }
    }

    void OnEventAnimString(string s)
    {
        if (m_bReady)
        {
            m_cBehavior.OnEventAnimString(s);
        }
    }

    void OnEventAnimObject(object o)
    {
        if (m_bReady)
        {
            m_cBehavior.OnEventAnimObject(o);
        }
    }

    /**
     * Get the lua class instance (Actually a lua table).
     * 
     * @param void.
     * @return LuaTable - The class instance table..
     */
    public LuaTable GetInstance()
    {
        return m_cBehavior.GetChunk();
    }

    /**
     * Create a lua class instance for monobehavior instead of do a file.
     * 
     * @param string strFile - The lua class name.
     * @return bool - true if success, otherwise false.
     */
    private bool CreateClassInstance(string strClassName)
    {
        if (!m_cBehavior.CreateClassInstance(strClassName))
        {
            return false;
        }

        // Init variables.
        m_cBehavior.SetData("this", this);
        m_cBehavior.SetData("transform", transform);
        m_cBehavior.SetData("gameObject", gameObject);

        m_bReady = true;
        return true;
    }
}
