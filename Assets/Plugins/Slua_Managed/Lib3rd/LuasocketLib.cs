using System.Collections.Generic;
using LuaInterface;



namespace SLua
{
	//luasocket lib
	public class LuasocketLib
	{
		public static void Reg(Dictionary<string, LuaCSFunction> reg_functions)
		{
			reg_functions.Add("socket.core",LuaDLL.luaopen_socket_core);
			reg_functions.Add("mime.core",LuaDLL.luaopen_mime_core);
		}
	}
}