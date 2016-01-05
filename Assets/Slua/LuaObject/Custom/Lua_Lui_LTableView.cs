using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LTableView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Lui.LTableView o;
			o=new Lui.LTableView();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int reloadData(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			self.reloadData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllFromUsed(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			self.removeAllFromUsed();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllFromFreed(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			self.removeAllFromFreed();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int cellAtIndex(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.cellAtIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int updateCellAtIndex(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
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
	static public int get_AUTO_RELOCATE_SPPED(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LTableView.AUTO_RELOCATE_SPPED);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AUTO_RELOCATE_SPPED(IntPtr l) {
		try {
			System.Single v;
			checkType(l,2,out v);
			Lui.LTableView.AUTO_RELOCATE_SPPED=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cellsCount(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cellsCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cellsCount(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cellsCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cellsSize(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cellsSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cellsSize(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			self.cellsSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_autoRelocate(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.autoRelocate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_autoRelocate(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.autoRelocate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cellTemplate(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cellTemplate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cellTemplate(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			Lui.LTableViewCell v;
			checkType(l,2,out v);
			self.cellTemplate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onDataSourceAdapterHandler(IntPtr l) {
		try {
			Lui.LTableView self=(Lui.LTableView)checkSelf(l);
			Lui.LDataSourceAdapter<Lui.LTableViewCell,System.Int32> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onDataSourceAdapterHandler=v;
			else if(op==1) self.onDataSourceAdapterHandler+=v;
			else if(op==2) self.onDataSourceAdapterHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LTableView");
		addMember(l,reloadData);
		addMember(l,removeAllFromUsed);
		addMember(l,removeAllFromFreed);
		addMember(l,cellAtIndex);
		addMember(l,updateCellAtIndex);
		addMember(l,"AUTO_RELOCATE_SPPED",get_AUTO_RELOCATE_SPPED,set_AUTO_RELOCATE_SPPED,false);
		addMember(l,"cellsCount",get_cellsCount,set_cellsCount,true);
		addMember(l,"cellsSize",get_cellsSize,set_cellsSize,true);
		addMember(l,"autoRelocate",get_autoRelocate,set_autoRelocate,true);
		addMember(l,"cellTemplate",get_cellTemplate,set_cellTemplate,true);
		addMember(l,"onDataSourceAdapterHandler",null,set_onDataSourceAdapterHandler,true);
		createTypeMetatable(l,constructor, typeof(Lui.LTableView),typeof(Lui.LScrollView));
	}
}
