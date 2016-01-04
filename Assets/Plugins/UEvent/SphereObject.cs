using UnityEngine;
using System.Collections;

public class SphereObject : MonoBehaviour 
{
	private float position;
	private float targetPosition;
	private float currentVelocity;

	void Awake()
	{
		// 添加事件侦听
		ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.CUBE_CLICK, OnClickHandler);
	}

	/// <summary>
	/// 事件回调函数
	/// </summary>
	/// <param name="uEvent">U event.</param>
	private void OnClickHandler(UEvent uEvent)
	{
		this.targetPosition = this.targetPosition == 2f ? -2f : 2f;
		
		this.StopCoroutine (this.PositionOperater ());
		this.StartCoroutine (this.PositionOperater());
	}
	
	IEnumerator PositionOperater()
	{
		while (this.position != this.targetPosition) 
		{
			this.position = Mathf.SmoothDamp (this.position, this.targetPosition, ref currentVelocity, 0.5f);
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.position, this.transform.localPosition.z);
			
			if(Mathf.Abs(this.position - this.targetPosition) <= 0.1f) this.position = this.targetPosition;
			
			yield return null;
		}
	}
}
