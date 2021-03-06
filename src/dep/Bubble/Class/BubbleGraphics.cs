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
// Version: 19.11.16
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
        static Dictionary<string, TQMGFont> Fonts = new Dictionary<string, TQMGFont>();
        static Dictionary<string, TQMGText> Texts = new Dictionary<string, TQMGText>();

        static public void InitGraphics(string vm) {
            new BubbleGraphics(SBubble.State(vm).state,vm);
        }

        string vm = "";
        private BubbleGraphics(Lua state,string id) {
            var bt = QuickStream.OpenEmbedded("Graphics.nil");
            var script = bt.ReadString((int)bt.Size);
            vm = id;
            bt.Close();
            state["BubbleGraphics"] = this;
            SBubble.DoNIL(id, script,"Graphics init script");
            SBubble.State(id).DoString("Color = SetColor", "HACKING!"); // Prevent confusion since the console's not really accesible anyway
        }

        public string Load(string file, string assign = "") {
            try {
                var tag = assign;
                var at = 0;
                if (tag == "") do { at++; tag = $"IMAGE:{at}"; } while (Images.ContainsKey(tag));
                if (qstr.Suffixed(file.ToLower(), ".jpbf"))
                    Images[tag] = TQMG.GetBundle(file);
                else
                    Images[tag] = TQMG.GetImage(file);
                if (Images[tag] == null) throw new Exception($"Filed loading {file} at {tag}\n{UseJCR6.JCR6.JERROR}");
                return tag;
            } catch (Exception Catastrophe) {
#if DEBUG
                SBubble.MyError($"Bubble.Graphics.Images.Load(\"{file}\",\"{assign}\")", Catastrophe.Message, $"{SBubble.TraceLua(vm)}\n\n.NET Traceback:\n{Catastrophe.StackTrace}");
#else
                SBubble.MyError($"Bubble.Graphics.Images.Load(\"{file}\",\"{assign}\")", Catastrophe.Message, $"{SBubble.TraceLua(vm)}");
#endif
                return "Il ya une catastrophe";
            }
        }

        public string GrabScreen(string assign = "") {
            try {
                var tag = assign;
                var at = 0;
                if (tag == "") do { at++; tag = $"IMAGE:{at}"; } while (Images.ContainsKey(tag));
                Images[tag] = TQMG.GrabImage();
                if (Images[tag] == null) throw new Exception("Grabbed image == null!");
                return tag;
            } catch (Exception Catastrophe) {
#if DEBUG
                SBubble.MyError($"Bubble.Graphics.Images.Grab(\"{assign}\")", Catastrophe.Message, $"{SBubble.TraceLua(vm)}\n\n.NET Traceback:\n{Catastrophe.StackTrace}");
#else
                SBubble.MyError($"Bubble.Graphics.Images.Grab(\"{assign}\")", Catastrophe.Message, $"{SBubble.TraceLua(vm)}");
#endif
                return "Il ya une catastrophe";
            }
        }

        public string GrabPart(int x,int y, int w, int h, string assign = "") {
            try {
                var tag = assign;
                var at = 0;
                if (tag == "") do { at++; tag = $"IMAGE:{at}"; } while (Images.ContainsKey(tag));
                Images[tag] = TQMG.GrabImage(x,y,w,h);
                return tag;
            } catch (Exception Catastrophe) {
#if DEBUG
                SBubble.MyError($"Bubble.Graphics.Images.Grab(\"{assign}\",{x},{y},{w},{h})", Catastrophe.Message, $"{SBubble.TraceLua(vm)}\n\n.NET Traceback:\n{Catastrophe.StackTrace}");
#else
                SBubble.MyError($"Bubble.Graphics.Images.Grab(\"{assign}\",{x},{y},{w},{h})", Catastrophe.Message, $"{SBubble.TraceLua(vm)}");
#endif
                return "Il ya une catastrophe";
            }
        }

        public void Line(int x1, int y1, int x2, int y2) => TQMG.Line(x1, y1, x2, y2);
        public void Circle(int x, int y, double radius, double steps) => TQMG.Circle(x, y, radius, steps);
            
        

        public int ScrWidth => TQMG.ScrWidth;
        public int ScrHeight => TQMG.ScrHeight;

        public void SetViewPort(int x, int y, int w, int h) => TQMG.ViewPort(x, y, w, h);
        public void FullViewPort() => TQMG.ViewPortFull();
        public string GetViewPort() {
            var vp = TQMG.GetViewPort;
            return $"return {vp.X},{vp.Y},{vp.Width},{vp.Height}";
        }

        public int Frames(string tag) => Images[tag].Frames;

        public void HotCenter(string tag) => Images[tag].HotCenter();
        public void HotTopCenter(string tag) => Images[tag].HotTopCenter();
        public void HotBottomCenter(string tag) => Images[tag].HotBottomCenter();
        public void Hot(string tag, int x, int y) => Images[tag].Hot(x, y);
        public int Height(string tag) {
            //BubConsole.CSay($"Trying to get: {tag}");
            try {
                if (!Images.ContainsKey(tag)) {
                    SBubble.MyError("Image.Height():", $"Unknown tag: {tag}", SBubble.TraceLua(vm));
                    return 0;
                }
                return Images[tag].Height;
            } catch (Exception e) {
                SBubble.MyError(".NET error", e.Message,"");
                return 0;
            }
        }
        public int Width(string tag) => Images[tag].Width;

        public bool HasTag(string tag) => Images.ContainsKey(tag);

        public void Draw(string tag,int x, int y, int frame) {
            if (!Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(FlowManager.CurrentFlow));
            try {
                Images[tag].Draw(x, y, frame);
            } catch (Exception e) {
                SBubble.MyError($".NET: Draw(\"{tag}\",{x},{y},{frame}):", e.Message, $"FLOW:\n{SBubble.TraceLua(FlowManager.CurrentFlow)}");
            }
        }

        public void XDraw(string tag,int x, int y, int frame) {
            try {
                if (!Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(FlowManager.CurrentFlow));
                Images[tag].XDraw(x, y, frame);
            } catch (Exception KakkieDeKakkerlak) {
                SBubble.MyError($"XDraw(\"{tag}\", {x}, {y}, {frame}):", KakkieDeKakkerlak.Message, SBubble.TraceLua(FlowManager.CurrentFlow));
            }
        }

        public void StretchDraw(string tag,int x, int y, int w, int h, int frame) {
            try {
                if (!Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(FlowManager.CurrentFlow));
                Images[tag].StretchDraw(x, y, w,h,frame);
            } catch (Exception KakkieDeKakkerlak) {
                SBubble.MyError($"StretchDraw(\"{tag}\", {x}, {y}, {frame}):", KakkieDeKakkerlak.Message, SBubble.TraceLua(FlowManager.CurrentFlow));
            }
        }

        public int ScaleX { get => TQMG.ScaleX; set { TQMG.ScaleX = value; } }
        public int ScaleY { get => TQMG.ScaleY; set { TQMG.ScaleY = value; } }
        public int RotateDegrees { set => TQMG.RotateDEG(value); }
        public float RotateRadian { set => TQMG.RotateRAD(value); }

        public void Tile(string tag,int x,int y, int width,int height,int frame) {
            if (!Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(FlowManager.CurrentFlow));
            TQMG.Tile(Images[tag], x, y, 0, 0, width, height, frame);
        }

        public void ITile(string tag, int x, int y, int width, int height, int ix,int iy, int frame) {
            if (!Images.ContainsKey(tag)) SBubble.MyError("Bubble Graphics Error", $"There is no image tagged'{tag}'", SBubble.TraceLua(FlowManager.CurrentFlow));
            TQMG.Tile(Images[tag], ix, iy, x, y, width, height, frame);
        }

        public void Color(byte r,byte g, byte b) {
            TQMG.Color(r, g, b);
        }

        public string GetColor() {
            var c = TQMG.GetColor();
            return $"return {c.R}, {c.G}, {c.B}";
        }

        public byte Alpha {
            set => TQMG.SetAlpha(value);
            get => TQMG.GetAlpha();
        }

        public void Free(string tag) {
            Images.Remove(tag);
        }

        public void Rect(int x, int y, int w, int h,bool filled = true) {
            if (filled)
                TQMG.DrawRectangle(x, y, w, h);
            else
                TQMG.DrawLineRect(x, y, w, h);
        }

        public string LoadFont(string FontBundle, string assign = "") {
            var tag = assign;
            try {
                var at = 0;
                if (tag == "") do { at++; tag = $"FONT:{at}"; } while (Fonts.ContainsKey(tag));
                var font = TQMG.GetFont(FontBundle);
                Fonts[tag] = font ?? throw new Exception($"Failed loading {FontBundle} at {tag}\n{UseJCR6.JCR6.JERROR}");
                BubConsole.WriteLine($"Font bundle \"{FontBundle}\" loaded and assigned to {tag}", 180, 255, 0);
            } catch (Exception e) {
                SBubble.MyError("Font error!", e.Message, $"LoadImageFont(\"{FontBundle}\",\"{assign}\");\n\n{e.StackTrace}");
                BubConsole.WriteLine("ERROR!", 255, 0, 0);
                BubConsole.WriteLine(e.Message, 180, 0, 255);
#if DEBUG
                BubConsole.WriteLine(e.StackTrace, 255, 180, 0);
#endif
                tag = "";
            }
            return tag;
        }


        public void FreeFont(string tag) {
            if (Fonts.ContainsKey(tag)) Fonts.Remove(tag);
            BubConsole.WriteLine($"Font released: {tag}",255,180,105);
        }

        public string Text(string fonttag,string txt) {
            try {
                var tag = "";
                var i = 0;
                do {
                    i++;
                    tag = $"TEXT:{i}";
                } while (Texts.ContainsKey(tag));
                if (!Fonts.ContainsKey(fonttag)) throw new Exception($"No font tagged {fonttag}");
                Texts[tag] = Fonts[fonttag].Text(txt);
                //BubConsole.CSay($"Text Created: {tag};\t\"{txt}\";\t{Texts[tag].Width}x{Texts[tag].Height}; with font {fonttag}!");
                return tag;
            } catch (Exception er) {
                SBubble.MyError("Text error", er.Message, "");
                return "";
            }
        }

        public void FreeText(string tag) {
            //BubConsole.CSay("...");
            if (Texts.ContainsKey(tag)) Texts.Remove(tag);
        }

        public void TextDraw(string tag, int x, int y, byte alignment = 0) {
            if (!Texts.ContainsKey(tag)) throw new Exception($"Text tag {tag} does not exist!");            
            Texts[tag].Draw(x, y, (TQMG_TextAlign)alignment);
        }

        public int TextWidth(string tag) {
            if (!Texts.ContainsKey(tag)) throw new Exception($"Text tag {tag} does not exist!");
            return Texts[tag].Width;
        }

        public int TextHeight(string tag) {
            if (!Texts.ContainsKey(tag)) throw new Exception($"Text tag {tag} does not exist!");
            return Texts[tag].Height;
        }


    }
}










