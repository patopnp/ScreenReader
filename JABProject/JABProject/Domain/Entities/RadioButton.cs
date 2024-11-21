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
    public class RadioButton : Widget
    {
        bool isChecked;

        public RadioButton(string content, JavaObjectHandle handle, bool isChecked, int x, int y, int height) : base(content, handle, x, y, height)
        {
            Command click = new ClickCommand(handle);
            commands.Add(click);
            this.isChecked = isChecked;
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public override string getWidgetType()
        {
            return "RadioButton";
        }
    }
}