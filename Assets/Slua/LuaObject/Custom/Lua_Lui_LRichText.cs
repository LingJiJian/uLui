using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Lui_LRichText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeAllElements(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			self.removeAllElements();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int insertElement(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				Lui.LRichText self=(Lui.LRichText)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.insertElement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				Lui.LRichText self=(Lui.LRichText)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				self.insertElement(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				Lui.LRichText self=(Lui.LRichText)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.String a3;
				checkType(l,4,out a3);
				self.insertElement(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==8){
				Lui.LRichText self=(Lui.LRichText)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				UnityEngine.Color a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				System.Boolean a5;
				checkType(l,6,out a5);
				UnityEngine.Color a6;
				checkType(l,7,out a6);
				System.String a7;
				checkType(l,8,out a7);
				self.insertElement(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int reloadData(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			self.reloadData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnPointerClick(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerClick(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_alignType(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.alignType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_alignType(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			Lui.RichAlignType v;
			checkEnum(l,2,out v);
			self.alignType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_verticalSpace(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.verticalSpace);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_verticalSpace(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.verticalSpace=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_maxLineWidth(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.maxLineWidth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_maxLineWidth(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.maxLineWidth=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_font(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.font);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_font(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			UnityEngine.Font v;
			checkType(l,2,out v);
			self.font=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onClickHandler(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			UnityEngine.Events.UnityAction<System.String> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onClickHandler=v;
			else if(op==1) self.onClickHandler+=v;
			else if(op==2) self.onClickHandler-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_realLineHeight(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.realLineHeight);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_realLineWidth(IntPtr l) {
		try {
			Lui.LRichText self=(Lui.LRichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.realLineWidth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Lui.LRichText");
		addMember(l,removeAllElements);
		addMember(l,insertElement);
		addMember(l,reloadData);
		addMember(l,OnPointerClick);
		addMember(l,"alignType",get_alignType,set_alignType,true);
		addMember(l,"verticalSpace",get_verticalSpace,set_verticalSpace,true);
		addMember(l,"maxLineWidth",get_maxLineWidth,set_maxLineWidth,true);
		addMember(l,"font",get_font,set_font,true);
		addMember(l,"onClickHandler",null,set_onClickHandler,true);
		addMember(l,"realLineHeight",get_realLineHeight,null,true);
		addMember(l,"realLineWidth",get_realLineWidth,null,true);
		createTypeMetatable(l,null, typeof(Lui.LRichText),typeof(UnityEngine.MonoBehaviour));
	}
}
