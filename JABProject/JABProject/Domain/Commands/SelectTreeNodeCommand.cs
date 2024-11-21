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
    public class SelectTreeNodeCommand : Command
    {
        Widget tree;
        CommandsMediator mediator;

        public SelectTreeNodeCommand(Widget tree, CommandsMediator mediator)
        {
            this.tree = tree;
            this.mediator = mediator;
        }

        public override string commandName()
        {
            return "Select node from";
        }

        private void addChildrenNodes(Widget parent, List<string> answers, Dictionary<string, (Widget,int)> nodesHandles)
        {
            for (int i = 0; i < parent.Children.Count; i++)
            {
                nodesHandles.Add(parent.Children[i].Name, (parent, i));
                answers.Add(parent.Children[i].Name);
                addChildrenNodes(parent.Children[i], answers, nodesHandles);
            }
        }


        public override void execute()
        {
            Dictionary<string, (Widget, int)> nodesHandles = new Dictionary<string, (Widget, int)>();
            List<string> answers = new List<string>();
            addChildrenNodes(tree, answers, nodesHandles);

            string question = "Pick node: ";
            foreach (string option in answers)
            {
                question += option + ",";
            }
            string message = mediator.getResponse(question, answers);

            nodesHandles.TryGetValue(message, out (Widget, int) tuple);

            WindowsInterOp.jab.Functions.AddAccessibleSelectionFromContext(WindowsInterOp.vmId, tuple.Item1.Handle, tuple.Item2);
            return;
        }
        public override int getPriority()
        {
            return 1;
        }
    }
}