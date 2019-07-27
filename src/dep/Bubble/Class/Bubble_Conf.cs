using System;
using System.Diagnostics;
using System.Windows.Forms;
using TrickyUnits;

namespace Bubble {

    class Bubble_Conf  {

        string statename;

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