using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Entities
{
    public class PageTabList : List
    {
        public PageTabList(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height, cm)
        {
        }

        public override string getWidgetType()
        {
            return "PageTabList";
        }
    }
}