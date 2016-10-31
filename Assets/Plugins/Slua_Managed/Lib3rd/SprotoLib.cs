using System.Collections.Generic;
using LuaInterface;



namespace SLua
{
	//pbc lib
	public class SprotoLib
	{
		public static void Reg(Dictionary<string, LuaCSFunction> reg_functions)
		{
			reg_functions.Add("sproto.core",LuaDLL.luaopen_sproto_core);
		}
	}
}