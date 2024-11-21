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
    public class TextField : Widget
    {
        string content;


        public TextField(string content, string name, JavaObjectHandle handle, bool isEditable, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            this.content = content;
            Command read = new ReadContentCommand(content, cm);
            commands.Add(read);

            if (isEditable)
            {
                Command write = new WriteTextCommand(handle, cm);
                Command erase = new EraseTextCommand(handle);
                commands.Add(erase);
                commands.Add(write);
            }
        }

        public string getContent()
        {
            return content;
        }

        public override string getWidgetType()
        {
            return "TextField";
        }
    }
}