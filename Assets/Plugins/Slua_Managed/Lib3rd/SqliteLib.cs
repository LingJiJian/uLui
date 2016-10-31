using System.Collections.Generic;
using LuaInterface;



namespace SLua
{
	//sqlite lib
	public class SqliteLib
	{
		public static void Reg(Dictionary<string, LuaCSFunction> reg_functions)
		{
			reg_functions.Add("lsqlite3",LuaDLL.luaopen_lsqlite3);
		}
	}
}