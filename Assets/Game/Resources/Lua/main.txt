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

	-- LTextureAtlas:GetInstance():LoadData("mytest")

	-- local sp = LTextureAtlas:GetInstance():getSprite("cat_pic")
	-- local obj = GameObject()
	-- local imageComp = obj:AddComponent(Image)
	-- imageComp.sprite = sp
	-- imageComp:SetNativeSize();
	-- obj.transform:SetParent(GameObject.Find("Canvas").transform)

	-- Object.Destroy(obj)
	-- LTextureAtlas:GetInstance():RemoveTexture("test")



end

-- Declare global function.
LDeclare("main", main)

return main
