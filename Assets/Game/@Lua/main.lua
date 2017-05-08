import "UnityEngine"
import "UnityEngine.Object"
import "UnityEngine.UI"
import "UnityEngine.SceneManagement"
import "Lui"
require "import"

local function main() 
	print(i18n["1001"])

	-- NetworkManager:GetInstance()

	LLoadBundle.GetInstance():LoadAllBundles({
		"scenes-first_unity.ab",
		"scenes-second_unity.ab",
		"atlas-num_png.ab",
		"atlas-face_png.ab",
		"atlas-common_png.ab",
		"prefabs-msgbox_prefab.ab",
		"prefabs-windowgridview_prefab.ab",
		"prefabs-list_cell_prefab.ab",
		"prefabs-grid_cell_prefab.ab",
		"prefabs-page_cell_prefab.ab"
	},function()

		local lab_progress = GameObject.Find("lab_progress"):GetComponent(Text)

		LWindowManager:GetInstance():LoadSceneAsync("first",function( p )
			lab_progress.text = string.format(i18n["1002"],p)
		end)

	end,nil)

end

-- Declare global function.
LDeclare("main", main)

return main
