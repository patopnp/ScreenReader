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

namespace JABProject.UI
{
    public class VoiceInputHandler
    {
        
        private bool listen = false;
        private SpeechRecognitionEngine recognizer;

        AutoResetEvent syncEvent;
        private string lastMessage;


        public delegate void speechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e);

        public VoiceInputHandler(AutoResetEvent syncEvent, speechRecognitionRejected method)
        {
            loadSpeechRecognition(method);
            this.syncEvent = syncEvent;
        }

        public void Listen(bool on){
            this.listen = on;
        }

        public string getMessage(){
            return lastMessage;
        }

        private void recognizerSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (listen == false) return;
            lastMessage = e.Result.Text;
            syncEvent.Set();

        }

        public void loadGrammar(List<string> choices)
        {
            recognizer.UnloadAllGrammars();
            Choices myChoices = new Choices();
            foreach (string choice in choices)
            {
                myChoices.Add(choice);
            }

            var gb = new GrammarBuilder(myChoices);
            var g = new Grammar(gb);
            recognizer.LoadGrammarAsync(g);
        }

        private void loadSpeechRecognition(speechRecognitionRejected method)
        {
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            //Add a handler for the speech recognized event
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizerSpeechRecognized);
            recognizer.RecognizerUpdateReached += new EventHandler<RecognizerUpdateReachedEventArgs>(recognizer_RecognizerUpdateReached);
            recognizer.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(method);

            //Configure input to the speech recognizer
            recognizer.SetInputToDefaultAudioDevice();

            loadGrammar(new List<string>() { "exit" });

            //Start asynchronous, continuos speech recogniztion
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        // At the update, get the names and enabled status of the currently loaded grammars.  
        public void recognizer_RecognizerUpdateReached(
          object sender, RecognizerUpdateReachedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Update reached:");
            Thread.Sleep(1000);

            string qualifier;
            List<Grammar> grammars = new List<Grammar>(recognizer.Grammars);
            foreach (Grammar g in grammars)
            {
                qualifier = (g.Enabled) ? "enabled" : "disabled";
                Console.WriteLine("  {0} grammar is loaded and {1}.",
                g.Name, qualifier);
            }
        }
    }
}
