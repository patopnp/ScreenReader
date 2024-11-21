using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Commands
{
    public class SpinValueCommand : Command
    {
        JavaObjectHandle handle;
        string direction;
        CommandsMediator mediator;
        public SpinValueCommand(JavaObjectHandle handle, string direction, CommandsMediator mediator)
        {
            this.mediator = mediator;
            this.direction = direction;
            this.handle = handle;
        }

        public override string commandName()
        {
            return direction;
        }

        public override void execute()
        {
            List<string> answers = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            string question = "How many times?";

            string message = mediator.getResponse(question, answers);
            int.TryParse(message, out int times);

            WindowsInterOp.jab.Functions.GetAccessibleActions(WindowsInterOp.vmId, handle, out AccessibleActions actions);


            AccessibleActionInfo action = Array.Find<AccessibleActionInfo>(actions.actionInfo, a => a.name == direction);

            AccessibleActionsToDo accessibleActionsToDo = new AccessibleActionsToDo();
            accessibleActionsToDo.actionsCount = actions.actionInfo.Length;
            accessibleActionsToDo.actions = new AccessibleActionInfo[actions.actionInfo.Length];
            accessibleActionsToDo.actions[0] = action;

            for (int i = 0; i < times; i++)
                WindowsInterOp.jab.Functions.DoAccessibleActions(WindowsInterOp.vmId, handle, ref accessibleActionsToDo, out int failure);

        }

        public override int getPriority()
        {
            return 2;
        }
    }
}
