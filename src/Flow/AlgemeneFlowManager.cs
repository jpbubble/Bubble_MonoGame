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
// Version: 19.05.09
// EndLic


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;



namespace Bubble {

    abstract class HardFlowClass {
        abstract public void Draw(GameTime gameTime);
        abstract public void Update(GameTime gameTime);
    }



    static class FlowManager {

        static Dictionary<string, HardFlowClass> HardFlow = new Dictionary<string, HardFlowClass>();
        static HardFlowClass HFC = null;
        static public bool TimeToDie = false;
        static public KeyboardState KB;
        static public MouseState MS;
         

        static public TimeSpan Time { get; private set; }

        static public void GoHardFlow(HardFlowClass Flow) {
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
            Time = gt.ElapsedGameTime;
            MS = Mouse.GetState();
            KB = Keyboard.GetState();
            if (HFC != null) HFC.Update(gt);
        }

        

    }
}


