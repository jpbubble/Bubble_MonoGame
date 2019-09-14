// Lic:
// BUBBLE config
// Config
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
// Version: 19.08.05
// EndLic


using System;
using System.Diagnostics;
using System.Windows.Forms;
using TrickyUnits;

namespace Bubble {

    class Bubble_Conf  {

        string statename="";

        private Bubble_Conf() { }

        void Crash(string msg, string addtrace = "") {
            var trace = $"\tState: {statename}\n\n{SBubble.TraceLua(statename)}\n\n{addtrace}";
            SBubble.MyError("Bubble_Conf API error!", msg, trace);
        }

        void Crash(Exception e) => Crash($".NET: {e.Message}", e.StackTrace);

        public bool Yes(string Question) => Confirm.Yes(Question);
        public string YNC(string Question) {
            switch (Confirm.YNC(Question)) {
                case -1: return "Cancel";
                case 0: return "No";
                case 1: return "Yes";
                default: SBubble.MyError("HACKED!", "HACKED!", "HACKED!"); return "HACKED!"; // Shouldn't be possible!
            }
        }
        public string FailureBox(string Caption) => $"{Confirm.Failure(Caption)}";
        public void Annoy(string msg,string Caption="",string Icon = "Information") {
            var useicon = MessageBoxIcon.Information;
            foreach (MessageBoxIcon iIcon in (MessageBoxIcon[])Enum.GetValues(typeof(MessageBoxIcon))) {
                if (Icon == $"{iIcon}") useicon = iIcon;
            }
            Confirm.Annoy(msg, Caption, useicon);
        }

        public static void Init(string s) {
            var state = SBubble.State(s).state;
            var script = QuickStream.StringFromEmbed("Bubble_Conf.nil");
            state["Bubble_Conf"] = new Bubble_Conf();
            Debug.WriteLine($"Initiating state: {s}");
            BubConsole.CSay($"State \"{s}\" being prepared for Bubble_Conf");
            SBubble.DoNIL(s, script, "Message box init script");
        }
    }
}

