using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;

namespace DC3Safe.Controllers
{
    public class OccupationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OccupationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Occupations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Occupations.ToListAsync());
        }

        // GET: Occupations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occupation = await _context.Occupations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (occupation == null)
            {
                return NotFound();
            }

            return View(occupation);
        }

        // GET: Occupations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Occupations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Occupation occupation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(occupation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(occupation);
        }

        // GET: Occupations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occupation = await _context.Occupations.FindAsync(id);
            if (occupation == null)
            {
                return NotFound();
            }
            return View(occupation);
        }

        // POST: Occupations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Occupation occupation)
        {
            if (id != occupation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(occupation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OccupationExists(occupation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(occupation);
        }

        // GET: Occupations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occupation = await _context.Occupations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (occupation == null)
            {
                return NotFound();
            }

            return View(occupation);
        }

        // POST: Occupations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var occupation = await _context.Occupations.FindAsync(id);
            if (occupation != null)
            {
                _context.Occupations.Remove(occupation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OccupationExists(string id)
        {
            return _context.Occupations.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(ImportModel model)
        {
            if (model.ImportFile == null) return NotFound();

            if (!IsValidExtensionFile(model.ImportFile))
            {
                ModelState.AddModelError(string.Empty, "Extensión inválida.");
                return View(model);
            }

            try
            {
                using var wb = new ClosedXML.Excel.XLWorkbook(model.ImportFile.OpenReadStream());
                var ws = wb.Worksheet("Ocupaciones");
                var occupations = new List<Occupation>();
                bool rowHasData = true;
                int counter = 2;
                while (rowHasData)
                {
                    var row = ws.Row(counter);
                    if (row.IsEmpty()) break;

                    var name = row.Cell(1).GetValue<string>();
                    occupations.Add(new Occupation { Name = name });
                    counter++;
                }
                _context.Occupations.AddRange(occupations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
            }

            ModelState.AddModelError(string.Empty, "Error al leer datos.");
            return View(model);
        }

        private bool IsValidExtensionFile(IFormFile importTemplate)
        {
            string extension = Path.GetExtension(importTemplate.FileName).ToLower();

            return string.Equals(extension, ".xlsx");
        }
    }
}
