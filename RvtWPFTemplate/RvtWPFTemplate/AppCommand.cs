//Alls icons are using free from the website https://icons8.com/icons


#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using System.Reflection;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using RvtWPFTemplate.Properties;
using RvtWPFTemplate.UI;
using RvtWPFTemplate.Utilities;

#endregion

namespace RvtWPFTemplate
{
    public class AppCommand : IExternalApplication
    {

        public static ColorHandler Handler { get; set; } = null;
        public static ExternalEvent ExEvent { get; set; } = null;



        private readonly string _tabName = "TD";
        private static UIControlledApplication _uiApp;

        internal static string assemblyPath = typeof(AppCommand).Assembly.Location;

        /// <summary>
        /// Orivude access to this class instance
        /// </summary>
        internal static AppCommand GetInstance { get; private set; } = null;

       

        #region Result Startup
        public Result OnStartup(UIControlledApplication a)
        {
            try
            {
                GetInstance = this;
                _uiApp = a;
                //Buil all ribbon item
                BuildUI(a);

                Handler = new ColorHandler();
                ExEvent = ExternalEvent.Create(Handler);
                return Result.Succeeded;
            }
            catch (Exception eX)
            {
                TaskDialog td = new TaskDialog("Error in Setup");
                td.ExpandedContent = eX.GetType().Name + ": " + eX.Message + Environment.NewLine + eX.StackTrace;
                td.Show();
                return Result.Failed;
            }
        }
        #endregion

        #region Result Shutdown
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
        #endregion

        #region Create all ribbon item: tab, panel, button....
        //Create the buton on the panel
        private void BuildUI(UIControlledApplication uiApp)
        {

            // Create buttons of trackchanges panel
            var panel = RibbonUtils.CreatePanel(uiApp, _tabName, "Utilities");
            var data = new PushButtonData("btnColor", "Color\nElement", assemblyPath, typeof(CmdColorElement).FullName);
            data.LargeImage = RibbonUtils.ConvertFromBitmap(Properties.Resources.subscript_32);
            data.Image = RibbonUtils.ConvertFromBitmap(Properties.Resources.subscript_16);
            data.ToolTip = "Read level name and update for each elements in parameter VCF_Etage";
            var btnColor  = panel.AddItem(data) as PushButton;
           
            //instruction file to open by F1 key
            string instructionFile = @"https://github.com/tienduy-nguyen";
            ContextualHelp contextualHelp = new ContextualHelp(ContextualHelpType.Url, instructionFile);
            btnColor.SetContextualHelp(contextualHelp);
         
        }

        #endregion //Create all ribbon item: tab, panel, button

        /// <summary>
        /// Reset the top button to be the current one.
        /// Alternative solution: 
        /// set RibbonItem.IsSynchronizedWithCurrentItem 
        /// to false after creating the SplitButton.
        /// </summary>
        public static void SetTopButtonCurrent()
        {
            //IList<PushButton> sbList = splTrack.GetItems();
            //splTrack.CurrentButton = sbList[0];
        }

    }
}
