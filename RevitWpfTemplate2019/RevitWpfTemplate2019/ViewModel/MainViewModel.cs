using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitWpfTemplate2019
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand BtnOKCommand { get; set; }


        //  Commands
        // This will be used by the button in the WPF window.
        public MainViewModel()
        {
            BtnOKCommand = new RelayCommand<object>((p) => true,
                (p) => {
                    MyForm.m_ExEvent.Raise();
                });
        }

      
    }
}
