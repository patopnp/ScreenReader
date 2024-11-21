using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using JABProject;
using Domain.Commands;

namespace Domain.Entities
{
    public class Widget
    {

        private string name;
        string content;
        int x, y, height;
        protected JavaObjectHandle handle;
        private List<Widget> children;
        protected List<Command> commands;

        public Widget(string name, JavaObjectHandle handle, int x, int y, int height)
        {
            commands = new List<Command>();
            this.children = new List<Widget>();
            this.name = name;
            this.x = x;
            this.y = y;
            this.height = height;
            this.handle = handle;
        }

        public JavaObjectHandle Handle
        {
            get { return handle; }
            set { handle = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public List<Widget> Children
        {
            get { return children; }
        }

        static string toReadableString(string name)
        {
            if (name == "" || name == null) return "";

            int lastLetterIndex = name.Length;
            // Check if the last character is a letter or digit
            while (!char.IsLetterOrDigit(name[lastLetterIndex - 1]) && lastLetterIndex >= 0)
            {
                lastLetterIndex--;
                // If it's a special character, remove it

            }

            if (lastLetterIndex < 0) return "";
            // If the last character is alphanumeric, return the word as is
            return name.Substring(0, lastLetterIndex);
        }


        public string getTag()
        {
            string textForSpeech = "";
            string role = "";
            string name = toReadableString(Name);

            if (getWidgetType() != "Label" && getWidgetType() != "Widget")
            {
                role = getWidgetType();
            }
            if (role + name != "")
            {
                if (name != "" && role != "")
                {
                    textForSpeech += name + " " + role + "";
                }
                else
                {
                    textForSpeech += (name == "" ? role : name) + "";
                }

            }

            return textForSpeech;
        }

        public virtual string getWidgetType()
        {
            return "Widget";
        }
        public Dictionary<string, Command> getCommands()
        {
            Dictionary<string, Command> voiceCmdToAction = new Dictionary<string, Command>();
            foreach (Command action in commands)
            {
                string key = action.commandName() + " " + getTag();
                voiceCmdToAction.Add(key, action);
            }

            return voiceCmdToAction;
        }
    }
}