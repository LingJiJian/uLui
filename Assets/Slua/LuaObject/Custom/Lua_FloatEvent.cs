using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_FloatEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			FloatEvent o;
			o=new FloatEvent();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		LuaUnityEvent_float.reg(l);
		getTypeTable(l,"FloatEvent");
		createTypeMetatable(l,constructor, typeof(FloatEvent),typeof(LuaUnityEvent_float));
	}
}
