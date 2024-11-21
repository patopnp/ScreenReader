using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using WindowsAccessBridgeInterop;
using JABProject;

namespace Domain.Commands
{
    public class WriteTextCommand : Command
    {
        JavaObjectHandle handle;
        CommandsMediator mediator;

        public WriteTextCommand(JavaObjectHandle handle, CommandsMediator mediator)
        {
            this.mediator = mediator;
            this.handle = handle;
        }
        public override string commandName()
        {
            return "Write text into";
        }
        public override void execute()
        {
            string question = "What do you want to write?";
            List<string> answers = new List<String>() { "sample text", "C:/", "Art" };

            string message = mediator.getResponse(question, answers);

            WindowsInterOp.jab.Functions.SetTextContents(WindowsInterOp.vmId, handle, message);

        }

        public override int getPriority()
        {
            return 2;
        }
    }
}