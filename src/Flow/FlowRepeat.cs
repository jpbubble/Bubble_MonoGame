// Lic:
// Bubble for MonoGame
// Repeatative Flow Mode
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
// Version: 19.07.14
// EndLic



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Bubble {
    class FlowRepeat:FlowCallBack {

        readonly public static FlowRepeat me = new FlowRepeat();

        public override void Update(GameTime gameTime) {
            try {
                State.DoString($"(BUB_Update or {FlowManager.NOTHING})()", "Draw");
            } catch (Exception err) {
                var trace = SBubble.TraceLua(FlowManager.CurrentFlow); //trace = State.GetDebugTraceback();
#if DEBUG
                trace += $"\n\n{err.StackTrace}";
#endif
                SBubble.MyError("Update Callback error", err.Message, trace);
            }
        }
    }
}



