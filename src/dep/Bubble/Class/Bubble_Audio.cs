// Lic:
// Bubble in C#
// Audio library
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
// Version: 19.05.14
// EndLic


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TrickyUnits;

using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Media; // As XNA only supports the use of external players requiring "real files" this road is for now not discussable.
//Even more because it's very well possible, future editions of Bubble may even not be using XNA.

namespace Bubble {


    class Bubble_Audio {
        static Dictionary<string, SoundEffect> AudioMap = new Dictionary<string, SoundEffect>();
        static Dictionary<string, SoundEffectInstance> InstanceMap = new Dictionary<string, SoundEffectInstance>();

        public static void Init(string vm) { new Bubble_Audio(vm); }
        public Bubble_Audio(string s) {
            var state = SBubble.State(s).state;
            var script = QuickStream.StringFromEmbed("Audio.nil");
            state["Bubble_Audio"] = this;
            SBubble.DoNIL(s, script, "Audio init script");
        }

        public string Load(string file,string tag = "") {
            Debug.Print($"Loading Audio: {file}\t{tag}");
            var bt = SBubble.JCR.AsMemoryStream(file);
            if (bt==null) throw new Exception(UseJCR6.JCR6.JERROR);
            Debug.WriteLine($"Retrieving from memstream");
            SoundEffect sfx = SoundEffect.FromStream(bt);
            Debug.Print($"Loaded {file}");
            if (sfx == null) throw new Exception("Audio load failed!");
            bt.Close();
            Debug.WriteLine("Checking tag");
            if (tag == "") { 
                var h = file.Length;
                for(int i = 0; i < file.Length; i++) h += (byte)file[i];
                do h++; while (AudioMap.ContainsKey($"AUDIO{h}"));
                tag = $"AUDIO{h}";
            }
            Debug.Print($"Assigned on {tag}");
            AudioMap[tag] = sfx;
            return tag;
        }

        public void Play(string buf) {
            try {
                //Debug.Write($"Playing {buf}");
                if (!AudioMap.ContainsKey(buf)) throw new Exception($"No audio buffer tagged {buf} available");
                //Debug.Write(" ... ");
                AudioMap[buf].Play();
            } catch (Exception huh) {
                SBubble.MyError("Odd things happen here!", huh.Message, huh.StackTrace);
            }
            //Debug.WriteLine("Done");
        }

        public string IPlay(string buf, bool loop) {
            var tag = "";
            try {
                if (!AudioMap.ContainsKey(buf)) throw new Exception($"No audio buffer tagged {buf} available");
                var i = AudioMap[buf].CreateInstance();
                var c = 0;
                do {
                    c++;
                    tag = $"INSTANCE{c.ToString("X10")}";
                } while (InstanceMap.ContainsKey(tag));
                InstanceMap[tag] = i;
                i.IsLooped = loop;
                i.Play();
            } catch (Exception huh) {
                SBubble.MyError("Odd things happen here!", huh.Message, huh.StackTrace);
            } 
            return tag;            
        }

        public void FreeInstance(string buf) {
            try {
                if (!InstanceMap.ContainsKey(buf)) return;
                var i = InstanceMap[buf];
                i.Stop(); // I don't want any sound to linger until the C# garbage collector picks this up.
                InstanceMap.Remove(buf);
            } catch (Exception Error) {
                SBubble.MyError($"Error on audio instance {buf}",Error.Message,"");
            }
        }

        public void Free(string buf) => AudioMap.Remove(buf);
        
    }
}


