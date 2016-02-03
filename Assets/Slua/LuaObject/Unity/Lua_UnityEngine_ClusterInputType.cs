using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ClusterInputType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.ClusterInputType");
		addMember(l,0,"Button");
		addMember(l,1,"Axis");
		addMember(l,2,"Tracker");
		addMember(l,3,"CustomProvidedInput");
		LuaDLL.lua_pop(l, 1);
	}
}
