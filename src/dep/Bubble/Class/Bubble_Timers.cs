// Lic:
// src/dep/Bubble/Class/Bubble_Timers.cs
// Bubble Timers
// version: 19.11.16
// Copyright (C)  Jeroen P. Broks
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 1. The origin of this software must not be misrepresented; you must not
// claim that you wrote the original software. If you use this software
// in a product, an acknowledgment in the product documentation would be
// appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
// misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// EndLic
namespace Bubble {
	
	class BubbleTimer {
	
		const string Script = @"
			BubbleTimer = {}
			setmetatable(BubbleTimer,{
				__index = function(t,k)
					if k:upper()=='UPDATE' then return API_BUBBLE_TIMER.UPDATE
					elseif k:upper()=='DRAW' then return API_BUBBLE_TIMER.DRAW 
					elseif k:upper()=='SHOWTIME' then return API_BUBBLE_TIMER.SHOWTIME end					
				end,
				__newindex = function(t,k,v) 
					if k:upper()=='SHOWTIME' then
						assert(type(v)=='boolean','ShowTime is a boolean property.... Not a '..type(v)..' property!')
						API_BUBBLE_TIMER.SHOWTIME=v
					else
						error('BubbleTimer is a READ-ONLY table, so no values can be assigned to it!') 
					end
				end,
				__call = function(t) return API_BUBBLE_TIMER.UPDATE,API_BUBBLE_TIMER.DRAW end
			})
			";
			
		private BubbleTimer() {}
		string statename = "";
		
		static public void Init(string vm){
			var nieuw = new BubbleTimer();
			var state = SBubble.State(vm).state;
			nieuw.statename=vm;
			state["API_BUBBLE_TIMER"] = nieuw;
			state.DoString(Script, "Timer init script");
		}
		
		// Looks useless, but it isn't from Lua API perspective.
		// The value is always the same, but may only be written out 
		// by C#, yet Lua may read it out, but not write it. 
		// This was the only way to do it properly.
		public static int UpdateTime;
		public int UPDATE => UpdateTime; 
		public static int DrawTime;
		public int DRAW => DrawTime;
		public static bool ShowTime = false;
		public bool SHOWTIME {get=>ShowTime; set {ShowTime=value;}}
		
	}
}

