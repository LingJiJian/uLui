using UnityEngine;
using System.Collections;

public class CubeObject : MonoBehaviour 
{
	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = new RaycastHit();
			if(Physics.Raycast(ray, out raycastHit))
			{
				if(raycastHit.collider.gameObject.name == "Cube")
				{
					// 触发事件
					ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.CUBE_CLICK, "cube"), this);
				}
			}
		}
	}
}
