using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JABProject;
using WindowsAccessBridgeInterop;
using static JABProject.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static WindowsAccessBridgeInterop.TextReaderExtensions;

namespace Domain.Entities
{
    public class Text : Widget
    {
        string content;
        public Text(string content, string name, JavaObjectHandle handle, int x, int y, int height) : base(name, handle, x, y, height)
        {
            this.content = content;
        }

        public override string getWidgetType()
        {
            return "Text";
        }
    }
}
