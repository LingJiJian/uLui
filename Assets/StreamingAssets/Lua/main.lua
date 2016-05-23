import "UnityEngine"
import "UnityEngine.Object"
import "UnityEngine.UI"
import "UnityEngine.SceneManagement"
import "Lui"
require "import"

local function main()
	print("初始化游戏")

	LLoadBundle.GetInstance():LoadAllBundles({"scenebundles","prefabbundles"},function()
		local wm = LWindowManager.GetInstance();
		wm:runWindow("MsgBox", 1)
	end)
end

-- Declare global function.
LDeclare("main", main)

return main
