using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RvtWPFTemplate.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;


namespace RvtWPFTemplate.UI
{
    public class ColorHandler : IExternalEventHandler
    {
        private static Document _doc;
        private static UIApplication _uiapp;
        private static Autodesk.Revit.ApplicationServices.Application _app;
        private static UIDocument _uidoc;
        private ColorViewModel viewModel = null;

        public ColorViewModel ViewModel { get { return viewModel; } set { viewModel = value; } }

        public void Execute(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _uidoc = uiapp.ActiveUIDocument;
            _app = uiapp.Application;
            _doc = uiapp.ActiveUIDocument.Document;
            try
            {
                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            return;
                        }
                    case RequestId.SetColor:
                        {
                            if (viewModel.SelectOption == ComponentOption.OnlyVisible)
                            {
                               ColorVisibleElement();

                            }
                            else if (viewModel.SelectOption == ComponentOption.SelectedElements)
                            {
                               ColorSelectedElement();
                            }
                            break;
                        }
                   
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Failed to execute the external event.\n" + ex.Message, "Execute Event", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public string GetName()
        {
            return "Task External Event1";
        }

        public CommunicatorRequest Request { get; set; } = new CommunicatorRequest();

        public class CommunicatorRequest
        {
            private int _request = (int)RequestId.None;

            public RequestId Take()
            {
                return (RequestId)Interlocked.Exchange(ref _request, (int)RequestId.None);
            }

            public void Make(RequestId request)
            {
                Interlocked.Exchange(ref _request, (int)request);
            }
        }

        public enum RequestId
        {
            None,
            SetColor
            
        }

        #region External event
        private void ColorVisibleElement()
        {
            var view = _doc.ActiveView;
            var elemIds = ElementUtils.GetElementInstanceInView(_doc, view).Select(x => x.Id).ToList();
            RunSetColor(view, elemIds);
        }
        private void ColorSelectedElement()
        {
            var view = _doc.ActiveView;
            var elemIds = ElementUtils.GetElementPreSelected(_doc).Select(x => x.Id).ToList();
            RunSetColor(view, elemIds);
        }

        #endregion External event


        #region private Method
        private void RunSetColor(Autodesk.Revit.DB.View view, IList<ElementId> elemIds)
        {
            DisableTemperaryMode();
            _uidoc.Selection.SetElementIds(elemIds);
            _uidoc.ShowElements(elemIds);

            var color = viewModel.RvtColorElement;

            try
            {

                var ogs = new Autodesk.Revit.DB.OverrideGraphicSettings();

                var patternCollector = new FilteredElementCollector(_doc.ActiveView.Document);
                patternCollector.OfClass(typeof(Autodesk.Revit.DB.FillPatternElement));
                Autodesk.Revit.DB.FillPatternElement solidFill = patternCollector.ToElements().Cast<Autodesk.Revit.DB.FillPatternElement>().First(x => x.GetFillPattern().IsSolidFill);
#if REVIT2018
                ogs.SetProjectionFillColor(color);
                ogs.SetProjectionFillPatternId(solidFill.Id);
                ogs.SetProjectionLineColor(color);
                ogs.SetCutFillColor(color);
                ogs.SetCutFillPatternId(solidFill.Id);
                ogs.SetCutLineColor(color);
#else
                ogs.SetSurfaceForegroundPatternColor(color);
                ogs.SetSurfaceForegroundPatternId(solidFill.Id);
                ogs.SetProjectionLineColor(color);
                ogs.SetCutForegroundPatternColor(color);
                ogs.SetCutForegroundPatternId(solidFill.Id);
                ogs.SetCutLineColor(color);
#endif 

                using (Transaction t = new Transaction(_doc, "Set Element Override"))
                {
                    t.Start();
                    foreach (var id in elemIds)
                    {
                        _doc.ActiveView.SetElementOverrides(id, ogs);
                    }

                    t.Commit();
                }
            }
            catch
            {
                throw;
            }

        }

        private void SaveSettingsJson()
        {
            JsonUtils.Save<ColorSettings>(ColorViewModel.ColorSettingsFiles, ColorViewModel._ColorSettings);
        }

        private void DisableTemperaryMode()
        {
            try
            {
                View view = _doc.ActiveView;
                using (Transaction tr = new Transaction(_doc, "Unhide"))
                {
                    tr.Start();
                    if (view.IsTemporaryHideIsolateActive())
                    {
                        TemporaryViewMode tempView = TemporaryViewMode.TemporaryHideIsolate;
                        view.DisableTemporaryViewMode(tempView);
                    }
                    tr.Commit();
                }
            }
            catch { }
        }

        #endregion

    }
}