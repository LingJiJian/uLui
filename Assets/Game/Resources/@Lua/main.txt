import "UnityEngine"
import "UnityEngine.Object"
import "UnityEngine.UI"
import "UnityEngine.SceneManagement"
import "Lui"
require "import"

local function main() 
	print("初始化游戏")

	NetworkManager:GetInstance()

	LLoadBundle.GetInstance():LoadAllBundles({
		"scenes-first_unity.ab",
		"scenes-second_unity.ab",
		"atlas-digit1.ab",
		"atlas-face01.ab",
		"atlas-face02.ab",
		"atlas-mytest.ab",
		"prefabs-msgbox_prefab.ab",
		"prefabs-windowgridview_prefab.ab",
		"prefabs-tbl_cell_prefab.ab",
		"prefabs-list_cell_prefab.ab",
		"prefabs-grid_cell_prefab.ab",
		"prefabs-page_cell_prefab.ab"
	},function()

		local lab_progress = GameObject.Find("lab_progress"):GetComponent(Text)

		LWindowManager:GetInstance():LoadSceneAsync("first",function( p )
			lab_progress.text = string.format("初始化需要一点点时间(%d/100)",p)
		end)

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
