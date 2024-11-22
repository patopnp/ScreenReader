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
    public class MenuItem : Widget
    {
        public MenuItem(string name, JavaObjectHandle handle, int x, int y, int height) : base(name, handle, x, y, height)
        {
            Command click = new ClickCommand(handle);
            commands.Add(click);
        }

        public override string getWidgetType()
        {
            return "MenuItem";
        }
    }
}