using System.Collections;

public class UEventListener
{
	/// <summary>
	/// 事件类型
	/// </summary>
	public string eventType;

	public UEventListener(string eventType)
	{
		this.eventType = eventType;
	}

	public delegate void EventListenerDelegate(UEvent evt);
	public event EventListenerDelegate OnEvent;
	
	public void Excute(UEvent evt)
	{
		if (OnEvent != null) 
		{
			this.OnEvent (evt);
		}
	}
}

