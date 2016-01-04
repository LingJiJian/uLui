using UnityEngine;
using System.Collections;

public class CapsuleObject : MonoBehaviour 
{
	private float angle;
	private float targetAngle;
	private float currentVelocity;

	void Awake()
	{
		// 事件侦听者
		ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.CUBE_CLICK, OnClickHandler);
	}
	
	private void OnClickHandler(UEvent uEvent)
	{
		this.targetAngle = this.targetAngle == 90f ? 0f : 90f;

		this.StopCoroutine (this.RotationOperater ());
		this.StartCoroutine (this.RotationOperater());
	}

	IEnumerator RotationOperater()
	{
		while (this.angle != this.targetAngle) 
		{
			this.angle = Mathf.SmoothDampAngle (this.angle, this.targetAngle, ref currentVelocity, 0.5f);
			this.transform.rotation = Quaternion.AngleAxis(this.angle, Vector3.forward);

			if(Mathf.Abs(this.angle - this.targetAngle) <= 1) this.angle = this.targetAngle;

			yield return null;
		}
	}
}
