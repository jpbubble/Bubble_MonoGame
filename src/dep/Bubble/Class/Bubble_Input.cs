// Lic:
// Bubble for MonoGame
// Input API
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
// Version: 19.06.01
// EndLic



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyUnits;
using Microsoft.Xna.Framework.Input;

namespace Bubble {
    class Bubble_Input {
        static public void Init(string state) => new Bubble_Input(state);
        private Bubble_Input(string state) {
            var script = QuickStream.StringFromEmbed("Input.lua");
            SBubble.State(state).state["Bubble_Input"] = this;
            SBubble.DoNIL(state,"#macro MouseLeft 1\n#macro MouseRight 2\n#macro MouseCenter 3","Mouse Button macros");
            SBubble.State(state).DoString(script, "Input API header");
        }

        public int X => FlowManager.MS.X;
        public int Y => FlowManager.MS.Y;
        public bool Held(byte b) {
            switch (b) {
                case 1:
                    return FlowManager.MS.LeftButton == ButtonState.Pressed;
                case 2:
                    return FlowManager.MS.RightButton == ButtonState.Pressed;
                case 3:
                    return FlowManager.MS.MiddleButton == ButtonState.Pressed;
                default:
                    SBubble.MyError("HEY!", "I don't know what you mean by mouse button #{b}", "There's only 1,2 and 3!");
                    return false;

            }
        }

        Dictionary<string, Keys> keyname2code = new Dictionary<string, Keys>();
        public bool HeldKey(string name) {
            name = name.ToUpper();
            if (!keyname2code.ContainsKey(name)) {
                var found = false;
                foreach(Keys k in (Keys[]) Enum.GetValues(typeof(Keys))) {
                    var a = $"{k}".ToUpper();
                    if (name==a) {
                        keyname2code[name] = k;
                        found = true;
                    }
                }
                if (!found)
                    SBubble.MyError("Keyboard error", $"No key named {name} appears to exist, or is at least not named to this engine!", "");
            }            
            return TQMGKey.Held(keyname2code[name]);            
        }

        public int KeyCode => (int)TQMGKey.GetKey();
        public string KeyName => $"{TQMGKey.GetKey()}";
        public string KeyChar => $"{TQMGKey.GetChar()}";
        public byte KeyByte => (byte)TQMGKey.GetChar();
    }
}



