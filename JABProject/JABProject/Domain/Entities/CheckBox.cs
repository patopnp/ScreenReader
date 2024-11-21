using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WindowsAccessBridgeInterop;
using Domain.Commands;

namespace Domain.Entities
{
    public class CheckBox : Widget
    {
        bool isChecked;

        public CheckBox(string content, JavaObjectHandle handle, bool isChecked, int x, int y, int height) : base(content, handle, x, y, height)
        {
            this.isChecked = isChecked;
            Command click = new CheckCommand(handle, this);
            commands.Add(click);
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public override string getWidgetType()
        {
            return "CheckBox";
        }
    }
}
