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
