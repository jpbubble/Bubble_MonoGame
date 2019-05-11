// Lic:
// Bubble for MonoGame
// Graphics Library
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
// Version: 19.05.11
// EndLic


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyUnits;
using NLua;

namespace Bubble {
    class BubbleGraphics {

        static Dictionary<string, TQMGImage> Images = new Dictionary<string, TQMGImage>();

        static public void InitGraphics(string vm) {
            new BubbleGraphics(SBubble.State(vm).state,vm);
        }

        private BubbleGraphics(Lua state,string id) {
            var bt = QuickStream.OpenEmbedded("Graphics.nil");
            var script = bt.ReadString((int)bt.Size);
            bt.Close();
            state["BubbleGraphics"] = this;
            SBubble.DoNIL(id, script,"Graphics init script");
        }

        public string Load(string file, string assign = "") {
            var tag = assign;
            var at = 0;
            if (tag == "") do { at++; tag = $"IMAGE:{at}"; } while (Images.ContainsKey(tag));
            if (qstr.Suffixed(file.ToLower(), ".jpbf"))
                Images[tag] = TQMG.GetBundle(file);
            else
                Images[tag] = TQMG.GetImage(file);
            return tag;
        }

        public void HotCenter(string tag) => Images[tag].HotCenter();
        public void HotTopCenter(string tag) => Images[tag].HotTopCenter();
        public void HotBottomCenter(string tag) => Images[tag].HotBottomCenter();
        public int Height(string tag) => Images[tag].Height;
        public int Width(string tag) => Images[tag].Width;

        public bool HasTag(string tag) => Images.ContainsKey(tag);

        public void Draw(string tag,int x, int y, int frame) {
            if (Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(tag));
        }

        public void Free(string tag) {
            Images.Remove(tag);
        }

    }
}


