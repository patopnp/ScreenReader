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
    public class Button : Widget
    {
        public Button(string content, JavaObjectHandle handle, int x, int y, int height) : base(content, handle, x, y, height)
        {
            Command click = new ClickCommand(handle);
            commands.Add(click);
        }

        public override string getWidgetType()
        {
            return "Button";
        }
    }

    
}