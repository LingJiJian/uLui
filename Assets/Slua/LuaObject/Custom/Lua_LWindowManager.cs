using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LWindowManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadScene(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.LoadScene(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSceneAsync(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.Events.UnityAction<System.Single> a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.LoadSceneAsync(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int runWindow(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			WindowHierarchy a2;
			checkEnum(l,3,out a2);
			System.Collections.ArrayList a3;
			checkType(l,4,out a3);
			self.runWindow(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int seekWindow(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.seekWindow(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int popWindow(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(LWindowBase))){
				LWindowManager self=(LWindowManager)checkSelf(l);
				LWindowBase a1;
				checkType(l,2,out a1);
				self.popWindow(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				LWindowManager self=(LWindowManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.popWindow(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int popAllWindow(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				LWindowManager self=(LWindowManager)checkSelf(l);
				self.popAllWindow();
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				LWindowManager self=(LWindowManager)checkSelf(l);
				WindowHierarchy a1;
				checkEnum(l,2,out a1);
				self.popAllWindow(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeCachedWindow(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.removeCachedWindow(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllCachedWindow(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			self.removeAllCachedWindow();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isRunning(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.isRunning(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetInstance_s(IntPtr l) {
		try {
			var ret=LWindowManager.GetInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onProgressAsyncScene(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			UnityEngine.Events.UnityAction<System.Single> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onProgressAsyncScene=v;
			else if(op==1) self.onProgressAsyncScene+=v;
			else if(op==2) self.onProgressAsyncScene-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_canvas(IntPtr l) {
		try {
			LWindowManager self=(LWindowManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.canvas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LWindowManager");
		addMember(l,LoadScene);
		addMember(l,LoadSceneAsync);
		addMember(l,runWindow);
		addMember(l,seekWindow);
		addMember(l,popWindow);
		addMember(l,popAllWindow);
		addMember(l,removeCachedWindow);
		addMember(l,removeAllCachedWindow);
		addMember(l,isRunning);
		addMember(l,GetInstance_s);
		addMember(l,"onProgressAsyncScene",null,set_onProgressAsyncScene,true);
		addMember(l,"canvas",get_canvas,null,true);
		createTypeMetatable(l,null, typeof(LWindowManager),typeof(UnityEngine.MonoBehaviour));
	}
}
