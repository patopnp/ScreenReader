﻿using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using Domain.Commands;

namespace Domain.Entities
{
    public class Table : Widget
    {
        public Table(string name, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm) : base(name, handle, x, y, height)
        {
            Command select = new SelectChildrenCommand(handle, Children, cm);
            Command read = new ReadCommand(this, cm);
            commands.Add(select);
            commands.Add(read);
        }

        public override string getWidgetType()
        {
            return "Table";
        }
    }
}