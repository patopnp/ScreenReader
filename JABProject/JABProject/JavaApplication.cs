using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace JABProject
{
    public class JavaApplication
    {
        string windowsTitle;
        IntPtr processPtr;

        public JavaApplication(string wt, IntPtr ptr)
        {
            this.windowsTitle = wt;
            this.processPtr = ptr;
        }

        public string WindowsTitle
        {
            get { return windowsTitle; }
            set { windowsTitle = value; }
        }

        public IntPtr ProcessPtr
        {
            get { return processPtr; }
            set { processPtr = value; }
        }
    }
}
