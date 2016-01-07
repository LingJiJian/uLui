using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LControlView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerDown(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
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
	static public int OnDrag(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnDrag(a1);
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
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
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
	static public int get_centerPoint(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.centerPoint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_centerPoint(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			self.centerPoint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_radius(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.radius);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_radius(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.radius=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_relocateWithAnimation(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.relocateWithAnimation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_relocateWithAnimation(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.relocateWithAnimation=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_joyStick(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.joyStick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_joyStick(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.joyStick=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onControlHandler(IntPtr l) {
		try {
			Lui.LControlView self=(Lui.LControlView)checkSelf(l);
			UnityEngine.Events.UnityAction<System.Single,System.Single> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onControlHandler=v;
			else if(op==1) self.onControlHandler+=v;
			else if(op==2) self.onControlHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LControlView");
		addMember(l,OnPointerDown);
		addMember(l,OnDrag);
		addMember(l,OnPointerUp);
		addMember(l,"centerPoint",get_centerPoint,set_centerPoint,true);
		addMember(l,"radius",get_radius,set_radius,true);
		addMember(l,"relocateWithAnimation",get_relocateWithAnimation,set_relocateWithAnimation,true);
		addMember(l,"joyStick",get_joyStick,set_joyStick,true);
		addMember(l,"onControlHandler",null,set_onControlHandler,true);
		createTypeMetatable(l,null, typeof(Lui.LControlView),typeof(UnityEngine.MonoBehaviour));
	}
}
