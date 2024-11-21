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
    public class SpinBox : Widget
    {

        // tambien se puede escribir el valor en la casilla usando AccessibleTextInterface el unico problema seria el dictado de numeros

        public SpinBox(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            SpinValueCommand increment = new SpinValueCommand(handle, "increment", cm);
            SpinValueCommand decrement = new SpinValueCommand(handle, "decrement", cm);
            commands.Add(increment);
            commands.Add(decrement);
        }

        public override string getWidgetType()
        {
            return "SpinBox";
        }
    }
}
