using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UEvent o;
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			o=new UEvent(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_eventType(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.eventType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_eventType(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.eventType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_eventParams(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.eventParams);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_eventParams(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.eventParams=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_target(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target(IntPtr l) {
		try {
			UEvent self=(UEvent)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.target=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UEvent");
		addMember(l,"eventType",get_eventType,set_eventType,true);
		addMember(l,"eventParams",get_eventParams,set_eventParams,true);
		addMember(l,"target",get_target,set_target,true);
		createTypeMetatable(l,constructor, typeof(UEvent));
	}
}
