using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UEventListener : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UEventListener o;
			System.String a1;
			checkType(l,2,out a1);
			o=new UEventListener(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Excute(IntPtr l) {
		try {
			UEventListener self=(UEventListener)checkSelf(l);
			UEvent a1;
			checkType(l,2,out a1);
			self.Excute(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_eventType(IntPtr l) {
		try {
			UEventListener self=(UEventListener)checkSelf(l);
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
			UEventListener self=(UEventListener)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"UEventListener");
		addMember(l,Excute);
		addMember(l,"eventType",get_eventType,set_eventType,true);
		createTypeMetatable(l,constructor, typeof(UEventListener));
	}
}
