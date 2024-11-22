using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Entities;
using JABProject.Utils.InterOp;

namespace Domain.Commands
{
    public class SelectChildrenCommand : Command
    {
        private JavaObjectHandle handle;
        private List<Widget> children;
        private CommandsMediator mediator;

        public SelectChildrenCommand(JavaObjectHandle handle, List<Widget> children, CommandsMediator mediator)
        {
            this.handle = handle;
            this.children = children;
            this.mediator = mediator;
        }

        public override void execute()
        {
            if (children.Count == 0)
            {
                mediator.read("No elements");
                return;
            }
            List<string> answers = children.Select(c => c.Name).ToList<string>();

            string question = "Select an option from the followings: ";
            question += string.Join(", ", answers);

            string message = mediator.getResponse(question, answers);

            int selectedIndex = children.FindIndex(c => c.Name == message);

            if (selectedIndex != -1)
                WindowsInterOp.jab.Functions.AddAccessibleSelectionFromContext(WindowsInterOp.vmId, handle, selectedIndex);
        }

        public override string commandName()
        {
            return "Select element from";
        }

        public override int getPriority()
        {
            return 2;
        }
    }
}
