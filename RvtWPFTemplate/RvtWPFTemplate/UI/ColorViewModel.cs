using System;
using System.IO;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RvtWPFTemplate.Utilities;


namespace RvtWPFTemplate.UI
{
    public class ColorViewModel : BaseViewModel
    {
        public RelayCommand WindowLoaded { get; set; }
        public RelayCommand WindowClosed { get; set; }
        public RelayCommand ColorSettingsElement { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }

        private static UIApplication _uiapp = null;
        private static Document _doc = null;
        private Autodesk.Revit.DB.Color rvtColorElement;
        private ComponentOption selectOption = ComponentOption.OnlyVisible;
        private ColorHandler _handler = null;

        public Autodesk.Revit.DB.Color RvtColorElement { get { return rvtColorElement; } set { rvtColorElement = value; OnPropertyChanged(); } }
        public ComponentOption SelectOption { get { return selectOption; } set { selectOption = value; OnPropertyChanged(); } }
        public static ColorViewModel Instance { get; set; }
        public static Document Doc { get { return _doc; } set { _doc = value; } }
        public static UIApplication Uiapp { get { return _uiapp; } set { _uiapp = value; } }
        public static bool IsOpen { get; private set; } = false;
        public static ColorSettings _ColorSettings { get; set; }
        public static string ColorSettingsFiles = string.Empty;

        public ColorViewModel(UIApplication uiapp, ColorHandler handler)
        {
            Instance = this;
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _handler = handler;

            try
            {
                WindowLoaded = new RelayCommand(param => this.LoadedExecuted(param));
                WindowClosed = new RelayCommand(param => this.ClosedExecuted(param));
                ColorSettingsElement = new RelayCommand(param => this.ColorElementExecuted(param));
                ApplyCommand = new RelayCommand(param => this.ApplyExecuted(param));
                CancelCommand = new RelayCommand(param => this.CancelExecuted(param));

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

        }


        #region Public methods
        //Get all infos before show modeless form
        public bool DisplayUI()
        {
            bool result = false;
            try
            {
                CollectSettingsJson();

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to display UI components\n" + ex.Message, "TD: DisplayUI", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return result;
        }
        #endregion //Public methods

        #region Event command
        private void LoadedExecuted(object param)
        {
            IsOpen = true;
        }
        private void ClosedExecuted(object param)
        {
            IsOpen = false;
        }
        private void CancelExecuted(object param)
        {
            var win = param as Window;
            win.Close();
        }
        //Raise methos when click apply button
        private void ApplyExecuted(object param)
        {
            _handler.ViewModel = this;
            AppCommand.Handler.Request.Make(ColorHandler.RequestId.SetColor);
            AppCommand.ExEvent.Raise();
            RevitUtils.SetFocusToRevit();
        }

        //Get colorSetting from color selected
        private void ColorElementExecuted(object param)
        {
            //Model.ColorSettings();
            ColorSelectionDialog colorSelectionDialog = new ColorSelectionDialog();
            colorSelectionDialog.Show();
            RvtColorElement = colorSelectionDialog.SelectedColor;
            _ColorSettings.ColorElement = RvtColorElement;

        }

        #endregion

        #region Private method
        private void CollectSettingsJson()
        {
            ColorSettingsFiles = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ColorSettings.json";
            _ColorSettings = JsonUtils.Load<ColorSettings>(ColorSettingsFiles);

            //Get Color Revit form color defaut of wpf
            var cNewElement = System.Drawing.Color.Red; //defaut Red color
            RvtColorElement = _ColorSettings.ColorElement ?? new Autodesk.Revit.DB.Color(cNewElement.R, cNewElement.G, cNewElement.B);
           
        }
        #endregion



    }
}
