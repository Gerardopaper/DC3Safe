using Microsoft.AspNetCore.Mvc;
using ClosedXML.Extensions;
using DC3Safe.Models;

namespace DC3Safe.Controllers
{
    public class ImportTemplatesController : Controller
    {
        public IActionResult Download(ExcelTemplate template)
        {
            using var workSheet = GenerateImportTemplate(template);
            if (workSheet == null) return NotFound();

            return workSheet.Deliver("import_template.xlsx");
        }

        private ClosedXML.Excel.XLWorkbook? GenerateImportTemplate(ExcelTemplate template)
        {
            switch (template)
            {
                case ExcelTemplate.Occupation:
                    return GenerateOccupationsTemplate();
                default:
                    return null;
            }
        }

        private ClosedXML.Excel.XLWorkbook GenerateOccupationsTemplate()
        {
            var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.AddWorksheet("Ocupaciones");
            ws.Cell("A1").Value = "Nombre";
            return wb;
        }
    }
}
