using Microsoft.AspNetCore.Mvc;
using ClosedXML.Extensions;
using DC3Safe.Models;
using DC3Safe.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace DC3Safe.Controllers
{
    public class ImportTemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImportTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Download(ExcelTemplate template)
        {
            using var workSheet = await GenerateImportTemplateAsync(template);
            if (workSheet == null) return NotFound();

            return workSheet.Deliver("import_template.xlsx");
        }

        private async Task<XLWorkbook?> GenerateImportTemplateAsync(ExcelTemplate template)
        {
            switch (template)
            {
                case ExcelTemplate.Occupations:
                    return GenerateOccupationsTemplate();
                case ExcelTemplate.ProgramsInformation:
                    return await GenerateProgramsInformationTemplateAsync();
                default:
                    return null;
            }
        }

        private XLWorkbook GenerateOccupationsTemplate()
        {
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet(ImportWorksheetNames.Occupations);
            ws.Cell("A1").Value = "Nombre";
            return wb;
        }

        private async Task<XLWorkbook> GenerateProgramsInformationTemplateAsync()
        {
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet(ImportWorksheetNames.ProgramsInformation);
            ws.Cell("A1").Value = "Nombre del curso";
            ws.Cell("B1").Value = "Duración en horas";
            ws.Cell("C1").Value = "Periodo de ejecución (Inicio)";
            ws.Cell("D1").Value = "Periodo de ejecución (Fin)";
            ws.Cell("E1").Value = "Área temática del curso";
            ws.Range("A1:E1").Style.Fill.BackgroundColor = XLColor.PowderBlue;
            ws.Columns(1, 5).AdjustToContents();
            ws.Range("C2:C101").DataType = XLDataType.DateTime;            
            ws.Range("D2:D101").DataType = XLDataType.DateTime;
            
            var validation = ws.Range("E2:E101").CreateDataValidation();
            var categories = await _context.ProgramCategories
                .Select(x => x.Name)
                .ToListAsync();
            var validOptions = $"\"{string.Join(",", categories)}\"";
            validation.List(validOptions, true);
            return wb;
        }
    }
}
