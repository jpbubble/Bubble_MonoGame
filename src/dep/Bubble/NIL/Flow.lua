--[[

   FLOW MANGER GLUE
   
]]

function GetFlow() return Bubble_Flow.State end
function LoadFlow(flow,file) Bubble_Flow:LoadFlow(flow,file) end
function GoToFlow(flow) Bubble_Flow:GoToFlow(flow) end
function StartFlow() Bubble_Flow:StartFlow() end
GotoFlow=GoToFlow
NewFlow=LoadFlow


print("Flow Manager set up")
