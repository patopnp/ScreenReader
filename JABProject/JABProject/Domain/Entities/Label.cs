﻿using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace Domain.Entities
{
    public class Label : Widget
    {
        public Label(string name, JavaObjectHandle handle, int x, int y, int height) : base(name, handle, x, y, height)
        {
        }

        public override string getWidgetType()
        {
            return "Label";
        }
    }
}