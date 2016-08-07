-- 网络管理
local NetworkManager = LDeclare("NetworkManager", LClass("NetworkManager"))
NetworkManager._instance = nil
NetworkManager._netWork = nil

function NetworkManager:GetInstance()

	if NetworkManager._instance == nil then
		NetworkManager._instance = NetworkManager()
		NetworkManager._instance:init()
	end

	return NetworkManager._instance
end

function NetworkManager:init()

    require "Lib/protobuf/protobuf"

    -- self.ip = "192.168.1.39"
    -- self.port = 20096
    self.ip = "localhost"
    self.port = 9001

	self._netWork = Network.GetInstance()
	self._netWork.onConnect = function(isConn)
	 	print("onConnect:" , isConn);

	 	self.isConn = isConn
		-- local component_hello = {
		-- 	appId = 123,
		-- 	appType = 12345,
		-- 	groupId = 15615
		-- }

		-- self:send(MsgId.hello,component_hello)
	 	LMessage:dispatchEvent(EventNames.NET_CONNECT,isConn)
	end

	self._netWork.onHandleMessage = function(msgId,packet)
		print("onHandleMessage")
		local decode = protobuf.decode(string.format("component.component_%s",self.msgIdsName[msgId]),packet)
		LMessage:dispatchEvent(EventNames.MSG_PROTO,{id=msgId,obj=decode})
	end

	self._netWork.onDisconnect = function()
		print("onDisconnect")
		self.isConn = false

		LMessage:dispatchEvent(EventNames.NET_DISCONNECT)
	end

	self:proto()

	self._netWork:connect(self.ip, self.port);
end

function NetworkManager:proto()

	self.msgIdsName = {}
	for name,id in pairs(MsgId) do
		self.msgIdsName[id] = name
	end

	self.protoFile = "masterd"

	local buffer = self._netWork:GetProtoBytes(self.protoFile)
	protobuf.register(buffer)
end

function NetworkManager:send(msgId,protoObj)
	
	if self._netWork:valid() and self.isConn then

		local code = protobuf.encode(string.format("component.component_%s",self.msgIdsName[msgId]),protoObj)
		self._netWork:send(msgId,code)
	end
end


return NetworkManager