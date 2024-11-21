using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using WindowsAccessBridgeInterop.Win32;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Runtime.Remoting.Contexts;
using static WindowsAccessBridgeInterop.Win32.NativeMethods;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Threading;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static JABProject.Form1;
using JABProject.Controller;
using JABProject.Utils;
using Domain.Entities;
using Domain.Commands;

namespace JABProject
{

    public partial class Form1 : Form
    {
        VoiceInputHandler voiceHandler;
        private Dictionary<string, Command> voiceInputToCommand;
        
        List<Action> actions = new List<Action>();
        List<Widget> sortedWidgets = new List<Widget>();
        List<String> validReadableWidgets = new List<String>();
        List<Widget> readings = new List<Widget>();
        
        public static AccessBridge accessBridge;
        Int32 vmId;
        

        public Form1()
        {

            InitializeComponent();

        }

        

        private void readApplication()
        {

            //loadSpeechRecognition();
            //loadGrammar("one", "two", "three");
            //IntPtr windowHandle = FindWindow(null, "Document classifier");
            //IntPtr windowHandle = FindJavaWindow("javaw", "jp2launcher", "java");
            IntPtr windowHandle = IntPtr.Zero;
            
            List<JavaApplication> javaApplications = WindowsInterOp.FindJavaWindow("javaw", "jp2launcher", "java");
            int appIndex = voiceHandler.chooseApp(javaApplications);

            windowHandle = javaApplications[appIndex-1].ProcessPtr;

            CommandsMediator mediator = new CommandsMediator(voiceHandler);

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
                voiceHandler.readText(r);
                


                Command executableCommand = voiceHandler.askAboutActions(sortedWidgets);
                executableCommand.execute();


            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            voiceHandler = new VoiceInputHandler();
            Thread waitingThread = new Thread(readApplication);
            waitingThread.Start();

        }

        
        
        

    }
}
