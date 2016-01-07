using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LPageView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Lui.LPageView o;
			o=new Lui.LPageView();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int updateCellAtIndex(IntPtr l) {
		try {
			Lui.LPageView self=(Lui.LPageView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.updateCellAtIndex(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onPageChangedHandler(IntPtr l) {
		try {
			Lui.LPageView self=(Lui.LPageView)checkSelf(l);
			UnityEngine.Events.UnityAction<System.Int32> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onPageChangedHandler=v;
			else if(op==1) self.onPageChangedHandler+=v;
			else if(op==2) self.onPageChangedHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_pageIndex(IntPtr l) {
		try {
			Lui.LPageView self=(Lui.LPageView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pageIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LPageView");
		addMember(l,updateCellAtIndex);
		addMember(l,"onPageChangedHandler",null,set_onPageChangedHandler,true);
		addMember(l,"pageIndex",get_pageIndex,null,true);
		createTypeMetatable(l,constructor, typeof(Lui.LPageView),typeof(Lui.LTableView));
	}
}
