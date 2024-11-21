using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Entities;
using JABProject.Controller;

namespace Domain.Commands
{
    public class ExpandTreeNodeCommand : Command
    {
        Widget tree;
        CommandsMediator mediator;

        public ExpandTreeNodeCommand(Widget tree, CommandsMediator mediator)
        {
            this.tree = tree;
            this.mediator = mediator;
        }

        public override string commandName()
        {
            return "Expand node from";
        }

        private void addChildrenNodes(Widget parent, List<string> answers, Dictionary<string, JavaObjectHandle> nodesHandles)
        {
            foreach (Widget child in parent.Children)
            {
                nodesHandles.Add(child.Name, child.Handle);
                answers.Add(child.Name);
                addChildrenNodes(child, answers, nodesHandles);
            }
        }


        public override void execute()
        {
            Dictionary<string, JavaObjectHandle> nodesHandles = new Dictionary<string, JavaObjectHandle>();
            List<string> answers = new List<string>();
            addChildrenNodes(tree, answers, nodesHandles);

            string question = "Pick node: ";
            foreach (string option in answers)
            {
                question += option + ",";
            }
            string message = mediator.getResponse(question, answers);

            nodesHandles.TryGetValue(message, out JavaObjectHandle childJavaObjectHandle);
            WindowsInterOp.jab.Functions.GetAccessibleActions(WindowsInterOp.vmId, childJavaObjectHandle, out AccessibleActions accessibleActions);

            AccessibleActionInfo action = Array.Find<AccessibleActionInfo>(accessibleActions.actionInfo, a => a.name == "toggleexpand");
            AccessibleActionsToDo accessibleActionsToDo = new AccessibleActionsToDo();
            accessibleActionsToDo.actionsCount = accessibleActions.actionInfo.Length;
            accessibleActionsToDo.actions = new AccessibleActionInfo[accessibleActions.actionInfo.Length];
            accessibleActionsToDo.actions[0] = action;
            WindowsInterOp.jab.Functions.DoAccessibleActions(WindowsInterOp.vmId, childJavaObjectHandle, ref accessibleActionsToDo, out int failure);
            return;
        }
        public override int getPriority()
        {
            return 1;
        }
    }
}