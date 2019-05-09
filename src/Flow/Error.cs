using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TrickyUnits;

namespace Bubble {
    class Error:HardFlowClass {

        static TQMGImage Death = null;

        static void GoError(string ct, string message, string trace) {
            Death = TQMG.GetImage(QuickStream.OpenEmbedded("Death.png"));
        }

        public override void Draw(GameTime gameTime) {
            TQMG.Color(0, 18, 25);
            TQMG.DrawRectangle(0, 0, TQMG.ScrWidth, TQMG.ScrHeight);
            Death.Draw(0, 0);
        }

        public override void Update(GameTime gameTime) {
            throw new NotImplementedException();
        }
    }
}
