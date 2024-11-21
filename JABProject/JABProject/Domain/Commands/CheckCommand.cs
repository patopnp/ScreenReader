using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Entities;

namespace Domain.Commands { 
    public class CheckCommand : ClickCommand
    {
        CheckBox checkBox;

        public CheckCommand(JavaObjectHandle handle, CheckBox checkbox) : base(handle)
        {
            this.checkBox = checkbox;
        }

        public override string commandName()
        {
            if (checkBox.IsChecked == true)
                return "Uncheck";
            else
                return "Check";
        }

    }
}