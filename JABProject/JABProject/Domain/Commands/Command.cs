using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Commands
{
    public abstract class Command
    {
        public abstract int getPriority();
        public abstract void execute();
        public abstract string commandName();

    }
}