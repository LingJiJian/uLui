using UnityEngine;
using System.Collections;
using SLua;

[CustomLuaClassAttribute]
public class ObjectEventDispatcher
{
	public static readonly UEventDispatcher dispatcher = new UEventDispatcher();
}
