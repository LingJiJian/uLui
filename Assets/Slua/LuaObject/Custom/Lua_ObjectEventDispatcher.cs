using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ObjectEventDispatcher : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ObjectEventDispatcher o;
			o=new ObjectEventDispatcher();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_dispatcher(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ObjectEventDispatcher.dispatcher);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ObjectEventDispatcher");
		addMember(l,"dispatcher",get_dispatcher,null,false);
		createTypeMetatable(l,constructor, typeof(ObjectEventDispatcher));
	}
}
