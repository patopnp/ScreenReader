using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Entities;

namespace Domain.Commands
{
    public class ReadCommand : Command
    {

        Widget root;
        CommandsMediator mediator;

        public ReadCommand(Widget root, CommandsMediator mediator)
        {
            this.mediator = mediator;
            this.root = root;
        }

        private void addChildren(Widget parent, List<string> elements)
        {
            elements.Add("inside " + parent.Name);
            foreach (Widget child in parent.Children)
            {
                elements.Add(child.Name);
                addChildren(child, elements);
            }
            elements.Add("outside " + parent.Name);
        }

        public override int getPriority()
        {
            return 1;
        }
        public override void execute()
        {
            List<string> elements = new List<string>() { root.Name };
            addChildren(root, elements);

            string elementsReading = string.Join(",", elements);

            mediator.read(elementsReading);

            return;
        }

        public override string commandName()
        {
            return "Read elements from";
        }
    }
}