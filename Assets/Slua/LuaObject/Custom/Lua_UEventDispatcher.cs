using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UEventDispatcher : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UEventDispatcher o;
			o=new UEventDispatcher();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int addEventListener(IntPtr l) {
		try {
			UEventDispatcher self=(UEventDispatcher)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UEventListener.EventListenerDelegate a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.addEventListener(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeEventListener(IntPtr l) {
		try {
			UEventDispatcher self=(UEventDispatcher)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UEventListener.EventListenerDelegate a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.removeEventListener(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int hasListener(IntPtr l) {
		try {
			UEventDispatcher self=(UEventDispatcher)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.hasListener(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int dispatchEvent(IntPtr l) {
		try {
			UEventDispatcher self=(UEventDispatcher)checkSelf(l);
			UEvent a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.dispatchEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UEventDispatcher");
		addMember(l,addEventListener);
		addMember(l,removeEventListener);
		addMember(l,hasListener);
		addMember(l,dispatchEvent);
		createTypeMetatable(l,constructor, typeof(UEventDispatcher));
	}
}
