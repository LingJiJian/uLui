using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_EventSystems_TouchInputModule : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateModule(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			self.UpdateModule();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsModuleSupported(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			var ret=self.IsModuleSupported();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShouldActivateModule(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			var ret=self.ShouldActivateModule();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Process(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			self.Process();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DeactivateModule(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			self.DeactivateModule();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_forceModuleActive(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.forceModuleActive);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_forceModuleActive(IntPtr l) {
		try {
			UnityEngine.EventSystems.TouchInputModule self=(UnityEngine.EventSystems.TouchInputModule)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.forceModuleActive=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.EventSystems.TouchInputModule");
		addMember(l,UpdateModule);
		addMember(l,IsModuleSupported);
		addMember(l,ShouldActivateModule);
		addMember(l,Process);
		addMember(l,DeactivateModule);
		addMember(l,"forceModuleActive",get_forceModuleActive,set_forceModuleActive,true);
		createTypeMetatable(l,null, typeof(UnityEngine.EventSystems.TouchInputModule),typeof(UnityEngine.EventSystems.PointerInputModule));
	}
}
