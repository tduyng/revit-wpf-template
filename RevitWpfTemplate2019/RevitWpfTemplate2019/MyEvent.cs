
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;

namespace RevitWpfTemplate2019
{
    //Event for button
    public class MyEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            MessageBox.Show("Hi from external event");
        }

        public string GetName()
        {
            return "External Event";
        }
    }
}
