using NReco.PdfGenerator;
using System.IO;
using System.Web.Mvc;

namespace _2DRakun.Code
{
    public class PdfHelper
    {
        public static byte[] GeneratePdfFromHtml(string html)
        {
            var converter = new HtmlToPdfConverter
            {
                Size = PageSize.A4,
                Orientation = PageOrientation.Portrait,
                Margins = new PageMargins
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 15,
                    Right = 15
                }
            };

            return converter.GeneratePdf(html);
        }

        public static string RenderViewToString(
        ControllerContext context,
        string viewName,
        object model)
        {
            var viewResult = ViewEngines.Engines.FindView(context, viewName, null);
            context.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    viewResult.View,
                    context.Controller.ViewData,
                    context.Controller.TempData,
                    sw);

                viewResult.View.Render(viewContext, sw);
                return sw.ToString();
            }
        }

    }
}