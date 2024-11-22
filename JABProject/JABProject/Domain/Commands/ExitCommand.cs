using Domain.Commands;
using JABProject.Utils.InterOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;

namespace JABProject.Domain.Commands
{
    public class ExitCommand : Command
    {
        JavaObjectHandle handle;

        public override void execute()
        {
            Environment.Exit(0);
        }

        public override string commandName()
        {
            return "Exit";
        }

        public override int getPriority()
        {
            return 2;
        }
    }
    }
