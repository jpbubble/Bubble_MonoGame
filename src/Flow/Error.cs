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
