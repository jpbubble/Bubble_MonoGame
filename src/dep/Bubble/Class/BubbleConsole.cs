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

ï»¿
namespace Bubble {
    class BubbleConsole {

        static BubbleConsole api;

        static public void StateInit(string vm) {
            api = new BubbleConsole();
            var s = SBubble.State(vm).state;
            s["Bubble_Console_Console"] = api;
            //*
            SBubble.DoNIL(vm, @"

            #accept Bubble_Console_Console

            global void CWriteLine(string msg,int r, int g, int b)
                assert(r>=0 and r<=255 and g>=0 and g<=255 and b>=0 and b<=255,'CWriteLine color setting invalid!')
                Bubble_Console_Console:WriteLine(msg,r,g,b)
            end

            global void CPrint(string msg,r,g,b)
                CWriteLine(msg,tonumber(r) or 255, tonumber(g) or 255, tonumber(b) or 255)
            end

            global void CSayColor(int r, int g, int b)
                assert(r>=0 and r<=255 and g>=0 and g<=255 and b>=0 and b<=255,'CWriteLine color setting invalid!')
                Bubble_Console_Console:SetCSayColor(r,g,b)
            end

            global void CSay(string msg)
                Bubble_Console_Console:CSay(msg)
            end


            ", "Console initizer");
            // */
            /*
            SBubble.State(vm).DoString(@" -- Translation by NIL... Absolutely ludicrous I must do it this way, but since no errors are properly thrown, I don't have a choice!

-- comment:  whiteline
function CWriteLine(msg, r, g, b) assert(type(msg)=='string' and type(r)=='number' and type(g)=='number' and type(b)=='number','NR: Function did not receive the parameters the way it wanted!')
assert ( r >= 0  and  r <= 255  and  g >= 0  and  g <= 255  and  b >= 0  and  b <= 255 , 'CWriteLine color setting invalid!' )
Bubble_Console_Console:WriteLine ( msg , r , g , b )
end
-- comment:  whiteline
function CPrint(msg, r, g, b) assert(type(msg)=='string','NR: Function did not receive the parameters the way it wanted!')
CWriteLine ( msg , tonumber ( r )  or  255 , tonumber ( g )  or  255 , tonumber ( b )  or  255 )
end
-- comment:  whiteline
function CSayColor(r, g, b) assert(type(r)=='number' and type(g)=='number' and type(b)=='number','NR: Function did not receive the parameters the way it wanted!')
assert ( r >= 0  and  r <= 255  and  g >= 0  and  g <= 255  and  b >= 0  and  b <= 255 , 'CWriteLine color setting invalid!' )
Bubble_Console_Console:SetCSayColor ( r , g , b )
end
-- comment:  whiteline
function CSay(msg) assert(type(msg)=='string','NR: Function did not receive the parameters the way it wanted!')
Bubble_Console_Console:CSay ( msg )
end");
//*/

        }


        public void WriteLine(string msg, byte r, byte g, byte b) => BubConsole.WriteLine(msg, r, g, b);
        public void CSay(string msg) => BubConsole.CSay(msg);
        public void SetCSayColor(byte r, byte g, byte b) {
            BubConsole.CSayR = r;
            BubConsole.CSayG = g;
            BubConsole.CSayB = b;
        }
    }


}
