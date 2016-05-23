using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LGridPageView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Lui.LGridPageView o;
			o=new Lui.LGridPageView();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int updateGridCellsPosition(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			self.updateGridCellsPosition();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int updatePageCount(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			self.updatePageCount();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int reloadData(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			self.reloadData();
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
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
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
	static public int setPageChangedHandler(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			UnityEngine.Events.UnityAction<System.Int32> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.setPageChangedHandler(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_gridCellsCount(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.gridCellsCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_gridCellsCount(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.gridCellsCount=v;
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
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
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
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
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
	static public int get_rows(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.rows);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_rows(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.rows=v;
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
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
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
	static public int get_gridCellsSize(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.gridCellsSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_gridCellsSize(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			self.gridCellsSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onGridDataSourceAdapterHandler(IntPtr l) {
		try {
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			Lui.LScrollView.LDataSourceAdapter<Lui.LGridPageViewCell,System.Int32> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onGridDataSourceAdapterHandler=v;
			else if(op==1) self.onGridDataSourceAdapterHandler+=v;
			else if(op==2) self.onGridDataSourceAdapterHandler-=v;
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
			Lui.LGridPageView self=(Lui.LGridPageView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pageIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LGridPageView");
		addMember(l,updateGridCellsPosition);
		addMember(l,updatePageCount);
		addMember(l,reloadData);
		addMember(l,updateCellAtIndex);
		addMember(l,setPageChangedHandler);
		addMember(l,"gridCellsCount",get_gridCellsCount,set_gridCellsCount,true);
		addMember(l,"cols",get_cols,set_cols,true);
		addMember(l,"rows",get_rows,set_rows,true);
		addMember(l,"onPageChangedHandler",null,set_onPageChangedHandler,true);
		addMember(l,"gridCellsSize",get_gridCellsSize,set_gridCellsSize,true);
		addMember(l,"onGridDataSourceAdapterHandler",null,set_onGridDataSourceAdapterHandler,true);
		addMember(l,"pageIndex",get_pageIndex,null,true);
		createTypeMetatable(l,constructor, typeof(Lui.LGridPageView),typeof(Lui.LTableView));
	}
}
