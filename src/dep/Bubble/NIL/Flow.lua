--[[

   FLOW MANGER GLUE
   
]]

function GetFlow() return Bubble_Flow.State end
function LoadFlow(flow,file) Bubble_Flow:LoadFlow(flow,file) end
function GoToFlow(flow) Bubble_Flow:GoToFlow(flow) end
function StartFlow() Bubble_Flow:StartFlow() end
GotoFlow=GoToFlow
NewFlow=LoadFlow


function LoadState(state,file) Bubble_Flow:LoadState(state,file) end
function KillState(state) Bubble_Flow:KillState(state) end

function LuaDoString(state,script,chunk) Bubble_Flow:DoStateLua(state,script,chunk or "LUA:DOSTATE") end
function NILDoString(state,script,chunk) Bubble_Flow:DoStateNIL(state,script,chunk or "NIL:DOSTATE") end

-- Please note, this should ONLY be used for function calls
function LuaGetInt   (state,call,chunk) return Bubble_Flow:GetIntLua   (state,call, chunk or "LUA.GETINT")    end
function LuaGetString(state,call,chunk) return Bubble_Flow:GetStringLua(state,call, chunk or "LUA.GETSTRING") end
function LuaGetBool  (state,call,chunk) return Bubble_Flow:GetBoolLua  (state,call, chunk or "LUA.GETBOOL")   end
-- This way of working can only be done with numbers, strings and bools, and for numbers I chose to only do this with integers to prevent rouding conflicts.
-- Tables, functions and userdata cannot be transferred this way, as they are pointer based and stuck to their respective states, trying to transfer them,
-- is looking for trouble, actually.

function KillFlow(flow) KillState("FLOW_"..flow) end

function StateExists(state) 
	--CSay("Glue: Checking state "..(state or "<<NIL>>"))
	local ret = Bubble_Flow:StateExists(state or "nil") 
	--if ret then CSay('Lua received: true') else CSay('Lua received: false') end
	--CSay(("Received %d!"):format(ret))
	return ret == 1
end

function CurrentFlow() return Bubble_Flow.CurrentFlow end

function Bye()
	Bubble_Flow:Bye()
end


print("Flow Manager set up")
