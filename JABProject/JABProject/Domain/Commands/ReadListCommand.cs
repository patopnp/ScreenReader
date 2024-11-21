using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Commands
{
    public class ReadListCommand : Command
    {
        List list;
        CommandsMediator mediator;

        public ReadListCommand(List list, CommandsMediator mediator)
        {
            this.mediator = mediator;
            this.list = list;
        }


        public override int getPriority()
        {
            return 1;
        }
        public override void execute()
        {
            if (list.Children.Count == 0)
            {
                mediator.read("Empty");
                return;
            }
            List<string> elements = list.Children.Select(a => a.getTag()).ToList();

            string text = string.Join(",", elements);

            mediator.read(text);

            return;
        }

        public override string commandName()
        {
            return "Read elements from";
        }
    }
}