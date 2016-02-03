using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Apple_TV_Remote : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.Apple.TV.Remote o;
			o=new UnityEngine.Apple.TV.Remote();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_allowExitToHome(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.Apple.TV.Remote.allowExitToHome);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_allowExitToHome(IntPtr l) {
		try {
			bool v;
			checkType(l,2,out v);
			UnityEngine.Apple.TV.Remote.allowExitToHome=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_allowRemoteRotation(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.Apple.TV.Remote.allowRemoteRotation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_allowRemoteRotation(IntPtr l) {
		try {
			bool v;
			checkType(l,2,out v);
			UnityEngine.Apple.TV.Remote.allowRemoteRotation=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_reportAbsoluteDpadValues(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.Apple.TV.Remote.reportAbsoluteDpadValues);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_reportAbsoluteDpadValues(IntPtr l) {
		try {
			bool v;
			checkType(l,2,out v);
			UnityEngine.Apple.TV.Remote.reportAbsoluteDpadValues=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_touchesEnabled(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.Apple.TV.Remote.touchesEnabled);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_touchesEnabled(IntPtr l) {
		try {
			bool v;
			checkType(l,2,out v);
			UnityEngine.Apple.TV.Remote.touchesEnabled=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Apple.TV.Remote");
		addMember(l,"allowExitToHome",get_allowExitToHome,set_allowExitToHome,false);
		addMember(l,"allowRemoteRotation",get_allowRemoteRotation,set_allowRemoteRotation,false);
		addMember(l,"reportAbsoluteDpadValues",get_reportAbsoluteDpadValues,set_reportAbsoluteDpadValues,false);
		addMember(l,"touchesEnabled",get_touchesEnabled,set_touchesEnabled,false);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Apple.TV.Remote));
	}
}
