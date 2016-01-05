using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_ObjectEventDispatcher.reg,
				Lua_UEvent.reg,
				Lua_UEventDispatcher.reg,
				Lua_UEventListener.reg,
				Lua_LSingleton.reg,
				Lua_LUtils.reg,
				Lua_Lui_LButton.reg,
				Lua_Lui_LControlView.reg,
				Lua_Lui_LFrameAnimation.reg,
				Lua_Lui_LScrollView.reg,
				Lua_Lui_LTableView.reg,
				Lua_Lui_LGridPageView.reg,
				Lua_Lui_LGridView.reg,
				Lua_Lui_LGuideLayer.reg,
				Lua_Lui_LListView.reg,
				Lua_Lui_LPageView.reg,
				Lua_Lui_LRichText.reg,
				Lua_LWindowManager.reg,
				Lua_Custom.reg,
				Lua_Deleg.reg,
				Lua_foostruct.reg,
				Lua_FloatEvent.reg,
				Lua_ListViewEvent.reg,
				Lua_SLuaTest.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_XXList.reg,
				Lua_AbsClass.reg,
				Lua_HelloWorld.reg,
				Lua_System_Collections_Generic_Dictionary_2_int_string.reg,
				Lua_System_String.reg,
			};
			return list;
		}
	}
}
