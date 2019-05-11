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
    }

    class LuaSysFont {
        public void DrawText(string t, int x, int y) => SysFont.DrawText(t, x, y);
    }
}
