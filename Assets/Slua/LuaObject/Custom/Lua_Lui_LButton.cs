using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LButton : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerDown(IntPtr l) {
		try {
			Lui.LButton self=(Lui.LButton)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerDown(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerUp(IntPtr l) {
		try {
			Lui.LButton self=(Lui.LButton)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerUp(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerExit(IntPtr l) {
		try {
			Lui.LButton self=(Lui.LButton)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerExit(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LONGPRESS_TIME(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LButton.LONGPRESS_TIME);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onLongClickHandler(IntPtr l) {
		try {
			Lui.LButton self=(Lui.LButton)checkSelf(l);
			UnityEngine.Events.UnityAction v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onLongClickHandler=v;
			else if(op==1) self.onLongClickHandler+=v;
			else if(op==2) self.onLongClickHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onLongClickUpdate(IntPtr l) {
		try {
			Lui.LButton self=(Lui.LButton)checkSelf(l);
			UnityEngine.Events.UnityAction v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onLongClickUpdate=v;
			else if(op==1) self.onLongClickUpdate+=v;
			else if(op==2) self.onLongClickUpdate-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LButton");
		addMember(l,OnPointerDown);
		addMember(l,OnPointerUp);
		addMember(l,OnPointerExit);
		addMember(l,"LONGPRESS_TIME",get_LONGPRESS_TIME,null,false);
		addMember(l,"onLongClickHandler",null,set_onLongClickHandler,true);
		addMember(l,"onLongClickUpdate",null,set_onLongClickUpdate,true);
		createTypeMetatable(l,null, typeof(Lui.LButton),typeof(UnityEngine.MonoBehaviour));
	}
}
