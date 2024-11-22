using Domain.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JABProject.UI;

namespace JABProject
{
    public class CommandsMediator
    {
        VoiceUI vui;

        public CommandsMediator(VoiceUI vui)
        {
            this.vui = vui;
        }

        public string getResponse(string question, List<string> otions)
        {
            return vui.ask(question, otions);
        }

        public void read(string txt)
        {
            vui.read(txt);
        }

        public Command askAboutActions(List<Widget> widgets)
        {

            Dictionary<string, Command> voiceInputToCommand = new Dictionary<string, Command>();
            List<string> options = new List<string>();

            
            foreach (Widget widget in widgets)
            {

                Dictionary<string, Command> command = widget.getCommands();
                if (command != null)
                {
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
            if (options.Count == 0)
            {
                read(Speaker.noOptionsText);
                return null;
            }
            else
            {
                read(Speaker.promptPossibleActions);
            }

            string message = vui.ask(string.Join(". ", options), options);

            voiceInputToCommand.TryGetValue(message, out Command executableCommand);
            return executableCommand;

        }

    }
}
