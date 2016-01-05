using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_Dropdown_DropdownEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.UI.Dropdown.DropdownEvent o;
			o=new UnityEngine.UI.Dropdown.DropdownEvent();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		LuaUnityEvent_int.reg(l);
		getTypeTable(l,"UnityEngine.UI.Dropdown.DropdownEvent");
		createTypeMetatable(l,constructor, typeof(UnityEngine.UI.Dropdown.DropdownEvent),typeof(LuaUnityEvent_int));
	}
}
