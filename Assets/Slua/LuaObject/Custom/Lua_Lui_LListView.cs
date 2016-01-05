using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LListView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Lui.LListView o;
			o=new Lui.LListView();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getNodeAtIndex(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.getNodeAtIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int insertNodeAtLast(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.insertNodeAtLast(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int insertNodeAtFront(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.insertNodeAtFront(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int insertNode(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.insertNode(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeNodeAtIndex(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.removeNodeAtIndex(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeNode(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.removeNode(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeFrontNode(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			self.removeFrontNode();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeLastNode(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			self.removeLastNode();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllNodes(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			self.removeAllNodes();
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
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			self.reloadData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int dequeueItem(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			var ret=self.dequeueItem();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HorizontalNodeAnchorPoint(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LListView.HorizontalNodeAnchorPoint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HorizontalNodeAnchorPoint(IntPtr l) {
		try {
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			Lui.LListView.HorizontalNodeAnchorPoint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_VerticalNodeAnchorPoint(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LListView.VerticalNodeAnchorPoint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_VerticalNodeAnchorPoint(IntPtr l) {
		try {
			UnityEngine.Vector2 v;
			checkType(l,2,out v);
			Lui.LListView.VerticalNodeAnchorPoint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_limitNum(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.limitNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_limitNum(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.limitNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_itemTemplate(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.itemTemplate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_itemTemplate(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.itemTemplate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_bounceBox(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bounceBox);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_bounceBox(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			UnityEngine.Rect v;
			checkValueType(l,2,out v);
			self.bounceBox=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_nodeList(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.nodeList);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_freeList(IntPtr l) {
		try {
			Lui.LListView self=(Lui.LListView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.freeList);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LListView");
		addMember(l,getNodeAtIndex);
		addMember(l,insertNodeAtLast);
		addMember(l,insertNodeAtFront);
		addMember(l,insertNode);
		addMember(l,removeNodeAtIndex);
		addMember(l,removeNode);
		addMember(l,removeFrontNode);
		addMember(l,removeLastNode);
		addMember(l,removeAllNodes);
		addMember(l,reloadData);
		addMember(l,dequeueItem);
		addMember(l,"HorizontalNodeAnchorPoint",get_HorizontalNodeAnchorPoint,set_HorizontalNodeAnchorPoint,false);
		addMember(l,"VerticalNodeAnchorPoint",get_VerticalNodeAnchorPoint,set_VerticalNodeAnchorPoint,false);
		addMember(l,"limitNum",get_limitNum,set_limitNum,true);
		addMember(l,"itemTemplate",get_itemTemplate,set_itemTemplate,true);
		addMember(l,"bounceBox",get_bounceBox,set_bounceBox,true);
		addMember(l,"nodeList",get_nodeList,null,true);
		addMember(l,"freeList",get_freeList,null,true);
		createTypeMetatable(l,constructor, typeof(Lui.LListView),typeof(Lui.LScrollView));
	}
}
