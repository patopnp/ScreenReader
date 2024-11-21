using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Commands
{
    public class ClickCommand : Command
    {
        private JavaObjectHandle handle;

        public ClickCommand(JavaObjectHandle handle)
        {
            this.handle = handle;
        }

        public override string commandName()
        {
            return "Click";
        }

        public override void execute()
        {
            WindowsInterOp.jab.Functions.GetAccessibleActions(WindowsInterOp.vmId, handle, out AccessibleActions actions);


            AccessibleActionInfo action = Array.Find<AccessibleActionInfo>(actions.actionInfo, a => a.name == "click");

            AccessibleActionsToDo accessibleActionsToDo = new AccessibleActionsToDo();
            accessibleActionsToDo.actionsCount = actions.actionInfo.Length;
            accessibleActionsToDo.actions = new AccessibleActionInfo[actions.actionInfo.Length];
            accessibleActionsToDo.actions[0] = action;
            WindowsInterOp.jab.Functions.DoAccessibleActions(WindowsInterOp.vmId, handle, ref accessibleActionsToDo, out int failure);
        }

        public override int getPriority()
        {
            return 1;
        }
    }
}