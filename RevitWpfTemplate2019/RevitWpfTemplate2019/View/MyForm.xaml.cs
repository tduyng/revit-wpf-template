using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitWpfTemplate2019
{
    /// <summary>
    /// Interaction logic for MyForm.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        public static ExternalEvent m_ExEvent { get; set; }
        public MyEvent m_Handler { get; set; }

        public MyForm(ExternalEvent exEvent, MyEvent handler)
        {
            InitializeComponent();

            //For material design
            ColorZoneAssist.SetMode(new GroupBox(), ColorZoneMode.Accent);
            Hue hue = new Hue("name", System.Windows.Media.Color.FromArgb(1, 2, 3, 4), System.Windows.Media.Color.FromArgb(1, 5, 6, 7));


            m_ExEvent = exEvent;
            m_Handler = handler;
        }
    }
}
