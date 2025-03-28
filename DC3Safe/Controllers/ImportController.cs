using DC3Safe.Data;
using DC3Safe.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DC3Safe.Controllers
{
    public class ImportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Occupations(ImportModel model)
        {
            if (!IsValidImportTemplateFile(model.ImportFile))
            {
                ModelState.AddModelError(string.Empty, "Archivo inválido.");
                return View("~/Views/Occupations/Import.cshtml", model);
            }

            try
            {
                var occupations = new List<Occupation>();
                using var wb = new XLWorkbook(model.ImportFile.OpenReadStream());
                var ws = wb.Worksheet(ImportWorksheetNames.Occupations);
                var firstRowUsed = ws.FirstRowUsed().RowBelow();
                var occupationRow = firstRowUsed.RowUsed();

                while (!occupationRow.Cell(1).IsEmpty())
                {
                    string name = occupationRow.Cell(1).GetString();
                    occupations.Add(new Occupation { Name = name});
                    occupationRow = occupationRow.RowBelow();
                }

                _context.Occupations.AddRange(occupations);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al leer datos.");
                return View("~/Views/Occupations/Import.cshtml", model);
            }

            return RedirectToAction("Index", "Occupations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProgramsInformation(ImportModel model)
        {
            if (!IsValidImportTemplateFile(model.ImportFile))
            {
                ModelState.AddModelError(string.Empty, "Archivo inválido.");
                return View("~/Views/ProgramsInformation/Import.cshtml", model);
            }

            try
            {
                using var wb = new XLWorkbook(model.ImportFile.OpenReadStream());
                var ws = wb.Worksheet(ImportWorksheetNames.ProgramsInformation);
                var firstRowUsed = ws.FirstRowUsed().RowBelow();
                var programRow = firstRowUsed.RowUsed();

                var categoryList = new List<ProgramCategory>();
                var programs = new List<ProgramInformation>();
                while (!programRow.Cell(1).IsEmpty())
                {
                    string categoryName = programRow.Cell(5).GetString();

                    var category = categoryList.Where(x => x.Name == categoryName).FirstOrDefault();
                    if(category == null)
                    {
                        category = await _context.ProgramCategories
                            .FirstOrDefaultAsync(x => x.Name == categoryName);
                    }
                    if (category == null)
                    {
                        throw new Exception($"Category {categoryName} not found");
                    }

                    categoryList.Add(category);
                    var program = new ProgramInformation
                    {
                        Name = programRow.Cell(1).GetString(),
                        DurationHours = (int)programRow.Cell(2).GetDouble(),
                        StartDate = programRow.Cell(3).GetDateTime(),
                        EndDate = programRow.Cell(4).GetDateTime(),
                        CategoryId = category.Id
                    };
                    programs.Add(program);
                    programRow = programRow.RowBelow();
                }

                _context.ProgramsInformation.AddRange(programs);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"Error al leer datos. ${e.Message}");
                return View("~/Views/ProgramsInformation/Import.cshtml", model);
            }

            return RedirectToAction("Index", "ProgramsInformation");
        }


        private bool IsValidImportTemplateFile(IFormFile importTemplate)
        {
            return IsValidExtensionFile(importTemplate)
                && IsValidFileContent(importTemplate);
        }

        private bool IsValidExtensionFile(IFormFile importTemplate)
        {
            string extension = Path.GetExtension(importTemplate.FileName).ToLower();

            return string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidFileContent(IFormFile importTemplate)
        {
            // .xlsx
            byte[] _fileSignature = { 80, 75, 3, 4, 20 };
            using var reader = new BinaryReader(importTemplate.OpenReadStream());

            return reader.ReadBytes(_fileSignature.Length)
                .SequenceEqual(_fileSignature);
        }
    }
}
