local MsgId = {}

MsgId.hello = 1;
MsgId.registerApp = 2;
MsgId.broadcastApp = 3;
MsgId.updateApp = 4;
MsgId.webAppCmd = 1000;
MsgId.adminAppCmd = 1001;



LDeclare("MsgId", MsgId)
return MsgId