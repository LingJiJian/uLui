using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ListViewEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ListViewEvent o;
			o=new ListViewEvent();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		LuaUnityEvent_int_string.reg(l);
		getTypeTable(l,"ListViewEvent");
		createTypeMetatable(l,constructor, typeof(ListViewEvent),typeof(LuaUnityEvent_int_string));
	}
}
