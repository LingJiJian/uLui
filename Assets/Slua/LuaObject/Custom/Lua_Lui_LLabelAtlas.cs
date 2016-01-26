using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LLabelAtlas : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setAtlasMap(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int32,System.Char>> a1;
			checkType(l,2,out a1);
			self.setAtlasMap(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sprite(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sprite);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_sprite(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			UnityEngine.Sprite[] v;
			checkArray(l,2,out v);
			self.sprite=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_text(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_text(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cuts(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cuts);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cuts(IntPtr l) {
		try {
			Lui.LLabelAtlas self=(Lui.LLabelAtlas)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cuts=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LLabelAtlas");
		addMember(l,setAtlasMap);
		addMember(l,"sprite",get_sprite,set_sprite,true);
		addMember(l,"text",get_text,set_text,true);
		addMember(l,"cuts",get_cuts,set_cuts,true);
		createTypeMetatable(l,null, typeof(Lui.LLabelAtlas),typeof(UnityEngine.MonoBehaviour));
	}
}
