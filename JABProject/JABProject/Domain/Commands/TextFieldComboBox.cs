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
    public class TextFieldComboBox : ComboBox
    {
        string content;

        public TextFieldComboBox(string content, string name, JavaObjectHandle textHandle, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(content, handle, x, y, height, cm)
        {
            Command write = new WriteTextCommand(textHandle, cm);
            Command select = new SelectChildrenCommand(handle, Children, cm);
            commands.Add(write);
            commands.Add(select);
        }

    }
}
