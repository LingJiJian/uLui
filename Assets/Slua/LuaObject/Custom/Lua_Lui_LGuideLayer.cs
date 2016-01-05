using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LGuideLayer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_panel_guide_sub1(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.panel_guide_sub1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_panel_guide_sub1(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.panel_guide_sub1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_panel_guide_sub2(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.panel_guide_sub2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_panel_guide_sub2(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.panel_guide_sub2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_panel_guide_sub3(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.panel_guide_sub3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_panel_guide_sub3(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.panel_guide_sub3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_panel_guide_sub4(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.panel_guide_sub4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_panel_guide_sub4(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.panel_guide_sub4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_btn_target(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.btn_target);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_btn_target(IntPtr l) {
		try {
			Lui.LGuideLayer self=(Lui.LGuideLayer)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.btn_target=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LGuideLayer");
		addMember(l,"panel_guide_sub1",get_panel_guide_sub1,set_panel_guide_sub1,true);
		addMember(l,"panel_guide_sub2",get_panel_guide_sub2,set_panel_guide_sub2,true);
		addMember(l,"panel_guide_sub3",get_panel_guide_sub3,set_panel_guide_sub3,true);
		addMember(l,"panel_guide_sub4",get_panel_guide_sub4,set_panel_guide_sub4,true);
		addMember(l,"btn_target",get_btn_target,set_btn_target,true);
		createTypeMetatable(l,null, typeof(Lui.LGuideLayer),typeof(UnityEngine.MonoBehaviour));
	}
}
