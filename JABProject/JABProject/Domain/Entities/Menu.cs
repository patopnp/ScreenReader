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
    public class Menu : Widget
    {
        public Menu(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            
            Command click = new ClickCommand(handle);
            Command read = new ReadCommand(this, cm);
            commands.Add(read);
            commands.Add(click);
        }

        public override string getWidgetType()
        {
            return "Menu";
        }
    }
}