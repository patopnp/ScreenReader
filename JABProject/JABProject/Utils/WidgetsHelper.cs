using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABProject.Utils
{
    static class WidgetsHelper
    {


        static void assignWidgetsMissingName(List<Widget> row)
        {
            string leftLabel = null;
            for (int i = 0; i < row.Count; i++)
            {
                if (row[i].getWidgetType() == "Label" && row[i].Name != "")
                {
                    leftLabel = row[i].Name;
                    continue;
                }
                string role = row[i].getWidgetType();
                if ((role == "TextField" || role == "TextFieldComboBox" || role == "ComboBox" || role == "List") && row[i].Name == "")
                {
                    row[i].Name = leftLabel;
                }
            }
        }

        public static string parseReading(string reading)
        {
            if (string.IsNullOrEmpty(reading))
                return reading;

            // Find the index where the last letter appears
            int lastLetterIndex = reading.LastIndexOf(reading.LastOrDefault(char.IsLetter));

            // If no letters are found, return an empty string
            if (lastLetterIndex == -1)
                return string.Empty;

            // Trim the part of the string after the last letter
            return reading.Substring(0, lastLetterIndex + 1);
        }

        
        public static string sortWidgets(Widget w, List<Widget> sortedWidgets)
        {

            string forReading = "";

            List<Widget> listOfWidgets = w.Children.ToList<Widget>();


            while (listOfWidgets.Count > 0)
            {
                Widget topWidget = listOfWidgets.Aggregate((min, next) => next.Y < min.Y ? next : min);
                List<Widget> row = new List<Widget>();
                foreach (Widget wr in listOfWidgets)
                {
                    int height = 20;
                    if (topWidget.Height < 10)
                        height = 10;

                    if (wr.Y > topWidget.Y - height && wr.Y < topWidget.Y + height)
                    {
                        row.Add(wr);
                    }
                }
                //sorted.Add(new WidgetReading("below", null, 0, 0, 0));
                row = row.OrderBy(reading => reading.X).ToList();
                assignWidgetsMissingName(row);


                foreach (Widget wr in row)
                {
                    listOfWidgets.Remove(wr);
                    //if (sortedWidgets.Contains(wr) == false)
                    // Puede que se repita el nombre y sea valido
                    if (sortedWidgets.Any(o => o.getTag() == wr.getTag()) == false)
                    {

                        if (wr.getWidgetType() == "CheckBox")
                        {
                            forReading += ((CheckBox)wr).IsChecked ? "checked " : "unchecked ";
                        }
                        else if (wr.getWidgetType() == "RadioButton")
                        {
                            forReading += ((RadioButton)wr).IsChecked ? "checked " : "unchecked ";
                        }

                        forReading += wr.getTag() + ", ";
                        sortedWidgets.Add(wr);
                    }
                    if (wr.Children.Count > 0)
                    {

                        if (wr.getWidgetType() != "List" && wr.getWidgetType() != "TextFieldComboBox")
                        {
                            forReading += "inside " + wr.getTag() + ", ";
                            forReading += sortWidgets(wr, sortedWidgets);
                            //if (wr.Name != "Options list")
                            forReading += "outside " + wr.getTag() + ", ";
                        }
                        else
                        {
                            forReading += "inside " + wr.getTag() + ", ";

                            int i = 0;
                            for (i = 0; i < 3 && i < wr.Children.Count; i++)
                            {
                                forReading += wr.Children[i].getTag() + ", ";
                                sortedWidgets.Add(wr.Children[i]);
                                //traverseChildren(wr);
                            }
                            if (i != wr.Children.Count)
                            {
                                forReading += "more... ";
                            }

                            //if (wr.Name != "Options list")
                            forReading += "outside " + wr.getTag() + ", ";
                        }
                    }
                }

            }

            return forReading;
        }

        public static void populateChildrenNodes(Widget w, CommandsMediator cm)
        {
            exploreChildrenJavaObjectHandle(w.Handle, w, cm);
        }

        public static void exploreChildrenJavaObjectHandle(JavaObjectHandle handle, Widget parent, CommandsMediator cm)
        {
            Widget w = WidgetFactory.CreateWidget(handle, parent, cm);
            WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, handle, out AccessibleContextInfo accessibleContextInfo);
            /*
            bool exploreInto = accessibleContextInfo.states.Contains("showing");
            */
            if (accessibleContextInfo.states.Contains("showing") == false && parent.getWidgetType() != "Table") return;

            if (w != null)
            {
                parent.Children.Add(w);
                parent = w;
            }

            for (int i = 0; i < accessibleContextInfo.childrenCount; i++)
            {
                var childJavaObjectHandle = WindowsInterOp.jab.Functions.GetAccessibleChildFromContext(WindowsInterOp.vmId, handle, i);
                exploreChildrenJavaObjectHandle(childJavaObjectHandle, parent, cm);
            }
        }
    }
}
