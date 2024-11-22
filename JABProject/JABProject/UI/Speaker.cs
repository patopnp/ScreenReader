using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace JABProject.UI
{
    internal class Speaker
    {
        SpeechSynthesizer synth;

        public const string noOptionsText = "No available actions to execute";
        public const string promptPossibleActions = "Pick an actions from the followings:";
        public const string unrecognizedCommand = "Please repeat";
        public const string noApp = "No running Java application found";

        public Speaker()
        {
            synth = new SpeechSynthesizer();
            // Configure the audio output
            synth.SetOutputToDefaultAudioDevice();
        }

        public void read(String text)
        {

            //string text = "This is a great text.";

            synth.Speak(text);
        }

    }
}
