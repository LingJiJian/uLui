-- 通用事件派发器 static class
--[[
    eg: LMessage:addEvent(
            "eventName", 
            function(data) 
                print(data[1], data[2], data[3])  --1, 2, 3
            end
        )
        LMessage:dispatchEvent("eventName", {1, 2, 3})
]]

LEventData = LDeclare("LEventData", LClass("LEventData"))
LEventData.name = ""
LEventData.listener = nil
LEventData.target = nil

function LEventData:create()
    return LEventData.new()
end


LMessage = LDeclare("LMessage", LClass("LMessage"))

LMessage._eventDict = {}

--[[
    注册事件
    @param eventName 事件名
    @param listener 回调函数
    @param target 注册者(类似使用target:listener())
]]
function LMessage:addEvent(eventName, listener, target)

    assert(type(eventName) == "string" or eventName ~= "", "invalid event name")

    if not listener then
        return
    end

    local listeners = LMessage._eventDict[eventName] or {}
    LMessage._eventDict[eventName] = listeners

    for _, v in ipairs(listeners) do
        if v.listener == listener then
            return
        end
    end

    local event = LEventData:create()
    event.listener = listener
    event.name = eventName
    event.target = target

    table.insert(listeners, event)

end

function LMessage:removeEvent(eventName, listener)
    local listeners = LMessage._eventDict[eventName]
    if not listeners then
        return
    end

    -- local event

    -- local i = 1
    -- while i < #listeners do
    --     if event.listener == listener then
    --         table.remove(listeners, i)
    --         break
    --     end
    -- end
    
    for i, event in ipairs(listeners) do
        if event.listener == listener then
            table.remove(listeners, i)
            break
        end
    end
end

function LMessage:dispatchEvent(eventName, data)
    local listeners = LMessage._eventDict[eventName]
    if not listeners then
        return
    end

    for _, v in ipairs(listeners) do
        local callback = v.listener
        if v.target then
            callback(v.target, data)
        else
            callback(data)
        end
    end

end

function LMessage:removeAllEvent(eventName)
    LMessage._eventDict[eventName] = nil
end

function LMessage:hasEvent(eventName, listener)
    local listeners = LMessage._eventDict[eventName]
    if not listeners then
        return false
    end

    for _, event in ipairs(listeners) do
        if event.listener == listener then
            return true
        end
    end

    return false

end
