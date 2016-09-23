local Util = {}

-- 清空子节点
Util.removeAllChild = function(gameObject)
	
	local len = gameObject.transform.childCount
	local obj = nil
	for i=0,len-1 do
		obj = gameObject.transform:GetChild(i).gameObject
		Object.Destroy(obj)
	end

end

-- 切割字符串
Util.split = function(s, p)
    local rt= {}
    string.gsub(s, '[^'..p..']+', function(w) table.insert(rt, w) end )
    return rt
end

-- 用标签获取组件参数
Util.GetComponentWithFlag = function(obj,flag,otype)
	if obj == nil then return end
	
	local links = obj:GetComponents(Link)
	for v in Slua.iter(links) do
		if v.flag == flag then

			if otype then
				return v.target:GetComponent(otype)
			end
			return v.target
		end
	end
	return nil
end

Util.FindComponentFlag = function(obj,str)
	local links = obj:GetComponents(Link)
	for v in Slua.iter(links) do
		local a,b = string.find(v.flag,str)
		if a ~=nil and b ~=nil then
			return string.sub(v.flag,b+1)
		end 
	end
	return nil
end

Util.AnimatorIsPlaying = function(animator,name)
	local ret = false
	if name then
		local stateInfo = animator:GetCurrentAnimatorStateInfo(0);
	    if stateInfo:IsName("Base Layer.".. name) and stateInfo.normalizedTime < 1 then
			ret = true
		end
	end
	return ret
end

LDeclare("Util", Util)

return Util