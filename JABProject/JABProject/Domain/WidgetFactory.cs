using JABProject;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsAccessBridgeInterop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Domain.Entities;
using JABProject.Utils.InterOp;

public static class WidgetFactory
{
    

    public static Widget CreateWidget(JavaObjectHandle handle, Widget parent, CommandsMediator cm)
    {
        WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, handle, out AccessibleContextInfo info);

        //AccessibleContextInfo contextInfo = jab.Functions.GetAccessibleChildFromContext(0, handle, 0);
        int x, y, height;
        Widget widget = null;

        x = info.x;
        y = info.y;
        height = info.height;

        switch(info.role)
        {
            case "push button":
                if (info.name != "")
                    widget = new Button(info.name, handle, x, y, height);
                break;
            case "text":
                widget = CreateTextVariant(info, handle, parent, x, y, height, cm);
                break;
            case "password text":
                widget = CreateTextVariant(info, handle, parent, x, y, height, cm);
                break;
            case "combo box":
                widget = CreateComboBoxVariant(info, handle, x, y, height, cm);
                break;
            case "page tab":
                widget = new PageTab(info.name, handle, x, y, height);
                break;
            case "label":
                widget = new Label(info.name, handle, x, y, height);
                break;
            case "menu":
                widget = new Menu(info.name, handle, x, y, height, cm);
                break;
            case "menu bar":
                widget = new MenuBar(info.name, handle, x, y, height, cm);
                break;
            case "menu item":
                widget = new MenuItem(info.name, handle, x, y, height);
                break;
            case "table":
                widget = new Table(info.name, handle, x, y, height, cm);
                break;
            case "tree":
                widget = new Tree(info.name, handle, x, y, height, cm);
                break;
            case "check box":
                widget = new CheckBox(info.name, handle, info.states.Contains("checked"), x, y, height);
                break;
            case "radio button":
                widget = new RadioButton(info.name, handle, info.states.Contains("checked"), x, y, height);
                break;
            case "spinbox":
                widget = new SpinBox(info.name, handle, x, y, height, cm);
                break;
            case "list":
                widget = new List(info.name, handle, x, y, height, cm);
                break;
            case "page tab list":
                widget = new PageTabList(info.name, handle, x, y, height, cm);
                break;
        }

        return widget;
    }


    private static Widget CreateTextVariant(AccessibleContextInfo contextInfo, JavaObjectHandle handle, Widget parent, int x, int y, int height, CommandsMediator cm)
    {
        
        string value = getTextContents(contextInfo, handle);
        if (contextInfo.childrenCount == 0)
        {
            return new TextField(value, contextInfo.name, handle, contextInfo.states.Contains("editable"), x, y, height, cm);
        }
        else
        {
            return new Text(value, contextInfo.name, handle, x, y, height);
        }
    }


    static string getTextContents(AccessibleContextInfo info, JavaObjectHandle handle)
    {
        if (info.accessibleText == 1)
        {
            AccessibleContextNode accContNode = new AccessibleContextNode(WindowsInterOp.jab, handle);
            WindowsInterOp.jab.Functions.GetAccessibleTextInfo(WindowsInterOp.vmId, handle, out AccessibleTextInfo textInfo, 0, 0);

            AccessibleTextReader reader = new AccessibleTextReader(accContNode, textInfo.charCount);

            var lines = reader
              .ReadFullLines(1024)
            .Where(x => !x.IsContinuation)
              .Take(1024);
            String value = "";


            foreach (var lineData in lines)
            {
                var lineEndOffset = lineData.Offset + lineData.Text.Length - 1;
                var name = string.Format("Line {0} [{1}, {2}]", lineData.Number + 1, lineData.Offset, lineEndOffset);
                value += lineData.Text + " ";

            }
            return value;
        }
        else
            return null;
    }


    private static JavaObjectHandle getListFromComboBox(JavaObjectHandle joh)
    {
        WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, joh, out AccessibleContextInfo info);
        int listChildrenCount = info.childrenCount;
        JavaObjectHandle result = null ;
        for (int lChildrenIndex = 0; lChildrenIndex < listChildrenCount && result == null; lChildrenIndex++)
        {

            JavaObjectHandle listChildren = WindowsInterOp.jab.Functions.GetAccessibleChildFromContext(WindowsInterOp.vmId, joh, lChildrenIndex);
            WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, listChildren, out AccessibleContextInfo childrenInfo);
            if (childrenInfo.role == "list")
            {
                return listChildren;
                //children.Add(new Label(childrenInfo.name, listChildren, childrenInfo.x, childrenInfo.y, childrenInfo.height));
            }
            else
            {
                if (childrenInfo.childrenCount > 0)
                {

                    result = getListFromComboBox(listChildren);

                }
                else
                {
                    return null;
                }
            }

        }
        return result;
    }

    private static Widget CreateComboBoxVariant(AccessibleContextInfo contextInfo, JavaObjectHandle handle, int x, int y, int height, CommandsMediator cm)
    {
        List<Widget> children = new List<Widget>();
        int childrenCount = contextInfo.childrenCount;
        TextFieldComboBox txtfcb = null;
        for (int cIndex = 0; cIndex < childrenCount; cIndex++) {

            JavaObjectHandle joh = WindowsInterOp.jab.Functions.GetAccessibleChildFromContext(WindowsInterOp.vmId, handle, cIndex);
            WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, joh, out AccessibleContextInfo info);
            if (info.role == "text")
            {
                string value = getTextContents(info, joh);
                txtfcb = new TextFieldComboBox(value, contextInfo.name, joh, handle, x, y, height, cm);
               
            }
            if (info.role == "popup menu")
            {
                JavaObjectHandle lh = getListFromComboBox(joh);
                WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, lh, out AccessibleContextInfo childrenInfo);
                int listChildrenCount = childrenInfo.childrenCount;

                for (int lChildrenIndex = 0; lChildrenIndex < listChildrenCount; lChildrenIndex++)
                {

                    JavaObjectHandle listChildren = WindowsInterOp.jab.Functions.GetAccessibleChildFromContext(WindowsInterOp.vmId, lh, lChildrenIndex);
                    WindowsInterOp.jab.Functions.GetAccessibleContextInfo(WindowsInterOp.vmId, listChildren, out AccessibleContextInfo lChildInfo);
                    if (lChildInfo.role == "label")
                    {
                        children.Add(new Label(lChildInfo.name, listChildren, lChildInfo.x, lChildInfo.y, lChildInfo.height));
                    }

                }


            }


        }
        Widget comboBoxVariant = txtfcb != null ? txtfcb: new ComboBox(contextInfo.name, handle, x, y, height, cm);
        
        foreach (Widget w in children)
            comboBoxVariant.Children.Add(w);

        return comboBoxVariant;
    }
}
