using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System;
using RvtWPFTemplate.UI;

namespace RvtWPFTemplate
{
    [Transaction(TransactionMode.Manual)]
    public class CmdColorElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Execute(commandData.Application);
        }

        //Call this methos with the external command
        public Result Execute(UIApplication uiapp)
        {
            try
            {
                var view = new ColorWindow(new ColorViewModel(uiapp, AppCommand.Handler));
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error! " + ex);
                return Result.Failed;
            }

        }

    }
}
