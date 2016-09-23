local LDeclare = require "Base/LGlobal"
local L_VERSION = require "Base/LVersion"

-- The hold all class type.
local __LClassTypeList = {}

-- The inherit class function.
local function __LClass(TypeName, SuperType)
    -- Create new class type.
    local ClassType = {}

    -- Set class type property.
    ClassType.TypeName = TypeName
    ClassType.SuperType = SuperType
    ClassType.ctor = false
    ClassType.dtor = false

    -- The new alloc function of this class.
    ClassType.new = function (...)
        -- Create a new object first and set metatable.
        local Obj = {}

        -- Give a tostring method.
        Obj.ToString = function (self)
            if not self.__InstanceName then
                local str = tostring(self)
                local _, _, addr = string.find(str, "table%s*:%s*(0?[xX]?%x+)")
                self.__InstanceName = ClassType.TypeName .. ":" .. addr
            end

            return self.__InstanceName
        end

        -- Get class type name.
        Obj.GetType = function (self)
            return ClassType.TypeName
        end

        -- Do constructor recursively.
        local CreateObj = function (Class, Object, ...)
            local Create
            Create = function (c, ...)
                if c.SuperType then
                    Create(c.SuperType, ...)
                end

                if c.ctor then
                    c.ctor(Object, ...)
                end
            end

            Create(Class, ...)
        end

        -- Do destructor recursively.
        local ReleaseObj = function (Class, Object)
            local Release
            Release = function (c)
                if c.dtor then
                    c.dtor(Object)
                end

                if c.SuperType then
                    Release(c.SuperType)
                end
            end

            Release(Class)
        end

        -- Do the destructor by lua version.
        if L_VERSION < L_VERSION_520 then
            -- Create a empty userdata with empty metatable.
            -- And mark gc method for destructor.
            local Proxy = newproxy(true)
            getmetatable(Proxy).__gc = function (o)
                ReleaseObj(ClassType, Obj)
            end

            -- Hold the one and only reference to the proxy userdata.
            Obj.__gc = Proxy

            -- Set metatable.
            setmetatable(Obj, {__index = __LClassTypeList[ClassType]})
        else
            -- Directly set __gc field of the metatable for destructor of this object.
            setmetatable(Obj, 
            {
                __index = __LClassTypeList[ClassType],

                __gc = function (o)
                    ReleaseObj(ClassType, o)
                end
            })
        end

        -- Do constructor for this object.
        CreateObj(ClassType, Obj, ...)
        return Obj
    end

    -- Give a ToString method.
    ClassType.ToString = function (self)
        return self.TypeName
    end

    -- The super class type of this class.
    if SuperType then
        ClassType.super = setmetatable({}, 
        {
            __index = function (t, k)
                local Func = __LClassTypeList[SuperType][k]
                if "function" == type(Func) then
                    t[k] = Func
                    return Func
                else
                    error("Accessing super class field are not allowed!")
                end
            end
        })
    end

    -- Virtual table.
    local Vtbl = {}
    __LClassTypeList[ClassType] = Vtbl
 
    -- Set index and new index of ClassType, and provide a default create method.
    setmetatable(ClassType,
    {
        __index = function (t, k)
            return Vtbl[k]
        end,

        __newindex = function (t, k, v)
            Vtbl[k] = v
        end,

        __call = function (self, ...)
            return ClassType.new(...)
        end
    })
 
    -- To copy super class things that this class not have.
    if SuperType then
        setmetatable(Vtbl,
        {
            __index = function (t, k)
                local Ret = __LClassTypeList[SuperType][k]
                Vtbl[k] = Ret
                return Ret
            end
        })
    end 
 
    return ClassType
end

-- Set into global.
if (not LIsDeclared("LClass")) or (not LClass) then
    LDeclare("LClass", __LClass)
end

-- Return this.
return __LClass