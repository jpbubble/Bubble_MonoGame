// Lic:
// Bubble for MonoGame
// Debug Console
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
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TrickyUnits;

namespace Bubble {

    internal class BCLine {
        public byte r = 255, g = 255, b = 255;        
        public string txt;

        public void Show(int x, int y) {
            TQMG.Color(r, g, b);
            SysFont.DrawText(txt, x, y);
        }
    }

    class BubConsole:HardFlowClass { // The "bub" prefix is to prevent conflicts with the System Console
        static public int MaxLines = 5000;
        static List<BCLine> Line = new List<BCLine>();
        static public byte CSayR = 0, CSayG = 180, CSayB = 255;
        static TQMGImage BackGround;
        readonly static public BubConsole Flow = new BubConsole();
        static int ScrollUp=0;
        static string TypingCommand = "";
        int LinesOnScreen => TQMG.ScrHeight / 22;
        int StartY {
            get {
                if (Line.Count < LinesOnScreen) return 0;
                var Overlines = (Line.Count+1) - LinesOnScreen; // +1 because the line where the commands are being typed in, does also count!
                return Overlines * 22;
            }
        }

        static public void WriteLine(string msg, byte r = 255, byte g = 255, byte b = 255) {
            foreach (string txt in msg.Split('\n')){
                var line = new BCLine();
                line.txt = txt;
                line.r = r;
                line.g = g;
                line.b = b;
                Debug.WriteLine($"Bubble Console:> {msg}");
                Line.Add(line);
                if (Line.Count > MaxLines) Line.RemoveAt(0);
            }
        }

        static public void CSay(string msg) => WriteLine(msg, CSayR, CSayG, CSayB);

        static BubConsole() {
            WriteLine($"Bubble {MKL.Newest} - (c) Jeroen P. Broks", 255, 255, 0);
            var s = new NLua.Lua();
            var v = (string)s.DoString("return _VERSION")[0];
            WriteLine($"Uses {v} by PUC-Rio",180,0,255);
            if (SBubble.JCR.Exists("Bubble/Background.png")) {
                WriteLine("Loading: Bubble/Background.png", 255, 180, 0);
                BackGround = TQMG.GetImage("Bubble/Console.png");
            } else if (SBubble.JCR.Exists("Bubble/Console.jpg")) {
                WriteLine("Loading: Bubble/Console.jpg", 255, 180, 0);
                BackGround = TQMG.GetImage("Bubble/Console.jpg");
            } else {
                WriteLine("No background found", 255, 0, 0);
            }
        }


        public override void Draw(GameTime gameTime) {
            TQMG.Color(255, 255, 255);
            if (BackGround != null) TQMG.Tile(BackGround, 0, 0, 0, 0, TQMG.ScrWidth, TQMG.ScrHeight);
            try {             
                var y = ScrollUp - StartY;
                foreach (BCLine l in Line) {
                    if (y > -30)
                        //SysFont.DrawText(l.txt, 2, y);
                        l.Show(2, y);
                    y += 22;
                }
                TQMG.Color(255, 180, 0);
                SysFont.DrawText($">{TypingCommand}_",2,y);
            } catch (Exception error) {
                Debug.WriteLine($"Exception during the debug log rendering!\n{error.Message}\nTraceback:\n{error.StackTrace}\n\n");
            }
        }

        public override void Update(GameTime gameTime) {
            // Please note, these routines do not scan anything. They only return the value the last TQMG.Start() request done in the main routine scanned for.
            var b = TQMGKey.GetChar();
            var k = TQMGKey.GetKey();
            switch (k) {
                case Keys.Back:
                    if (TypingCommand != "") TypingCommand = qstr.Left(TypingCommand, TypingCommand.Length - 1);
                    break;
                default: {
                        int tw = 0, th = 0;
                        SysFont.TextSizes($"{TypingCommand}__", ref tw, ref th);
                        if (b >= 32 && b <= 126 && tw < TQMG.ScrWidth - 25)
                            TypingCommand += b;
                        break;
                    }
            }
        }

    }
}


