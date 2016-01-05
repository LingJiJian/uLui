using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LUtils : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			LUtils o;
			o=new LUtils();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSecondString(IntPtr l) {
		try {
			LUtils self=(LUtils)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSecondString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTimeStamp_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			var ret=LUtils.GetTimeStamp(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NormalizeTimpstamp0_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=LUtils.NormalizeTimpstamp0(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConvertIntDateTime_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=LUtils.ConvertIntDateTime(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConvertDateTimeInt_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=LUtils.ConvertDateTimeInt(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DateTimeToUnixTimestamp_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=LUtils.DateTimeToUnixTimestamp(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnixTimestampToDateTime_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			var ret=LUtils.UnixTimestampToDateTime(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LUtils");
		addMember(l,GetSecondString);
		addMember(l,GetTimeStamp_s);
		addMember(l,NormalizeTimpstamp0_s);
		addMember(l,ConvertIntDateTime_s);
		addMember(l,ConvertDateTimeInt_s);
		addMember(l,DateTimeToUnixTimestamp_s);
		addMember(l,UnixTimestampToDateTime_s);
		createTypeMetatable(l,constructor, typeof(LUtils));
	}
}
