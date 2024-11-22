using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using JABProject.Utils.InterOp;

namespace Domain.Commands
{
    public class EraseTextCommand : Command
    {
        JavaObjectHandle handle;

        public EraseTextCommand(JavaObjectHandle handle)
        {
            this.handle = handle;
        }
        public override void execute()
        {
            WindowsInterOp.jab.Functions.SetTextContents(WindowsInterOp.vmId, handle, "");
        }

        public override string commandName()
        {
            return "Erase text from";
        }

        public override int getPriority()
        {
            return 2;
        }
    }
}