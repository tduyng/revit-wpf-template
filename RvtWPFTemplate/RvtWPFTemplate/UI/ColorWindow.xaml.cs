using Autodesk.Revit.UI;
using Microsoft.Win32;
using RvtWPFTemplate.Utilities;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;


namespace RvtWPFTemplate.UI
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class ColorWindow : Window
    {
        public static ColorWindow Instance { get; set; } = null;
        public ColorWindow(ColorViewModel vm)
        {

            #region Set ower revit window for Wpf form
            if (!ColorViewModel.IsOpen) //Enforce single window
            {
                InitializeComponent();
                Instance = this;
                var uiapp = ColorViewModel.Uiapp;
                IntPtr revitWindow;

#if REVIT2018
                revitWindow = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle; // 2018
#else
                revitWindow = uiapp.MainWindowHandle; //Revit 2019 and above
#endif

                //Get window of Revit form Revit handle
                HwndSource hwndSource = HwndSource.FromHwnd(revitWindow);
                var windowRevitOpen = hwndSource.RootVisual as Window;
                #endregion


                this.Owner = windowRevitOpen; //Set onwer for WPF window
                this.DataContext = vm;

                if (vm.DisplayUI())
                {
                    this.Show(); //Modeless form
                } 

            }
            //Check: if Wpf window is minimized, re-open it
            if (Instance?.WindowState == WindowState.Minimized)
                Instance.WindowState = WindowState.Normal;
        }

    }
}
