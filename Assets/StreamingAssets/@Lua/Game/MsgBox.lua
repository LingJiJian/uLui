-- 消息窗
local MsgBox = LDeclare("MsgBox", LClass("MsgBox"))

function MsgBox:Start()

	print("MsgBox Start")


	self.btn_close = self.gameObject.transform:Find("btn_close"):GetComponent(Button)
	self.btn_close.onClick:AddListener(function()
		self.gameObject:GetComponent(Image).sprite = nil

		LWindowManager.GetInstance():popWindow("Prefabs/MsgBox.prefab")


	end)
end

function MsgBox:OnDestroy()

	LLoadBundle.GetInstance():UnloadBundles({
		"Ab/atlas-face01.ab",
		"Ab/scenes-first_unity.ab",
		"Ab/prefabs-msgbox_prefab.ab"})
end

return MsgBox