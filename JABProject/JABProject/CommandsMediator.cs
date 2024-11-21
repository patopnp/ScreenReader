using JABProject.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABProject
{
    public class CommandsMediator
    {
        VoiceInputHandler vh;

        public CommandsMediator(VoiceInputHandler vh)
        {
            this.vh = vh;
        }

        public string getResponse(string question, List<string> otions)
        {
            return vh.ask(question, otions);
        }

        public void read(string txt)
        {
            vh.readText(txt);
        }
    }
}
