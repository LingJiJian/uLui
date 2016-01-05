using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SLuaTest : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_intevent(IntPtr l) {
		try {
			SLuaTest self=(SLuaTest)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.intevent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_intevent(IntPtr l) {
		try {
			SLuaTest self=(SLuaTest)checkSelf(l);
			FloatEvent v;
			checkType(l,2,out v);
			self.intevent=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SLuaTest");
		addMember(l,"intevent",get_intevent,set_intevent,true);
		createTypeMetatable(l,null, typeof(SLuaTest),typeof(UnityEngine.MonoBehaviour));
	}
}
