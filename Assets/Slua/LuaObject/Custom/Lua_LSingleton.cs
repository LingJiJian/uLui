using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LSingleton : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsCreatedInstance_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=LSingleton.IsCreatedInstance(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=LSingleton.getInstance(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveInstance_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			LSingleton.RemoveInstance(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsDestroying(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LSingleton.IsDestroying);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LSingleton");
		addMember(l,IsCreatedInstance_s);
		addMember(l,getInstance_s);
		addMember(l,RemoveInstance_s);
		addMember(l,"IsDestroying",get_IsDestroying,null,false);
		createTypeMetatable(l,null, typeof(LSingleton),typeof(UnityEngine.MonoBehaviour));
	}
}
