using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using ColorRevit = Autodesk.Revit.DB.Color;

namespace RvtWPFTemplate.UI
{
    [ValueConversion(typeof(Autodesk.Revit.DB.Color), typeof(System.Windows.Media.Brush))]
    public class BrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result = System.Windows.Media.Brushes.Red;

            if (value != null)
            {
                try
                {
                    ColorRevit rvt = value as ColorRevit;
                    System.Drawing.Color colorSystem = System.Drawing.Color.FromArgb(rvt.Red, rvt.Green, rvt.Blue);
                    result = new SolidColorBrush(System.Windows.Media.Color.FromArgb(colorSystem.A, colorSystem.R, colorSystem.G, colorSystem.B));
                    return result;
                }
                catch { }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result = new Autodesk.Revit.DB.Color(255, 0, 0);
            if (value != null)
            {
                try
                {
                    System.Windows.Media.Brush wpf = value as System.Windows.Media.Brush;
                    var colorSystem = ((SolidColorBrush)wpf).Color;
                    result = new Autodesk.Revit.DB.Color(colorSystem.R, colorSystem.G, colorSystem.B);
                    return result;
                }
                catch { }
            }
            return result;
        }
    }
}
