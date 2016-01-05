using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_Slider_SliderEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.UI.Slider.SliderEvent o;
			o=new UnityEngine.UI.Slider.SliderEvent();
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
		getTypeTable(l,"UnityEngine.UI.Slider.SliderEvent");
		createTypeMetatable(l,constructor, typeof(UnityEngine.UI.Slider.SliderEvent),typeof(LuaUnityEvent_float));
	}
}
