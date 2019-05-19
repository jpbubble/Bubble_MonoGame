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

function StateExists(state) 
	CSay("Glue: Checking state "..(state or "<<NIL>>"))
	local ret = Bubble_Flow:StateExists(state or "nil") 
	--if ret then CSay('Lua received: true') else CSay('Lua received: false') end
	CSay(("Received %d!"):format(ret))
	return ret == 1
end

print("Flow Manager set up")
