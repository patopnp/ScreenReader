using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Commands;

namespace Domain.Entities
{
    public class Tree : Widget
    {

        public Tree(string content, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(content, handle, x, y, height)
        {
            Command expandTreeNode = new ExpandTreeNodeCommand(this, cm);
            Command readCommand = new ReadCommand(this, cm);
            Command selectNode = new SelectTreeNodeCommand(this, cm);
            commands.Add(expandTreeNode);
            commands.Add(readCommand);
            commands.Add(selectNode);
        }

        public override string getWidgetType()
        {
            return "Tree";
        }
    }
}