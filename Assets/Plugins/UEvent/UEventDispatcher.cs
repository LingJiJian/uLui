using System.Collections.Generic;

public class UEventDispatcher
{
	protected IList<UEventListener> eventListenerList;

	public UEventDispatcher()
	{
		this.eventListenerList = new List<UEventListener> ();
	}

	/// <summary>
	/// 侦听事件
	/// </summary>
	/// <param name="eventType">事件类别</param>
	/// <param name="callback">回调函数</param>
	public void addEventListener(string eventType, UEventListener.EventListenerDelegate callback)
	{
		UEventListener eventListener = this.getListener(eventType);
		if (eventListener == null)
		{
			eventListener = new UEventListener(eventType);
			eventListenerList.Add(eventListener);
		}

		eventListener.OnEvent += callback;
	}
	
	/// <summary>
	/// 移除事件
	/// </summary>
	/// <param name="eventType">事件类别</param>
	/// <param name="callback">回调函数</param>
	public void removeEventListener(string eventType, UEventListener.EventListenerDelegate callback)
	{
		UEventListener eventListener = this.getListener(eventType);
		if (eventListener != null) 
		{
			eventListener.OnEvent -= callback;
		}
	}
	
	/// <summary>
	/// 是否存在事件
	/// </summary>
	/// <returns><c>true</c>, if listener was hased, <c>false</c> otherwise.</returns>
	/// <param name="eventType">Event type.</param>
	public bool hasListener(string eventType)
	{
		return this.getListenerList (eventType).Count > 0;
	}
	
	/// <summary>
	/// 发送事件
	/// </summary>
	/// <param name="evt">Evt.</param>
	/// <param name="gameObject">Game object.</param>
	public void dispatchEvent(UEvent evt, object gameObject)
	{
		IList<UEventListener> resultList = this.getListenerList (evt.eventType);

		foreach (UEventListener eventListener in resultList) 
		{
			evt.target = gameObject;

			eventListener.Excute(evt);
		}
	}

	/// <summary>
	/// 获取事件列表
	/// </summary>
	/// <returns>The listener list.</returns>
	/// <param name="eventType">Event type.</param>
	private IList<UEventListener> getListenerList(string eventType)
	{
		IList<UEventListener> resultList = new List<UEventListener> ();
		foreach (UEventListener eventListener in this.eventListenerList) 
		{
			if(eventListener.eventType == eventType) resultList.Add(eventListener);
		}
		return resultList;
	}

	/// <summary>
	/// 获取事件
	/// </summary>
	/// <returns>The listener.</returns>
	/// <param name="eventType">Event type.</param>
	private UEventListener getListener(string eventType)
	{
		foreach (UEventListener eventListener in this.eventListenerList) 
		{
			if(eventListener.eventType == eventType) return eventListener;
		}
		return null;
	}
}
