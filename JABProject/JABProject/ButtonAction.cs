using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABProject
{
    internal class ButtonAction : Action
    {

        public ButtonAction(WidgetReading reading, JavaObjectHandle handle) : base(reading, handle)
        {
        }


        public void executeAction(AccessBridge Java, int vmId)
        {
            Java.Functions.AddAccessibleSelectionFromContext(vmId, handle, 0);
        }
    }
}
