// Lic:
// Bubble for MonoGame
// Callback based flow
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
// Version: 19.05.14
// EndLic

#undef CallBackTrack

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLua;

namespace Bubble {
    class FlowCallBack : HardFlowClass {

        readonly public static FlowCallBack me = new FlowCallBack();
        protected Lua State => SBubble.State(FlowManager.CurrentFlow).state;
        Dictionary<Keys, bool> keyhit = new Dictionary<Keys, bool>();
        bool MSLeft = false;
        bool MSRight = false;
        bool MSCenter = false;
        string oldMS = "";

        public FlowCallBack() {
            foreach (Keys key in (Keys[])Enum.GetValues(typeof(Keys))) keyhit[key] = false;
        }

        public override void Update(GameTime gameTime) {
            var cmd = new StringBuilder("(BUB_Update or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")();\n"); // Please note, due to the odd syntax, Lua DOES require a semi-colon this time.
            var pressed = FlowManager.KB.GetPressedKeys();
            var mousepos = $"{FlowManager.MS.X},{FlowManager.MS.Y}";
            // keyboard
            foreach (Keys key in (Keys[])Enum.GetValues(typeof(Keys))) {
                if (FlowManager.KB.IsKeyDown(key) && !keyhit[key]) {
                    cmd.Append("(BUB_KeyPressed or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\""); cmd.Append(key.ToString()); cmd.Append("\", "); cmd.Append((int)key); cmd.Append(");\n");
                }
                if ((!FlowManager.KB.IsKeyDown(key)) && keyhit[key]) {
                    cmd.Append("(BUB_KeyReleased or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\""); cmd.Append(key.ToString()); cmd.Append("\", "); cmd.Append((int)key); cmd.Append(");\n");
                }
                keyhit[key] = FlowManager.KB.IsKeyDown(key);
            }
            // Mouse Left
            if (FlowManager.MS.LeftButton == ButtonState.Pressed && !MSLeft) { cmd.Append("(BUB_MousePressed or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Left\", "); cmd.Append(mousepos); cmd.Append(");\n"); }
            if (FlowManager.MS.LeftButton == ButtonState.Released && MSLeft) { cmd.Append("(BUB_MouseReleased or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Left\", "); cmd.Append(mousepos); cmd.Append(");\n"); }
            MSLeft = FlowManager.MS.LeftButton == ButtonState.Pressed;
            // Mouse Right
            if (FlowManager.MS.RightButton == ButtonState.Pressed && !MSRight) { cmd.Append("(BUB_MousePressed or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Right\", "); cmd.Append(mousepos); cmd.Append(");\n"); }
            if (FlowManager.MS.RightButton == ButtonState.Released && MSRight) { cmd.Append("(BUB_MouseReleased or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Right\", "); cmd.Append(mousepos); cmd.Append(";)\n"); }
            MSRight = FlowManager.MS.RightButton == ButtonState.Pressed;
            // Mouse Center
            if (FlowManager.MS.MiddleButton == ButtonState.Pressed && !MSCenter) { cmd.Append("(BUB_MousePressed or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Center\", "); cmd.Append(mousepos); cmd.Append(");\n"); }
            if (FlowManager.MS.MiddleButton == ButtonState.Released && MSCenter) { cmd.Append("(BUB_MouseReleased or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")(\"Center\", "); cmd.Append(mousepos); cmd.Append(");\n"); }
            MSCenter = FlowManager.MS.MiddleButton == ButtonState.Pressed;
            // Mouse Move
            if (mousepos != oldMS) {
                cmd.Append("(BUB_MouseMove or "); cmd.Append(FlowManager.NOTHING); cmd.Append(")("); cmd.Append(mousepos); cmd.Append(");\n");
                oldMS = mousepos;
            }
            try {
                State.DoString(cmd.ToString(), "Update/Events");
            } catch (Exception error) {
#if CallBackTrack
                SBubble.MyError("Callback error", error.Message, cmd.ToString());
#else
                SBubble.MyError("Callback error", error.Message, SBubble.TraceLua(SBubble.TraceLua(FlowManager.CurrentFlow)));
#endif
            }

        }

        public override void Draw(GameTime gameTime) {
            var cmd = $"(BUB_Draw or {FlowManager.NOTHING})()";
            try {
                //BubConsole.CSay("Draw Call");
                //BubConsole.CSay(FlowManager.CurrentFlow);
                State.DoString(cmd, "Draw");
                //BubConsole.CSay("End Draw Call");
            } catch (Exception error) {
#if CallBackTrack
                SBubble.MyError("Callback error", error.Message, cmd.ToString());
#else
                SBubble.MyError("Callback error", error.Message, SBubble.TraceLua(FlowManager.CurrentFlow));
#endif
            }
        }
    }
}

