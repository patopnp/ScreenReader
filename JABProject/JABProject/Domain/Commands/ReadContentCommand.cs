using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Commands
{
    public class ReadContentCommand : Command
    {
        string content;
        CommandsMediator mediator;
        public ReadContentCommand(string c, CommandsMediator mediator)
        {
            this.mediator = mediator;
            this.content = c;
        }

        public override void execute()
        {
            if (content == "")
            {
                mediator.read("No content");
                return;
            }
            mediator.read(content);

        }

        public override string commandName()
        {
            return "Read contents from";
        }

        public override int getPriority()
        {
            return 2;
        }
    }
}