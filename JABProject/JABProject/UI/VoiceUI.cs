using Domain.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JABProject.Utils.InterOp;

namespace JABProject.UI
{
    public class VoiceUI
    {

        Speaker speaker;
        VoiceInputHandler voiceController;
        AutoResetEvent syncEvent;

        public VoiceUI()
        {
            speaker = new Speaker();
            syncEvent = new AutoResetEvent(false);
            voiceController = new VoiceInputHandler(syncEvent, SpeechRecognitionRejected);
        }

        void SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            read(Speaker.unrecognizedCommand);
        }


        public string ask(string question, List<string> answers)
        {
            voiceController.loadGrammar(answers);
            read(question);
            voiceController.Listen(true);
            syncEvent.WaitOne();
            voiceController.Listen(false);
            return voiceController.getMessage();
        }

        public void read(string text)
        {
            speaker.read(text);
        }

        public int promptAppChoice(List<JavaApplication> javaApplications)
        {

            if (javaApplications.Count == 0) {
                read(Speaker.noApp);
                return -1;
            }
            else if (javaApplications.Count > 1)
            {
                read(javaApplications.Count + " Java applications running.");

                string instructions = "";
                List<string> possibleAnswers = new List<string>();

                for (int i = 1; i <= javaApplications.Count; i++){
                    instructions += "Say " + i + " for " + javaApplications[i - 1].WindowsTitle + ". ";
                    possibleAnswers.Add(i.ToString());
                }

                string answer = ask(instructions, possibleAnswers);
                return int.Parse(answer);
            }
            else
            {
                read(javaApplications[0].WindowsTitle + " is running.");
                return 1;
            }
        }
    }
}
