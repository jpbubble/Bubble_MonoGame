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
Version 19.06.01
]]



Mouse = {}
Keyboard = {}
-- TODO: Joystick/Joypad


local function showmouse()
	local MousePointer = TImage.Obtain("MOUSEPOINTER")
	MousePointer.Draw(Mouse.X,Mouse.Y)
end

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
		elseif key=="SHOW" then
			return showmouse
		elseif key=="POINTER" then
			return MousePointer
		else
			BubbleCrash("Mouse."..k.." does not exist!")
		end
	end,

	__newindex = function(t,k,v)
		local key=k:upper()
		if key=="POINTER" then 
			LoadImage(v,"MOUSEPOINTER");
			CSay(("Mouse pointer image changed to '%s'!"):format(v))
			--if not MousePointer then CSay("No mouse pointer is now there, though, so Mouse.Show() will be ignored!") end
		else
			BubbleCrash("Mouse."..k.." is either read-only or non-existent!")
		end
	end

}
setmetatable(Mouse,MetaMouse)


local MetaKeyboard = {
	__index = function(t,k)
		local key = k:upper()
		if key=="NAME" then
			return Bubble_Input.KeyName;
		elseif key=="CODE" then
			return Bubble_Input.KeyCode;
		elseif key=="CHAR" then
			return Bubble_Input.KeyChar;
		elseif key=="BYTE" then
			return Bubble_Input.KeyByte;
		elseif key=="HELD" then
			return function(b)
				if type(b)=='string' then
			       return Bubble_Input:HeldKey(b)
				else
					return nil
				end
			end
		else
			BubbleCrash("I don't know what Keyboard."..k.." is supposed to mean!")
		end
	end
}
setmetatable(Keyboard,MetaKeyboard)

CSay("Mouse and Keyboard support present")



