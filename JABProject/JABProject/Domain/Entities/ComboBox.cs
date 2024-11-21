using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using JABProject;
using Domain.Commands;

namespace Domain.Entities
{
    public class ComboBox : Widget
    {

        public ComboBox(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            Command selectOption = new SelectChildrenCommand(handle, Children, cm);
            commands.Add(selectOption);
        }

        public override string getWidgetType()
        {
            return "ComboBox";
        }
    }
}
