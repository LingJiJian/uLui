--
-- Lua version parse file.
--
-- @filename  Version.lua

-- Get global declare.
local LDeclare = require "Base/LGlobal"

-- Get current version number.
local _, _, majorv, minorv, rev = string.find(_VERSION, "(%d).(%d)[.]?([%d]?)")
local VersionNumber = tonumber(majorv) * 100 + tonumber(minorv) * 10 + (((string.len(rev) == 0) and 0) or tonumber(rev))

-- Declare current version number.
LDeclare("L_VERSION", VersionNumber)

-- Declare lua history version number.
LDeclare("L_VERSION_510", 510)
LDeclare("L_VERSION_520", 520)
LDeclare("L_VERSION_530", 530)

return L_VERSION
