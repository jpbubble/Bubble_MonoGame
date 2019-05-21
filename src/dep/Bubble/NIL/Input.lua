--[[
src/dep/Bubble/NIL/Input.lua
Copyright (C)  Jeroen P. Broks
Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
Version 19.05.21
]]


Mouse = {}
Keyboard = {}
-- TODO: Joystick/Joypad

local MetaMouse = {
	__index = function(t,k)
		local key=k:upper()
		if key=="X" then
			return Bubble_Input.X
		elseif key=="Y" then
			return Bubble_Input.Y
		elseif key=="HELDLEFT" then
			return Bubble_Input:Held(1)
		elseif key=="HELDRIGHT" then
			return Bubble_Input:Held(2)
		elseif key=="HELDCENTER" then
			return Bubble_Input:Held(3)
		elseif key=="MOUSEINSIDE" then
			return Bubble_Input.X>=0 and Bubble_Input.Y>0 and Bubble_Input.X<Screen.Width and Bubble_Input.Y<Screen.Height
		else
			BubbleCrash("Mouse."..k.." does not exist!")
		end
	end

}
setmetatable(Mouse,MetaMouse)


local MetaKeyboard = {

}
setmetatable(Keyboard,MetaKeyboard)

CSay("Mouse and Keyboard support present")


