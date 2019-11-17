#region Namespaces
using System;
using System.Collections.Generic;
using ApiViet.Ribbon;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitWpf.Properties;

#endregion

namespace RevitWpfTemplate2019
{
    class App : IExternalApplication
    {
        private MyForm m_MyForm;
        public static App thisApp = null;



        public Result OnStartup(UIControlledApplication a)
        {
            m_MyForm = null;   // no dialog needed yet; the command will bring it
            thisApp = this;  // static access to this application instance
            CreateRibbonItem(a); 
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            //dispose window wpf after using
            if (m_MyForm != null && m_MyForm.IsVisible)
            {
                m_MyForm.Close();
                
            }
            return Result.Succeeded;
        }

        //Create the buton on the panel
        private void CreateRibbonItem(UIControlledApplication uiApp)
        {
            CustomRibbon ribbon = new CustomRibbon(uiApp);
            var myTab = ribbon.Tab("TD");
            var panelLearning = myTab.Panel("Learning");
            var btn1 = panelLearning
                .CreateButton("btnGetView",
                    "GetView",
                    typeof(CmdMyCommand),
                    btn => btn
                        .SetLargeImage(Resources.icon)
                        .SetSmallImage(Resources.icon)
                        .SetContextualHelp(ContextualHelpType.Url, "https://help.autodesk.com"))
               .CreateButton("btnRetrieveParam",
                    "ExternalEvent",
                    typeof(CmdMyCommand),
                    btn => btn
                        .SetLargeImage(Resources.icon)
                        .SetSmallImage(Resources.icon)
                        .SetContextualHelp(ContextualHelpType.Url, "https://help.autodesk.com"));

        }


        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (m_MyForm is null || m_MyForm.DialogResult is null)
            {
                // A new handler to handle request posting by the dialog
                //My event is a external command that we want to do with button
                MyEvent handler = new MyEvent();

                // External Event for the dialog to use (to post requests)
                ExternalEvent exEvent = ExternalEvent.Create(handler);

                // We give the objects to the new dialog;
                // The dialog becomes the owner responsible fore disposing them, eventually.
                m_MyForm = new MyForm(exEvent, handler);
                m_MyForm.Show();
            }
        }
    }
}
