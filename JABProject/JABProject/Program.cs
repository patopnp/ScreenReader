using Domain.Commands;
using Domain.Entities;
using JABProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using JABProject.UI;
using JABProject.Utils.InterOp;

namespace JABProject
{
    internal class Program
    {

        VoiceUI voiceUI;
        private Dictionary<string, Command> voiceInputToCommand;

        List<Action> actions = new List<Action>();
        List<Widget> sortedWidgets = new List<Widget>();
        List<String> validReadableWidgets = new List<String>();
        List<Widget> readings = new List<Widget>();

        public static AccessBridge accessBridge;
        Int32 vmId;

        static void Main(string[] args)
        {
            new Program().readApplication();
        }
        
        private void readApplication()
        {
            voiceUI = new VoiceUI();
            IntPtr windowHandle = IntPtr.Zero;

            List<JavaApplication> javaApplications = WindowsInterOp.FindJavaWindow("javaw", "jp2launcher", "java");

            int appIndex = voiceUI.promptAppChoice(javaApplications);

            windowHandle = javaApplications[appIndex - 1].ProcessPtr;

            CommandsMediator mediator = new CommandsMediator(voiceUI);

            WindowsInterOp.jab = new AccessBridge();
            WindowsInterOp.jab.Initialize();
            System.Windows.Forms.Application.DoEvents();


            WindowsInterOp.jab.Functions.GetAccessibleContextFromHWND(windowHandle, out vmId, out JavaObjectHandle joh);
            WindowsInterOp.vmId = vmId;

            while (true)
            {
                Widget wr = new Widget(javaApplications[appIndex - 1].WindowsTitle, joh, 0, 0, 0);

                WidgetsHelper.populateChildrenNodes(wr, mediator);
                sortedWidgets = new List<Widget>();
                string r = WidgetsHelper.sortWidgets(wr, sortedWidgets);
                r = WidgetsHelper.parseReading(r);
                Console.WriteLine(r);
                voiceUI.read(r);



                Command executableCommand = mediator.askAboutActions(sortedWidgets);

                if (executableCommand != null){
                    executableCommand.execute();
                }
                else{
                    break;
                }


            }
        }

    }
}
