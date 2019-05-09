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

ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TrickyUnits;

namespace Bubble {
    class Error:HardFlowClass {

        static TQMGImage Death = null;

        static public void GoError(string ct, string message, string trace) {
            var s = QuickStream.OpenEmbedded("Death.png");
            if (s == null) Debug.WriteLine("ERROR! Trying to read Death resulted into null!");
            s.Position = 0;
            Death = TQMG.GetImage(s);
            FlowManager.GoHardFlow(new Error());
        }

        private Error() { }

        public override void Draw(GameTime gameTime) {
            TQMG.Color(0, 18, 25);
            TQMG.DrawRectangle(0, 0, TQMG.ScrWidth, TQMG.ScrHeight);
            TQMG.Color(0, 36, 50);
            Death.Draw(0, 0);
        }

        public override void Update(GameTime gameTime) {
            throw new NotImplementedException();
        }
    }
}

