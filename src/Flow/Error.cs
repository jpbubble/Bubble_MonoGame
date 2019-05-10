// Lic:
// Bubble for MonoGame
// Error Management
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
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TrickyUnits;

namespace Bubble {
    class Error:HardFlowClass {

        static TQMGImage Death = null;
        static string sct, smsg, strace;
        static bool crashed = false;

        static public void GoError(string ct, string message, string trace) {
            Debug.WriteLine($"{ct}: {message}");
            if (crashed) return; crashed = true;
            var s = QuickStream.OpenEmbedded("Death.png");
            if (s == null) Debug.WriteLine("ERROR! Trying to read Death resulted into null!");
            s.Position = 0;
            Death = TQMG.GetImage(s);
            sct = ct;
            smsg = message;
            strace = trace;
            FlowManager.GoHardFlow(new Error());            
        }

        private Error() { }

        public override void Draw(GameTime gameTime) {
            TQMG.Color(0, 18, 25);
            TQMG.DrawRectangle(0, 0, TQMG.ScrWidth, TQMG.ScrHeight);
            TQMG.Color(0, 36, 50);
            Death.Draw(0, 0);
            TQMG.Color(255, 180, 100);
            SysFont.DrawText("OOPS!", 50, 0);
            TQMG.Color(255, 255, 0);
            SysFont.DrawText("You tried something we didn't think of!", 50, 25);
            TQMG.Color(0, 180, 255);
            SysFont.DrawText(sct, 50, 75);
            TQMG.Color(0, 200, 255);
            SysFont.DrawText(smsg, 50, 125);
            TQMG.Color(0, 220, 255);
            SysFont.DrawText(strace, 50, 175);
            TQMG.Color(0, 255, 255);
            SysFont.DrawText("Hit Escape to exit this application",50,TQMG.ScrHeight-30);

        }

        public override void Update(GameTime gameTime) {
            //Debug.WriteLine("Hello? Anybody home?");
            if (FlowManager.KB.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) FlowManager.TimeToDie = true;
        }
    }
}

