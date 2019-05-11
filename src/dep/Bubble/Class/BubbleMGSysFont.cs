// Lic:
// Bubble for MonoGame
// System font manager
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
using System.Diagnostics;
using System.Text;
using TrickyUnits;

namespace Bubble {
    class SysFont {
        static Dictionary<byte, TQMGImage> CharPics = new Dictionary<byte, TQMGImage>();
        static int fw = 0;
        static int fh = 0;

        static public void DrawText(string text,int x, int y) {
            var dx = x;
            var dy = y;
            for(int i = 0; i < text.Length; i++) {
                var b = (byte)text[i];
                if (b == 32)
                    dx += fw;
                else if (b == 10) {
                    dx = x;
                    dy += fh;
                } else if (b > 32 && b < 127) {
                    if (!CharPics.ContainsKey(b)) {
                        var q = QuickStream.OpenEmbedded($"SysFont.{b}.png");
                        if (q != null) {
                            CharPics[b] = TQMG.GetImage(q);
                            Debug.WriteLine($"Loaded character {b} => {text[i]}");
                        }
                    }
                    if (CharPics.ContainsKey(b)) { // NO ELSE! That won't cause the desired effect
                        try {
                            var cp = CharPics[b];
                            cp.Draw(dx, dy);
                            if (fw < cp.Width) fw = cp.Width;
                            if (fh < cp.Height) fh = cp.Height;
                            dx += fw;
                            if (dx + fw > TQMG.ScrWidth) {
                                dx = x;
                                dy += fh;
                            }
                        } catch (Exception E) {
                            Debug.Print($"Caught: {E.Message}");
                        }
                    }
                }
            }
        }

        delegate void updatetype();
        static public void TextSizes(string text, ref int w, ref int h, int x=0) {
            var dx = x;
            var dy = 0;
            // Needed due to C#'s primitive nature!
            var tw = w;
            var th = h;
            void update() {
                if (dx > tw) tw = dx;
                if (dy > th) th = dy+fh;
            }           
            for (int i = 0; i < text.Length; i++) {
                var b = (byte)text[i];
                if (b == 32) {
                    dx += fw;
                    update();
                } else if (b == 10) {
                    dx = x;
                    dy += fh;
                    update();
                } else if (b > 32 && b < 127) {
                    if (!CharPics.ContainsKey(b)) {
                        var q = QuickStream.OpenEmbedded($"SysFont.{b}.png");
                        if (q != null) {
                            CharPics[b] = TQMG.GetImage(q);
                            Debug.WriteLine($"Loaded character {b} => {text[i]}");
                        }
                    }
                    if (CharPics.ContainsKey(b)) { // NO ELSE! That won't cause the desired effect
                        try {
                            var cp = CharPics[b];
                            if (fw < cp.Width) fw = cp.Width;
                            if (fh < cp.Height) fh = cp.Height;
                            dx += fw;
                            update();
                            if (dx + fw > TQMG.ScrWidth) {
                                dx = x;
                                dy += fh;
                                update();
                            }
                        } catch (Exception E) {
                            Debug.Print($"Caught: {E.Message}");
                        }
                    }
                }
            }
            w = tw;
            h = th;
        }


    }

    class LuaSysFont {
        public void DrawText(string t, int x, int y) => SysFont.DrawText(t, x, y);
    }
}


