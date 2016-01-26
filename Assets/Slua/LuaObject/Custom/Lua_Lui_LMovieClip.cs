using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LMovieClip : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int loadTexture(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			self.loadTexture();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int play(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			self.play();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int stop(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			self.stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int pause(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			self.pause();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_fps(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fps);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_fps(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.fps=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isPlayOnwake(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isPlayOnwake);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isPlayOnwake(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.isPlayOnwake=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_path(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.path);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_path(IntPtr l) {
		try {
			Lui.LMovieClip self=(Lui.LMovieClip)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.path=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LMovieClip");
		addMember(l,loadTexture);
		addMember(l,play);
		addMember(l,stop);
		addMember(l,pause);
		addMember(l,"fps",get_fps,set_fps,true);
		addMember(l,"isPlayOnwake",get_isPlayOnwake,set_isPlayOnwake,true);
		addMember(l,"path",get_path,set_path,true);
		createTypeMetatable(l,null, typeof(Lui.LMovieClip),typeof(UnityEngine.MonoBehaviour));
	}
}
