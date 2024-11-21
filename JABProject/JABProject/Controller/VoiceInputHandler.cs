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

namespace JABProject.Controller
{
    public class VoiceInputHandler
    {
        static SpeechSynthesizer synth;
        private static bool listen = false;
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private static string message;
        private Dictionary<string, int> textToNumber;
        private static SpeechRecognitionEngine recognizer;

        public VoiceInputHandler()
        {

            synth = new SpeechSynthesizer();
            // Configure the audio output
            synth.SetOutputToDefaultAudioDevice();
        }

        private void recognizerSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (listen == false) return;
            Console.WriteLine("Recognized text: " + e.Result.Text);
            message = e.Result.Text;
            autoResetEvent.Set();

        }

        private void loadGrammarOptions(int number)
        {
            recognizer.UnloadAllGrammars();
            Choices myChoices = new Choices();
            for (int i = 1; i <= number; i++)
            {
                myChoices.Add(i + "");
            }

            var gb = new GrammarBuilder(myChoices);
            var g = new Grammar(gb);
            recognizer.LoadGrammar(g);
        }
        private static void loadGrammar(List<string> choices)
        {
            Choices myChoices = new Choices();
            foreach (string choice in choices)
            {
                myChoices.Add(choice);
            }

            var gb = new GrammarBuilder(myChoices);
            var g = new Grammar(gb);
            recognizer.LoadGrammarAsync(g);
        }

        
        private void loadSpeechRecognition(int num)
        {
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));


            loadGrammarOptions(num);

            //Add a handler for the speech recognized event
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizerSpeechRecognized);
            recognizer.RecognizerUpdateReached += new EventHandler<RecognizerUpdateReachedEventArgs>(recognizer_RecognizerUpdateReached);
            recognizer.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(recognizer_SpeechRecognitionRejected);

            //Configure input to the speech recognizer
            recognizer.SetInputToDefaultAudioDevice();


            //Start asynchronous, continuos speech recogniztion
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        // Write a message to the console when recognition fails.  
        void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            readText("Please repeat");
            Console.WriteLine("Recognition attempt failed");
        }

        // At the update, get the names and enabled status of the currently loaded grammars.  
        public static void recognizer_RecognizerUpdateReached(
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

        public Command askAboutActions(List<Widget> widgets)
        {

            Dictionary<string, Command> voiceInputToCommand = new Dictionary<string, Command>();
            List<string> options = new List<string>();

            readText("Pick a possible action from the followings:");

            foreach (Widget widget in widgets)
            {

                Dictionary<string, Command> command = widget.getCommands();
                if (command != null)
                {
                    //Console.WriteLine(command);
                    foreach (var kvp in command)
                    {
                        Console.WriteLine(kvp.Key);
                        options.Add(kvp.Key);
                        if (voiceInputToCommand.ContainsKey(kvp.Key) == false)
                        {
                            voiceInputToCommand.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
            }

            string message = ask(string.Join(". ", options), options);
            
            voiceInputToCommand.TryGetValue(message, out Command executableCommand);
            return executableCommand;

        }

        public string ask(string question, List<string> answers)
        {

            Console.WriteLine(question);
            recognizer.UnloadAllGrammars();

            loadGrammar(answers);
            recognizer.RequestRecognizerUpdate();
            readText(question);
            listen = true;
            autoResetEvent.WaitOne();

            listen = false;
            return message;
        }

        public void readText(String text)
        {
            synth.Speak(text);
        }

        private void recognizeVoiceInput()
        {
            Console.WriteLine(recognizer.Recognize().Text);
        }

        public int chooseApp(List<JavaApplication> javaApplications)
        {
            
            if (javaApplications.Count == 0)
            {
                readText("No running Java application found.");
                return -1;
            }
            else if (javaApplications.Count > 1)
            {
                loadSpeechRecognition(javaApplications.Count);
                readText(javaApplications.Count + " Java applications running.");

                string instructions = "";
                List<string> possibleAnswers = new List<string>();

                for (int i = 1; i <= javaApplications.Count; i++)
                {
                    instructions += "Say " + i + " for " + javaApplications[i-1].WindowsTitle + ". ";
                    possibleAnswers.Add(i.ToString());


                }

                string answer = ask(instructions, possibleAnswers);
                return int.Parse(answer);
            }
            else
            {
                loadSpeechRecognition(javaApplications.Count);
                
                readText(javaApplications[0].WindowsTitle + " is running.");
                return 1;
            }
        }
    }
}
