using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;

namespace RvtWPFTemplate.Utilities
{
    public class ElementUtils
    {
        /// <summary>
        /// Retrieve Element instances in view specifique
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IList<Element> GetElementInstanceInView(Document doc, View view)
        {
            IList<Element> elements = new List<Element>();
            Options opt = new Options();
            FilteredElementCollector collector = new FilteredElementCollector(doc, view.Id);
            collector
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .Where<Element>(e =>
                   (null != e.get_BoundingBox(null))
                   && (null != e.get_Geometry(opt)));


            foreach (Element element in collector)
            {
                if (null != element.Category
                  && 0 < element.Parameters.Size
                && element.Category.HasMaterialQuantities)
                {
                    elements.Add(element);
                }
            }
            return elements;

        }


        /// <summary>
        /// Get pre-selected elements
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<Element> GetElementPreSelected(Document doc)
        {
            UIDocument uidoc = new UIDocument(doc);
            Selection sel = uidoc.Selection;
            bool isPreSelected = IsPreSelectedElement(doc);
            if (isPreSelected)
                return sel.GetElementIds().Select(x => doc.GetElement(x)).ToList();
            return new List<Element>();
        }
       

        /// <summary>
        /// Detect if in selecting in Revit
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static bool IsPreSelectedElement(Document doc)
        {
            try
            {
                UIDocument uidoc = new UIDocument(doc);
                Selection sel = uidoc.Selection;
                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
                if (selectedIds.Count > 0) return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
        

    }
}
