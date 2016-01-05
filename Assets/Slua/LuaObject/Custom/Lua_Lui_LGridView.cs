using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LGridView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Lui.LGridView o;
			o=new Lui.LGridView();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllFromUsed(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			self.removeAllFromFreed();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int insertSortableCell(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			Lui.LGridViewCell a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.insertSortableCell(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int updateCellAtIndex(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.updateCellAtIndex(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getCells(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			var ret=self.getCells();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int cellAtIndex(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
	static public int reloadData(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			self.reloadData();
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
	static public int get_cellsCount(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
	static public int get_cols(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cols);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cols(IntPtr l) {
		try {
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cols=v;
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			Lui.LGridViewCell v;
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
			Lui.LGridView self=(Lui.LGridView)checkSelf(l);
			Lui.LDataSourceAdapter<Lui.LGridViewCell,System.Int32> v;
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
		getTypeTable(l,"Lui.LGridView");
		addMember(l,removeAllFromUsed);
		addMember(l,removeAllFromFreed);
		addMember(l,insertSortableCell);
		addMember(l,updateCellAtIndex);
		addMember(l,getCells);
		addMember(l,cellAtIndex);
		addMember(l,reloadData);
		addMember(l,"cellsSize",get_cellsSize,set_cellsSize,true);
		addMember(l,"cellsCount",get_cellsCount,set_cellsCount,true);
		addMember(l,"cols",get_cols,set_cols,true);
		addMember(l,"autoRelocate",get_autoRelocate,set_autoRelocate,true);
		addMember(l,"cellTemplate",get_cellTemplate,set_cellTemplate,true);
		addMember(l,"onDataSourceAdapterHandler",null,set_onDataSourceAdapterHandler,true);
		createTypeMetatable(l,constructor, typeof(Lui.LGridView),typeof(Lui.LScrollView));
	}
}
