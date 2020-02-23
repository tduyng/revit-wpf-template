using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace RvtWPFTemplate.Utilities
{
    public class RibbonUtils
    {
        #region Helper pour create panel    
        public static RibbonPanel CreatePanel(UIControlledApplication a, string tabName, string panelName)
        {
            RibbonPanel ribbonPanel = null;
            try
            {
                a.CreateRibbonTab(tabName);
            }
            catch { }
            // Try to create ribbon panel.
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tabName, panelName);
            }
            catch { }
            // Search existing tab for your panel.
            List<RibbonPanel> panels = a.GetRibbonPanels(tabName);
            foreach (RibbonPanel p in panels)
            {
                if (p.Name == panelName)
                {
                    ribbonPanel = p;
                }
            }
            return ribbonPanel;
        }
        #endregion //Helper pour create panel

        #region ImageUtils
        /// <summary>
        /// Get Image form resources.resx
        /// </summary>
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] //Creates a GDI bitmap object from a GDI+
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ConvertFromBitmap(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            //Intptr: A handle to the GDI bitmap object that this method creates.
            IntPtr hBitmap = image.GetHbitmap();
            try
            {
                //Returns a managed BitmapSource, based on the provided pointer to an unmanaged bitmap and palette information
                var bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                return bs;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
        public static BitmapSource ConvertFromBitmap(Icon icon)
        {
            try
            {
                //Returns a managed BitmapSource, based on the provided pointer to an unmanaged icon image.
                var bs = Imaging
                    .CreateBitmapSourceFromHIcon(icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromWidthAndHeight(icon.Width, icon.Height));
                return bs;
            }
            finally
            {
                DeleteObject(icon.Handle);
            }
        }


        /// <summary>
        /// Get Image from resources folder
        /// </summary>
        /// <param name="embededlargeImageName"></param>
        /// <returns></returns>

        // Case: when we dont bother the extention of image input and return an imagsesource of Image
        public static ImageSource GetEmbededImageFromSource(string embededlargeImageName)
        {
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embededlargeImageName);
                string imageExtension = Path.GetExtension(embededlargeImageName);

                if (imageExtension.Equals(".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    PngBitmapDecoder img = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return img.Frames[0];
                }

                if (imageExtension.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase))
                {
                    BmpBitmapDecoder img = new BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return img.Frames[0];

                }

                if (imageExtension.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                {
                    JpegBitmapDecoder img = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return img.Frames[0];

                }

                if (imageExtension.Equals(".tiff", StringComparison.CurrentCultureIgnoreCase))
                {
                    TiffBitmapDecoder img = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return img.Frames[0];

                }

                if (imageExtension.Equals(".ico", StringComparison.CurrentCultureIgnoreCase))
                {
                    IconBitmapDecoder img = new IconBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return img.Frames[0];
                }
            }
            catch { return null; }
            return null;
        }

        public static BitmapImage GetIconFromAFolder(string imageFolder, string imageFullName)
        {
            try
            {
                return new BitmapImage(new Uri(Path.Combine(imageFolder, imageFullName)));
            }
            catch
            {
                return null;
            }
        }
        #endregion 
    }
}
