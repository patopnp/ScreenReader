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
    public class MenuBar : Widget
    {
        public MenuBar(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            Command read = new ReadCommand(this, cm);
            commands.Add(read);
        }

        public override string getWidgetType()
        {
            return "MenuBar";
        }
    }
}