// Lic:
// Bubble for MonoGame (OpenGL)
// Flow Manager
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 19.08.02
// EndLic


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrickyUnits;
using UseJCR6;
using NLua;



namespace Bubble {

    abstract class HardFlowClass {
        abstract public void Draw(GameTime gameTime);
        abstract public void Update(GameTime gameTime);
    }

    
    class APIFlow {

        public readonly string State;
        

        private APIFlow(string state) {
            State = state;
            var s = SBubble.State(state).state;
            s["Bubble_Flow"] = this;
            SBubble.State(state).DoString(QuickStream.StringFromEmbed("Flow.lua"), "Flow Management");
        }

        public void GoToFlow(string flow) {
            flow = flow.ToUpper();
            if (!qstr.Prefixed(flow, "FLOW_")) flow = $"FLOW_{flow}";
            if (!SBubble.HaveState(flow))
                SBubble.MyError("Flow Management Error", $"GoToFlow: Flow {flow} doesn't exist!", SBubble.TraceLua(State));
            else {
                FlowManager.CurrentFlow = flow;
                BubConsole.WriteLine($"Flow set to: {flow}");
                SBubble.State(flow).DoString("if BUB_Arrive then assert(type(BUB_Arrive)=='function','BUB_Arrive must be a function but it is a '..type(BUB_Arrive)) BUB_Arrive() end");
            }
        }

        public void LoadFlow(string flow,string file) {
            flow = flow.ToUpper();
            if (!qstr.Prefixed(flow, "FLOW_")) flow = $"FLOW_{flow}";
            SBubble.NewState(flow, file);
            BubConsole.WriteLine($"Created new flow: {flow}");
            SBubble.State(flow).state.DoString("if BUB_Load then BUB_Load() end");
        }

        public void KillFlow(string flow) {
            flow = flow.ToUpper();
            if (!qstr.Prefixed(flow, "FLOW_")) flow = $"FLOW_{flow}";
            if (SBubble.HaveState(flow))
                SBubble.MyError("Flow Management Error", $"KillFlow: Flow {flow} doesn't exist!", SBubble.TraceLua(State));
            SBubble.KillState(flow);
        }

        public void StartFlow() {
            if (FlowManager.CurrentFlow == "") throw new Exception("Before you can start the flow sequence\nA flow must be loaded and activated");
            switch (SBubble.RunMode) {
                case "CB":
                    FlowManager.GoHardFlow(FlowCallBack.me);
                    break;
                case "RF":
                    FlowManager.GoHardFlow(FlowRepeat.me);
                    break;
                default:
                    throw new Exception($"Unknown runmode ({SBubble.RunMode})");
            }
        }

        public void LoadState(string s,string file) {
            s = s.ToUpper();
            if (qstr.Prefixed(s, "FLOW_")) throw new Exception("Non-Flow states may NOT be prefixed with FLOW_");
            SBubble.NewState(s,file);
            SBubble.State(s).state.DoString("if BUB_Load then BUB_Load() end");
        }

        public void DoStateLua(string s, string script,string chunk="LUA:DOSTATE") {
            try {
                SBubble.State(s).DoString(script, chunk);
            } catch (Exception Uitzondering) {
                SBubble.MyError($"LuaDoString(\"{s}\",\"{script}\",\"{chunk}\"):", Uitzondering.Message, "");
            }
        }

        public void DoStateNIL(string s, string script, string chunk = "NIL:DOSTATE") {
            try { 
            SBubble.DoNIL(s, script, chunk);
            } catch (Exception Uitzondering) {
                SBubble.MyError($"NILDoString(\"{s}\",\"{script}\",\"{chunk}\"):", Uitzondering.Message, "");
            }
        }

        public int GetIntLua(string state,string call,string chunk = "LUA.GETINT") {
            try {
                var O = SBubble.State(state).DoString($"return {call}", chunk);
                if (O.Length == 0) {
                    SBubble.MyError("GetIntLuaError", "Nothing has been properly returned!", "");
                    return 0;
                } else {
                    return Convert.ToInt32(O[0]);
                }
            } catch (Exception Klote) {
                SBubble.MyError($"LuaGetInt(\"{state}\",\"{call}\",\"{chunk}\"):", Klote.Message, "");
                return 0;
            }
        }

        public string GetStringLua(string state, string call, string chunk = "LUA.GETSTRING") {
            try {
                var O = SBubble.State(state).DoString($"return {call}", chunk);
                if (O==null || O.Length == 0) {
                    SBubble.MyError("GetStringLuaError", "Nothing has been properly returned!", "");
                    return "";
                } else {
                    return (string)O[0];
                }
            } catch ( Exception Kut) {
                SBubble.MyError($"LuaGetString(\"{state}\",\"{call}\",\"{chunk}\"):", Kut.Message, "");
                return "Kilo Utrecht Tango";
                
            }
        }
        public bool GetBoolLua(string state, string call, string chunk = "LUA.GETBOOL") {
            var O = SBubble.State(state).DoString($"return {call}", chunk);
            if (O.Length == 0) {
                SBubble.MyError("GetBoolLuaError", "Nothing has been properly returned!", "");
                return false;
            } else {
                return (bool)O[0];
            }
        }


        public void KillState(string s) {
            s = s.ToUpper();
            //if (qstr.Prefixed(s, "FLOW_")) SBubble.MyError("KILL Error","Non-Flow states may NOT be prefixed with FLOW_","");
            SBubble.KillState(s);
        }

        public int StateExists(string s) {
            var r = SBubble.HaveState(s);
            //BubConsole.CSay($"Sending outcome {r} to Lua!");
            if (r) return 1;
            return 0;
        }

        public string CurrentFlow => FlowManager.CurrentFlow;
        

        static public void Init(string state) {
            new APIFlow(state);
        }
        

        public void Bye() {
            BubConsole.CSay("Bye request initiated!");
            FlowManager.TimeToDie = true;
        }
        
        
    }

    

    static class FlowManager {

        public const string NOTHING = "BUB_NOTHING_AT_ALL_THIS_MAY_BE_A_STUPID_NAME_BUT_I_NEEDED_TO_MAKE_SURE_THIS_LINE_WAS_NOT_USED_FOR_ANYTHING_ELSE_CAPICHE";

        static Dictionary<string, HardFlowClass> HardFlow = new Dictionary<string, HardFlowClass>();
        //static Dictionary<string, SoftFlowClass> SoftFlow = new Dictionary<string, SoftFlowClass>();
        static public HardFlowClass HFC = null;
        static public bool TimeToDie = false;
        static public KeyboardState KB;
        static public MouseState MS;
        static public string CurrentFlow = ""; // for soft flow!

         

        static public TimeSpan Time { get; private set; }

        static public void GoHardFlow(HardFlowClass Flow,bool force=false) {
            if ((!Error.blocked) || force)
                HFC = Flow;
        }

        static public void GoHardFlow(string Flow) {
            if (HardFlow.ContainsKey(Flow))
                HFC = HardFlow[Flow];
            else
                HFC = null;
        }

        static public void Draw(GameTime gt) {
            if (HFC != null) HFC.Draw(gt);
        }

        static public void Update(GameTime gt) {
            try {
                Time = gt.ElapsedGameTime;
                MS = Mouse.GetState();
                KB = Keyboard.GetState();
                TQMGKey.Start(FlowManager.KB);
                if (HFC != null) HFC.Update(gt);
            } catch (Exception Whatever) {
                SBubble.MyError("Flow.Update():", Whatever.Message, "");
            }
        }

        static public void StartInitFlow() {
            BubConsole.CSay("Looking for Init script!");
            var mainfile = "";
            foreach (string e in SBubble.ResFiles) {
                var ce = e.ToUpper();
                if (ce == "INIT.LUA" || qstr.Suffixed(ce, "/INIT.LUA") || ce=="INIT.NIL" || qstr.Suffixed(ce, "/INIT.NIL"))
                    mainfile = e;
            }
            if (mainfile == "")
                Error.GoError("Startup error", "INIT.LUA or INIT.NIL required somewhere in any folder.", "None have been found");
            else {
                //mainfile = "Script/Viezelul.lua";
                GoHardFlow(BubConsole.Flow);
                try {
                    BubConsole.WriteLine($"Loading Init Script: {mainfile}");
                    SBubble.NewState("$INIT", mainfile);
                    //SBubble.State("$INIT").state.DoString("local fc = ''\nfor k,v in pairs(_G) do\n fc=fc .. type(v) .. ' ' ..k..'\\n'\n end\n error(fc)"); // debug line
                    SBubble.State("$INIT").state.DoString("assert(type(Bubble_Init)=='function','Function expected for Bubble_Init, got '..type(Bubble_Init))\nBubble_Init()");
                } catch (Exception e) {
                    Error.GoError("Init Error", e.Message,"");
                }
            }
        }

        static public void NewSoftFlow(string tag,string scriptfile) {
            //BubConsole.CSay($"Starting new state {tag} with script {scriptfile}");
            tag = tag.ToUpper();
            if (!qstr.Prefixed(tag, "FLOW_")) tag = $"FLOW_{tag}";
            SBubble.NewState(tag, scriptfile);
            try {
                SBubble.State(tag).DoString($"(BUB_Load or {NOTHING})()", "LOAD");
            } catch (Exception NotAnExceptionJustAnErrorBozos) {
                SBubble.MyError("Load Error", NotAnExceptionJustAnErrorBozos.Message, "");
            }
        }

        static public void KillSoftFlow(string tag) {
            tag = tag.ToUpper();
            if (!qstr.Prefixed(tag, "FLOW_")) tag = $"FLOW_{tag}";
            if (SBubble.HaveState(tag))
                SBubble.MyError($"KillFlow(\"{tag}\"):", "Tag non-existent", "");
            else
                SBubble.KillState(tag);
        }

    }
}









