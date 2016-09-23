using UnityEngine;
using SLua;
using System.Collections;

// The lua behavior base class.
public class LLuaBehaviourInterface
{
	// The callback method name.
	private static readonly string AWAKE = "Awake";
	private static readonly string LATE_UPDATE = "LateUpdate";
	private static readonly string FIXED_UPDATE = "FixedUpdate";
	private static readonly string ON_ANIMATOR_IK = "OnAnimatorIK";
	private static readonly string ON_ANIMATOR_MOVE = "OnAnimatorMove";
	private static readonly string ON_APPLICATION_FOCUS = "OnApplicationFocus";
	private static readonly string ON_APPLICATION_PAUSE = "OnApplicationPause";
	private static readonly string ON_APPLICATION_QUIT = "OnApplicationQuit";
	//private static readonly string ON_AUDIO_FILTER_READ = "OnAudioFilterRead";	// Skip.
	private static readonly string ON_BECAME_INVISIBLE = "OnBecameInvisible";
	private static readonly string ON_BECAME_VISIBLE = "OnBecameVisible";
	private static readonly string ON_COLLISION_ENTER = "OnCollisionEnter";
	private static readonly string ON_COLLISION_ENTER_2D = "OnCollisionEnter2D";
	private static readonly string ON_COLLISION_EXIT = "OnCollisionExit";
	private static readonly string ON_COLLISION_EXIT_2D = "OnCollisionExit2D";
	private static readonly string ON_COLLISION_STAY = "OnCollisionStay";
	private static readonly string ON_COLLISION_STAY_2D = "OnCollisionStay2D";
	//private static readonly string ON_CONNECTED_SERVER = "OnConnectedToServer";	// Skip.
	private static readonly string ON_CONTROLLER_COLLIDER_HIT = "OnControllerColliderHit";
	private static readonly string ON_DESTROY = "OnDestroy";
	private static readonly string ON_DISABLE = "OnDisable";
	//private static readonly string ON_DISCONNECTED_FROM_SERVER = "OnDisconnectedFromServer";	// Skip.
	//private static readonly string ON_DRAW_GIZMOS = "OnDrawGizmos";	// Skip.
	//private static readonly string ON_DRAW_GIZMOS_SELECTED = "OnDrawGizmosSelected";	// Skip.
	private static readonly string ON_ENABLE = "OnEnable";
	//private static readonly string ON_FAILED_TO_CONNECT = "OnFailedToConnect";	// Skip.
	//private static readonly string ON_FAILED_TO_CONNECT_MASTER_SERVER = "OnFailedToConnectToMasterServer";	// Skip
	//private static readonly string ON_GUI = "OnGUI";	// Skip.
	private static readonly string ON_JOINT_BREAK = "OnJointBreak";
	private static readonly string ON_LEVEL_WAS_LOADED = "OnLevelWasLoaded";
	//private static readonly string ON_MASTER_SERVER_EVENT = "OnMasterServerEvent";	// Skip.
	private static readonly string ON_MOUSE_DOWN = "OnMouseDown";
	private static readonly string ON_MOUSE_DRAG = "OnMouseDrag";
	private static readonly string ON_MOUSE_ENTER = "OnMouseEnter";
	private static readonly string ON_MOUSE_EXIT = "OnMouseExit";
	private static readonly string ON_MOUSE_OVER = "OnMouseOver";
	private static readonly string ON_MOUSE_UP = "OnMouseUp";
	private static readonly string ON_MOUSE_UP_AS_BUTTON = "OnMouseUpAsButton";
	//private static readonly string ON_NETWORK_INSTANTIATE = "OnNetworkInstantiate";	// Skip.
	private static readonly string ON_PARTICLE_COLLISION = "OnParticleCollision";
	//private static readonly string ON_PLAYER_CONNECTED = "OnPlayerConnected";	// Skip.
	//private static readonly string ON_PLAYER_DISCONNECTED = "OnPlayerDisconnected";	// Skip.
	private static readonly string ON_POST_RENDER = "OnPostRender";
	private static readonly string ON_PRE_CULL = "OnPreCull";
	private static readonly string ON_PRE_RENDER = "OnPreRender";
	private static readonly string ON_RENDER_IMAGE = "OnRenderImage";
	private static readonly string ON_RENDER_OBJECT = "OnRenderObject";
	//private static readonly string ON_SERIALIZE_NETWORK_VIEW = "OnSerializeNetworkView";	// Skip.
	//private static readonly string ON_SERVER_INITIALIZED = "OnServerInitialized";	// Skip.
	private static readonly string ON_TRANSFORM_CHILDREN_CHANGED = "OnTransformChildrenChanged";
	private static readonly string ON_TRANSFORM_PARENT_CHANGED = "OnTransformParentChanged";
	private static readonly string ON_TRIGGER_ENTER = "OnTriggerEnter";
	private static readonly string ON_TRIGGER_ENTER_2D = "OnTriggerEnter2D";
	private static readonly string ON_TRIGGER_EXIT = "OnTriggerExit";
	private static readonly string ON_TRIGGER_EXIT_2D = "OnTriggerExit2D";
	private static readonly string ON_TRIGGER_STAY = "OnTriggerStay";
	private static readonly string ON_TRIGGER_STAY_2D = "OnTriggerStay2D";
	private static readonly string ON_VALIDATE = "OnValidate";
	private static readonly string ON_WILL_RENDER_OBJECT = "OnWillRenderObject";
	//private static readonly string RESET = "Reset";	// Skip.
	private static readonly string START = "Start";
	private static readonly string UPDATE = "Update";
    //custom
    private static readonly string ON_EVENT_ANIM = "OnEventAnim";
    private static readonly string ON_EVENT_ANIM_INT = "OnEventAnimInt";
    private static readonly string ON_EVENT_ANIM_FLOAT = "OnEventAnimFloat";
    private static readonly string ON_EVENT_ANIM_STRING = "OnEventAnimString";
    private static readonly string ON_EVENT_ANIM_OBJECT = "OnEventAnimObject";

    // The function for monobehavior callback event.
    private LuaFunction m_cAwakeFunc = null;
	private LuaFunction m_cLateUpdateFunc = null;
	private LuaFunction m_cFixedUpdateFunc = null;
	private LuaFunction m_cOnAnimatorIKFunc = null;
	private LuaFunction m_cOnAnimatorMoveFunc = null;
	private LuaFunction m_cOnApplicationFocusFunc = null;
	private LuaFunction m_cOnApplicationPauseFunc = null;
	private LuaFunction m_cOnApplicationQuitFunc = null;
	private LuaFunction m_cOnBecameInvisibleFunc = null;
	private LuaFunction m_cOnBecameVisibleFunc = null;
	private LuaFunction m_cOnCollisionEnterFunc = null;
	private LuaFunction m_cOnCollisionEnter2DFunc = null;
	private LuaFunction m_cOnCollisionExitFunc = null;
	private LuaFunction m_cOnCollisionExit2DFunc = null;
	private LuaFunction m_cOnCollisionStayFunc = null;
	private LuaFunction m_cOnCollisionStay2DFunc = null;
	private LuaFunction m_cOnControllerColliderHitFunc = null;
	private LuaFunction m_cOnDestroy = null;
	private LuaFunction m_cOnDisableFunc = null;
	private LuaFunction m_cOnEnableFunc = null;
	private LuaFunction m_cOnJointBreakFunc = null;
	private LuaFunction m_cOnLevelWasLoadedFunc = null;
	private LuaFunction m_cOnMouseDownFunc = null;
	private LuaFunction m_cOnMouseDragFunc = null;
	private LuaFunction m_cOnMouseEnterFunc = null;
	private LuaFunction m_cOnMouseExitFunc = null;
	private LuaFunction m_cOnMouseOverFunc = null;
	private LuaFunction m_cOnMouseUpFunc = null;
	private LuaFunction m_cOnMouseUpAsButtonFunc = null;
	private LuaFunction m_cOnParticleCollisionFunc = null;
	private LuaFunction m_cOnPostRenderFunc = null;
	private LuaFunction m_cOnPreCullFunc = null;
	private LuaFunction m_cOnPreRenderFunc = null;
	private LuaFunction m_cOnRenderImageFunc = null;
	private LuaFunction m_cOnRenderObjectFunc = null;
	private LuaFunction m_cOnTransformChildrenChangedFunc = null;
	private LuaFunction m_cOnTransformParentChangedFunc = null;
	private LuaFunction m_cOnTriggerEnterFunc = null;
	private LuaFunction m_cOnTriggerEnter2DFunc = null;
	private LuaFunction m_cOnTriggerExitFunc = null;
	private LuaFunction m_cOnTriggerExit2DFunc = null;
	private LuaFunction m_cOnTriggerStayFunc = null;
	private LuaFunction m_cOnTriggerStay2DFunc = null;
	private LuaFunction m_cOnValidateFunc = null;
	private LuaFunction m_cOnWillRenderObjectFunc = null;
	private LuaFunction m_cStartFunc = null;
	private LuaFunction m_cUpdateFunc = null;
    //custom
    private LuaFunction m_cOnEventAnim = null;
    private LuaFunction m_cOnEventAnimInt = null;
    private LuaFunction m_cOnEventAnimFloat = null;
    private LuaFunction m_cOnEventAnimString = null;
    private LuaFunction m_cOnEventAnimObject = null;

    // The lua table operator of this behavior.
    private LLuaTable m_cLuaTableOpt = null;

	/**
     * Constructor.
     * 
     * @param void.
     * @return void.
     */
	public LLuaBehaviourInterface()
	{
	}

	/**
     * Destructor.
     * 
     * @param void.
     * @return void.
     */
	~LLuaBehaviourInterface()
	{
	}

	/**
     * Awake method.
     * 
     * @param void.
     * @return void.
     */
	public void Awake()
	{
		CallMethod(ref m_cAwakeFunc, AWAKE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Late update method.
     * 
     * @param void.
     * @return void.
     */
	public void LateUpdate()
	{
		CallMethod(ref m_cLateUpdateFunc, LATE_UPDATE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Fixed update method.
     * 
     * @param void.
     * @return void.
     */
	public void FixedUpdate()
	{
		CallMethod(ref m_cFixedUpdateFunc, FIXED_UPDATE, m_cLuaTableOpt.GetChunk());
	}


	/**
     * Callback for setting up animation IK (inverse kinematics).
     * 
     * @param int nLayerIndex - The layer index.
     * @return void.
     */
	public void OnAnimatorIK(int nLayerIndex)
	{
		CallMethod(ref m_cOnAnimatorIKFunc, ON_ANIMATOR_IK, m_cLuaTableOpt.GetChunk(), nLayerIndex);
	}

	/**
     * Callback for processing animation movements for modifying root motion.
     * 
     * @param void.
     * @return void.
     */
	public void OnAnimatorMove()
	{
		CallMethod(ref m_cOnAnimatorMoveFunc, ON_ANIMATOR_MOVE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Sent to all game objects when the player gets or loses focus.
     * 
     * @param bool bFocusStatus - The focus status.
     * @return void.
     */
	public void OnApplicationFocus(bool bFocusStatus)
	{
		CallMethod(ref m_cOnApplicationFocusFunc, ON_APPLICATION_FOCUS, m_cLuaTableOpt.GetChunk(), bFocusStatus);
	}

	/**
     * Sent to all game objects when the player pauses.
     * 
     * @param bool bPauseStatus - The pause status.
     * @return void.
     */
	public void OnApplicationPause(bool bPauseStatus)
	{
		CallMethod(ref m_cOnApplicationPauseFunc, ON_APPLICATION_PAUSE, m_cLuaTableOpt.GetChunk(), bPauseStatus);
	}

	/**
     * Sent to all game objects before the application is quit.
     * 
     * @param void.
     * @return void.
     */
	public void OnApplicationQuit()
	{
		CallMethod(ref m_cOnApplicationQuitFunc, ON_APPLICATION_QUIT, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnBecameInvisible is called when the renderer is no longer visible by any camera.
     * 
     * @param void.
     * @return void.
     */
	public void OnBecameInvisible()
	{
		CallMethod(ref m_cOnBecameInvisibleFunc, ON_BECAME_INVISIBLE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnBecameVisible is called when the renderer became visible by any camera.
     * 
     * @param void.
     * @return void.
     */
	public void OnBecameVisible()
	{
		CallMethod(ref m_cOnBecameVisibleFunc, ON_BECAME_VISIBLE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
     * 
     * @param Collision cCollision - The collison.
     * @return void.
     */
	public void OnCollisionEnter(Collision cCollision)
	{
		CallMethod(ref m_cOnCollisionEnterFunc, ON_COLLISION_ENTER, m_cLuaTableOpt.GetChunk(), cCollision);
	}

	/**
     * Sent when an incoming collider makes contact with this object's collider (2D physics only).
     * 
     * @param Collision2D cCollision2D - The collison for 2d.
     * @return void.
     */
	public void OnCollisionEnter2D(Collision2D cCollision2D)
	{
		CallMethod(ref m_cOnCollisionEnter2DFunc, ON_COLLISION_ENTER_2D, m_cLuaTableOpt.GetChunk(), cCollision2D);
	}

	/**
     * OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
     * 
     * @param Collision2D cCollisionInfo - The collison info.
     * @return void.
     */
	public void OnCollisionExit(Collision cCollisionInfo)
	{
		CallMethod(ref m_cOnCollisionExitFunc, ON_COLLISION_EXIT, m_cLuaTableOpt.GetChunk(), cCollisionInfo);
	}

	/**
     * Sent when a collider on another object stops touching this object's collider (2D physics only).
     * 
     * @param Collision2D cCollision2DInfo - The collison info for 2d.
     * @return void.
     */
	public void OnCollisionExit2D(Collision2D cCollision2DInfo)
	{
		CallMethod(ref m_cOnCollisionExit2DFunc, ON_COLLISION_EXIT_2D, m_cLuaTableOpt.GetChunk(), cCollision2DInfo);
	}

	/**
     * OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
     * 
     * @param Collision cCollisionInfo - The collison info.
     * @return void.
     */
	public void OnCollisionStay(Collision cCollisionInfo)
	{
		CallMethod(ref m_cOnCollisionStayFunc, ON_COLLISION_STAY, m_cLuaTableOpt.GetChunk(), cCollisionInfo);
	}

	/**
     * Sent each frame where a collider on another object is touching this object's collider (2D physics only).
     * 
     * @param Collision2D cCollision2DInfo - The collison info for 2d.
     * @return void.
     */
	public void OnCollisionStay2D(Collision2D cCollision2DInfo)
	{
		CallMethod(ref m_cOnCollisionStay2DFunc, ON_COLLISION_STAY_2D, m_cLuaTableOpt.GetChunk(), cCollision2DInfo);
	}

	/**
     * OnControllerColliderHit is called when the controller hits a collider while performing a Move.
     * 
     * @param ControllerColliderHit cHit - The hit info.
     * @return void.
     */
	public void OnControllerColliderHit(ControllerColliderHit cHit)
	{
		CallMethod(ref m_cOnControllerColliderHitFunc, ON_CONTROLLER_COLLIDER_HIT, m_cLuaTableOpt.GetChunk(), cHit);
	}

	/**
     * On destroy method.
     * 
     * @param void.
     * @return void.
     */
	public void OnDestroy()
	{
		CallMethod(ref m_cOnDestroy, ON_DESTROY, m_cLuaTableOpt.GetChunk());
	}

	/**
     * This function is called when the behaviour becomes disabled () or inactive.
     * 
     * @param void.
     * @return void.
     */
	public void OnDisable()
	{
		CallMethod(ref m_cOnDisableFunc, ON_DISABLE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * This function is called when the object becomes enabled and active.
     * 
     * @param void.
     * @return void.
     */
	public void OnEnable()
	{
		CallMethod(ref m_cOnEnableFunc, ON_ENABLE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Called when a joint attached to the same game object broke.
     * 
     * @param float fBreakForce - The break force.
     * @return void.
     */
	public void OnJointBreak(float fBreakForce)
	{
		CallMethod(ref m_cOnJointBreakFunc, ON_JOINT_BREAK, m_cLuaTableOpt.GetChunk(), fBreakForce);
	}

	/**
     * This function is called after a new level was loaded.
     * 
     * @param int nLevel - The loaded level.
     * @return void.
     */
	public void OnLevelWasLoaded(int nLevel)
	{
		CallMethod(ref m_cOnLevelWasLoadedFunc, ON_LEVEL_WAS_LOADED, m_cLuaTableOpt.GetChunk(), nLevel);
	}

	/**
     * OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseDown()
	{
		CallMethod(ref m_cOnMouseDownFunc, ON_MOUSE_DOWN, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseDrag()
	{
		CallMethod(ref m_cOnMouseDragFunc, ON_MOUSE_DRAG, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Called when the mouse enters the GUIElement or Collider.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseEnter()
	{
		CallMethod(ref m_cOnMouseEnterFunc, ON_MOUSE_ENTER, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Called when the mouse is not any longer over the GUIElement or Collider.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseExit()
	{
		CallMethod(ref m_cOnMouseExitFunc, ON_MOUSE_EXIT, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Called every frame while the mouse is over the GUIElement or Collider.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseOver()
	{
		CallMethod(ref m_cOnMouseOverFunc, ON_MOUSE_OVER, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnMouseUp is called when the user has released the mouse button.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseUp()
	{
		CallMethod(ref m_cOnMouseUpFunc, ON_MOUSE_UP, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.
     * 
     * @param void.
     * @return void.
     */
	public void OnMouseUpAsButton()
	{
		CallMethod(ref m_cOnMouseUpAsButtonFunc, ON_MOUSE_UP_AS_BUTTON, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnParticleCollision is called when a particle hits a collider.
     * This can be used to apply damage to a game object when hit by particles.
     * 
     * @param GameObject cOtherObj - The other particle game object.
     * @return void.
     */
	public void OnParticleCollision(GameObject cOtherObj)
	{
		CallMethod(ref m_cOnParticleCollisionFunc, ON_PARTICLE_COLLISION, m_cLuaTableOpt.GetChunk(), cOtherObj);
	}

	/**
     * OnPostRender is called after a camera finished rendering the scene.
     * 
     * @param void.
     * @return void.
     */
	public void OnPostRender()
	{
		CallMethod(ref m_cOnPostRenderFunc, ON_POST_RENDER, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnPreCull is called before a camera culls the scene.
     * 
     * @param void.
     * @return void.
     */
	public void OnPreCull()
	{
		CallMethod(ref m_cOnPreCullFunc, ON_PRE_CULL, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnPreRender is called before a camera starts rendering the scene.
     * 
     * @param void.
     * @return void.
     */
	public void OnPreRender()
	{
		CallMethod(ref m_cOnPreRenderFunc, ON_PRE_RENDER, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnRenderImage is called after all rendering is complete to render image.
     * 
     * @param RenderTexture cSrc - The source render texture.
     * @param RenderTexture cDst - The destination render texture.
     * @return void.
     */
	public void OnRenderImage(RenderTexture cSrc, RenderTexture cDst)
	{
		CallMethod(ref m_cOnRenderImageFunc, ON_RENDER_IMAGE, m_cLuaTableOpt.GetChunk(), cSrc, cDst);
	}

	/**
     * OnRenderObject is called after camera has rendered the scene.
     * 
     * @param void.
     * @return void.
     */
	public void OnRenderObject()
	{
		CallMethod(ref m_cOnRenderObjectFunc, ON_RENDER_OBJECT, m_cLuaTableOpt.GetChunk());
	}

	/**
     * This function is called when the list of children of the transform of the GameObject has changed.
     * 
     * @param void.
     * @return void.
     */
	public void OnTransformChildrenChanged()
	{
		CallMethod(ref m_cOnTransformChildrenChangedFunc, ON_TRANSFORM_CHILDREN_CHANGED, m_cLuaTableOpt.GetChunk());
	}

	/**
     * This function is called when the parent property of the transform of the GameObject has changed.
     * 
     * @param void.
     * @return void.
     */
	public void OnTransformParentChanged()
	{
		CallMethod(ref m_cOnTransformParentChangedFunc, ON_TRANSFORM_PARENT_CHANGED, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnTriggerEnter is called when the Collider other enters the trigger.
     * 
     * @param Collider cOther - The other collider.
     * @return void.
     */
	public void OnTriggerEnter(Collider cOther)
	{
		CallMethod(ref m_cOnTriggerEnterFunc, ON_TRIGGER_ENTER, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * Sent when another object enters a trigger collider attached to this object (2D physics only).
     * 
     * @param Collider2D cOther - The other collider for 2d.
     * @return void.
     */
	public void OnTriggerEnter2D(Collider2D cOther)
	{
		CallMethod(ref m_cOnTriggerEnter2DFunc, ON_TRIGGER_ENTER_2D, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * OnTriggerExit is called when the Collider other has stopped touching the trigger.
     * 
     * @param Collider cOther - The other collider.
     * @return void.
     */
	public void OnTriggerExit(Collider cOther)
	{
		CallMethod(ref m_cOnTriggerExitFunc, ON_TRIGGER_EXIT, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * Sent when another object leaves a trigger collider attached to this object (2D physics only).
     * 
     * @param Collider2D cOther - The other collider for 2d.
     * @return void.
     */
	public void OnTriggerExit2D(Collider2D cOther)
	{
		CallMethod(ref m_cOnTriggerExit2DFunc, ON_TRIGGER_EXIT_2D, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
     * 
     * @param Collider2D cOther - The other collider.
     * @return void.
     */
	public void OnTriggerStay(Collider cOther)
	{
		CallMethod(ref m_cOnTriggerStayFunc, ON_TRIGGER_STAY, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * Sent each frame where another object is within a trigger collider attached to this object (2D physics only).
     * 
     * @param Collider2D cOther - The other collider for 2d.
     * @return void.
     */
	public void OnTriggerStay2D(Collider2D cOther)
	{
		CallMethod(ref m_cOnTriggerStay2DFunc, ON_TRIGGER_STAY_2D, m_cLuaTableOpt.GetChunk(), cOther);
	}

	/**
     * This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
     * 
     * @param void.
     * @return void.
     */
	public void OnValidate()
	{
		CallMethod(ref m_cOnValidateFunc, ON_VALIDATE, m_cLuaTableOpt.GetChunk());
	}

	/**
     * OnWillRenderObject is called once for each camera if the object is visible.
     * 
     * @param void.
     * @return void.
     */
	public void OnWillRenderObject()
	{
		CallMethod(ref m_cOnWillRenderObjectFunc, ON_WILL_RENDER_OBJECT, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Start method, this is the main entry.
     * 
     * @param void.
     * @return void.
     */
	public void Start()
	{
		CallMethod(ref m_cStartFunc, START, m_cLuaTableOpt.GetChunk());
	}

	/**
     * Update method.
     * 
     * @param void.
     * @return void.
     */
	public void Update()
	{
		CallMethod(ref m_cUpdateFunc, UPDATE, m_cLuaTableOpt.GetChunk());
	}

    public void OnEventAnim()
    {
        CallMethod(ref m_cOnEventAnim, ON_EVENT_ANIM, m_cLuaTableOpt.GetChunk());
    }

    public void OnEventAnimInt(int i)
    {
        CallMethod(ref m_cOnEventAnimInt, ON_EVENT_ANIM_INT, m_cLuaTableOpt.GetChunk(),i);
    }

    public void OnEventAnimFloat(float f)
    {
        CallMethod(ref m_cOnEventAnimFloat, ON_EVENT_ANIM_FLOAT, m_cLuaTableOpt.GetChunk(),f);
    }

    public void OnEventAnimString(string s)
    {
        CallMethod(ref m_cOnEventAnimString, ON_EVENT_ANIM_STRING, m_cLuaTableOpt.GetChunk(),s);
    }

    public void OnEventAnimObject(object o)
    {
        CallMethod(ref m_cOnEventAnimObject, ON_EVENT_ANIM_OBJECT, m_cLuaTableOpt.GetChunk(),o);
    }

    /**
     * Load an lua file.
     * 
     * @param string strFile - The file name without extension.
     * @return bool - true if success, otherwise false.
     */
    public bool DoFile(string strFile)
	{
		if (string.IsNullOrEmpty(strFile))
		{
			return false;
		}

		// Try to do file.
		try
		{
			// The lua file return the table itself.
			object cChunk = Game.GetLuaSvr().luaState.doFile(strFile);
			if ((null == cChunk) || !(cChunk is LuaTable))
			{
				return false;
			}

			// Remember lua table.
			m_cLuaTableOpt = new LLuaTable((LuaTable)cChunk);
			return true;
		}
		catch (System.Exception e)
		{
			Debug.LogError(LUtil.FormatException(e));
		}

		return false;
	}

	/**
     * Create a lua class instance for monobehavior instead of do a file.
     * 
     * @param string strFile - The lua class name.
     * @return bool - true if success, otherwise false.
     */
	public bool CreateClassInstance(string strClassName)
	{
		if (string.IsNullOrEmpty(strClassName))
		{
			return false;
		}

		// Try to get global lua class.
		try
		{
			// Get class first.
			LuaTable cClsTable = (LuaTable)Game.GetLuaSvr().luaState[strClassName];
			if (null == cClsTable)
			{
				return false;
			}

			// Get "new" method of the lua class to create instance.
			LuaFunction cNew = (LuaFunction)cClsTable["new"];
			if (null == cNew)
			{
				return false;
			}

			// We choose no default init parameter for constructor.
			object cInsChunk = cNew.call();
			if (null == cInsChunk)
			{
				return false;
			}

			// If we create instance ok, use it as table.
			m_cLuaTableOpt = new LLuaTable((LuaTable)cInsChunk);
			return true;
		}
		catch (System.Exception e)
		{
			Debug.LogError(LUtil.FormatException(e));
		}

		return false;
	}

	/**
     * Get the lua code chunk holder (lua table).
     * 
     * @param void.
     * @return YwLuaTable - The chunk table holder.
     */
	public LLuaTable GetChunkHolder()
	{
		return m_cLuaTableOpt;
	}

	/**
     * Get the lua code chunk (table).
     * 
     * @param void.
     * @return LuaTable - The chunk table.
     */
	public LuaTable GetChunk()
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.GetChunk();
	}

	/**
     * Set lua data to a lua table, used to communiate with other lua files.
     * 
     * @param string strName - The key name of the table.
     * @param object cValue - The value associated to the key.
     * @return void.
     */
	public void SetData(string strName, object cValue)
	{
		if (null == m_cLuaTableOpt)
		{
			return;
		}

		m_cLuaTableOpt.SetData(strName, cValue);
	}

	/**
     * Set lua data to a lua table, used to communiate with other lua files.
     * This is used to set an array data to an sub-table.
     * 
     * @param string strName - The key name of the sub-table.
     * @param object cValue - The value associated to the key.
     * @return void.
     */
	public void SetData(string strName, object[] cArrayValue)
	{
		if (null == m_cLuaTableOpt)
		{
			return;
		}

		m_cLuaTableOpt.SetData(strName, cArrayValue);
	}

	/**
     * Set lua data to a lua table, used to communiate with other lua files.
     * 
     * @param int nIndex - The index of the table. (Start from 1.).
     * @param object cValue - The value associated to the key.
     * @return void.
     */
	public void SetData(int nIndex, object cValue)
	{
		if (null == m_cLuaTableOpt)
		{
			return;
		}

		m_cLuaTableOpt.SetData(nIndex, cValue);
	}

	/**
     * Set lua data to a lua table, used to communiate with other lua files.
     * This is used to set an array data to an sub-table.
     * 
     * @param int nIndex - The index of the sub-table. (Start from 1.)
     * @param object cValue - The value associated to the key.
     * @return void.
     */
	public void SetData(int nIndex, object[] cArrayValue)
	{
		if (null == m_cLuaTableOpt)
		{
			return;
		}

		m_cLuaTableOpt.SetData(nIndex, cArrayValue);
	}

	/**
     * Get lua data from a lua table, used to communiate with other lua files.
     * 
     * @param string strName - The key name of the table.
     * @return object cValue - The value associated to the key.
     */
	public object GetData(string strName)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.GetData(strName);
	}

	/**
     * Get lua data from a lua table, used to communiate with other lua files.
     * 
     * @param int nIndex - The index of the table.
     * @return object cValue - The value associated to the key.
     */
	public object GetData(int nIndex)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.GetData(nIndex);
	}

	/**
     * Call a lua method.
     * 
     * @param out LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @return object - The number of result.
     */
	public object CallMethod(ref LuaFunction cFunc, string strFunc)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc);
	}

	/**
     * Call a lua method.
     * 
     * @param ref LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param object cParam - The param.
     * @return object - The number of result.
     */
	public object CallMethod(ref LuaFunction cFunc, string strFunc, object cParam)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc, cParam);
	}

	/**
     * Call a lua method.
     * 
     * @param ref LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param object cParam1 - The first param.
     * @param object cParam2 - The second param.
     * @return object - The number of result.
     */
	public object CallMethod(ref LuaFunction cFunc, string strFunc, object cParam1, object cParam2)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc, cParam1, cParam2);
	}

	/**
     * Call a lua method.
     * 
     * @param ref LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param object cParam1 - The first param.
     * @param object cParam2 - The second param.
     * @param object cParam3 - The third param.
     * @return object - The number of result.
     */
	public object CallMethod(ref LuaFunction cFunc, string strFunc, object cParam1, object cParam2, object cParam3)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc, cParam1, cParam2, cParam3);
	}

	/**
     * Call a lua method.
     * 
     * @param ref LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param params object[] aParams - The params.
     * @return object - The number of result.
     */
	public object CallMethod(ref LuaFunction cFunc, string strFunc, params object[] aParams)
	{
		if (null == m_cLuaTableOpt)
		{
			return null;
		}

		return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc, aParams);
	}
}

