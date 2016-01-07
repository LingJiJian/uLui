using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LScrollView : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContainerSize(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			self.setContainerSize(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerDown(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
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
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
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
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
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
	static public int setContentOffset(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			self.setContentOffset(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetWithoutCheck(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			self.setContentOffsetWithoutCheck(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetToTop(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			self.setContentOffsetToTop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetToBottom(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			self.setContentOffsetToBottom();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetToRight(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			self.setContentOffsetToRight();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetToLeft(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			self.setContentOffsetToLeft();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetInDuration(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.setContentOffsetInDuration(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setContentOffsetInDurationWithoutCheck(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.setContentOffsetInDurationWithoutCheck(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getContentOffset(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			var ret=self.getContentOffset();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_INVALID_INDEX(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LScrollView.INVALID_INDEX);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_INVALID_INDEX(IntPtr l) {
		try {
			System.Int32 v;
			checkType(l,2,out v);
			Lui.LScrollView.INVALID_INDEX=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RELOCATE_DURATION(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Lui.LScrollView.RELOCATE_DURATION);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RELOCATE_DURATION(IntPtr l) {
		try {
			System.Single v;
			checkType(l,2,out v);
			Lui.LScrollView.RELOCATE_DURATION=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_bounceable(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bounceable);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_bounceable(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.bounceable=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_container(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.container);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_container(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.container=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_direction(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.direction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_direction(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			Lui.ScrollDirection v;
			checkEnum(l,2,out v);
			self.direction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_dragable(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dragable);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_dragable(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.dragable=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onMoveCompleteHandler(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Events.UnityAction v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onMoveCompleteHandler=v;
			else if(op==1) self.onMoveCompleteHandler+=v;
			else if(op==2) self.onMoveCompleteHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onScrollingHandler(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Events.UnityAction v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onScrollingHandler=v;
			else if(op==1) self.onScrollingHandler+=v;
			else if(op==2) self.onScrollingHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onDraggingScrollEndedHandler(IntPtr l) {
		try {
			Lui.LScrollView self=(Lui.LScrollView)checkSelf(l);
			UnityEngine.Events.UnityAction v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onDraggingScrollEndedHandler=v;
			else if(op==1) self.onDraggingScrollEndedHandler+=v;
			else if(op==2) self.onDraggingScrollEndedHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LScrollView");
		addMember(l,setContainerSize);
		addMember(l,OnPointerDown);
		addMember(l,OnDrag);
		addMember(l,OnPointerUp);
		addMember(l,setContentOffset);
		addMember(l,setContentOffsetWithoutCheck);
		addMember(l,setContentOffsetToTop);
		addMember(l,setContentOffsetToBottom);
		addMember(l,setContentOffsetToRight);
		addMember(l,setContentOffsetToLeft);
		addMember(l,setContentOffsetInDuration);
		addMember(l,setContentOffsetInDurationWithoutCheck);
		addMember(l,getContentOffset);
		addMember(l,"INVALID_INDEX",get_INVALID_INDEX,set_INVALID_INDEX,false);
		addMember(l,"RELOCATE_DURATION",get_RELOCATE_DURATION,set_RELOCATE_DURATION,false);
		addMember(l,"bounceable",get_bounceable,set_bounceable,true);
		addMember(l,"container",get_container,set_container,true);
		addMember(l,"direction",get_direction,set_direction,true);
		addMember(l,"dragable",get_dragable,set_dragable,true);
		addMember(l,"onMoveCompleteHandler",null,set_onMoveCompleteHandler,true);
		addMember(l,"onScrollingHandler",null,set_onScrollingHandler,true);
		addMember(l,"onDraggingScrollEndedHandler",null,set_onDraggingScrollEndedHandler,true);
		createTypeMetatable(l,null, typeof(Lui.LScrollView),typeof(UnityEngine.MonoBehaviour));
	}
}
