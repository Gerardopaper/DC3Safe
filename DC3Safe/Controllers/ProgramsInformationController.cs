using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;
using DC3Safe.Services.DataTable;

namespace DC3Safe.Controllers
{
    public class ProgramsInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramsInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramInformations
        public IActionResult Index()
        {
            return View();
        }

        // GET: ProgramInformations
        public async Task<IActionResult> List([FromQuery] DataTableRequest request)
        {
            IQueryable<ProgramInformation> query = _context.ProgramsInformation;
            int total = await query.CountAsync();
            query = FilterPrograms(query, request);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    duration = x.DurationHours,
                    start_date = x.StartDate.ToString("yyyy/MM/dd"),
                    end_date = x.EndDate.ToString("yyyy/MM/dd"),
                    category = x.Category!.Name
                })
                .ToDataTableAsync(request, total);

            return Json(data);
        }

        private IQueryable<ProgramInformation> FilterPrograms(IQueryable<ProgramInformation> query, DataTableRequest request)
        {
            if (!string.IsNullOrEmpty(request.search))
            {
                string trimmedSearch = request.search.Trim();
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{trimmedSearch}%"));
            }

            switch ((request.column, request.dir))
            {
                case (1, "asc"):
                    query = query.OrderBy(x => x.Name);
                    break;
                case (1, "desc"):
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case (2, "asc"):
                    query = query.OrderBy(x => x.DurationHours);
                    break;
                case (2, "desc"):
                    query = query.OrderByDescending(x => x.DurationHours);
                    break;
                case (3, "asc"):
                    query = query.OrderBy(x => x.StartDate);
                    break;
                case (3, "desc"):
                    query = query.OrderByDescending(x => x.StartDate);
                    break;
                case (4, "asc"):
                    query = query.OrderBy(x => x.EndDate);
                    break;
                case (4, "desc"):
                    query = query.OrderByDescending(x => x.EndDate);
                    break;
                case (5, "asc"):
                    query = query.OrderBy(x => x.Category!.Name);
                    break;
                case (5, "desc"):
                    query = query.OrderByDescending(x => x.Category!.Name);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }
            return query;
        }

        // GET: ProgramInformations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programInformation = await _context.ProgramsInformation
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programInformation == null)
            {
                return NotFound();
            }

            return View(programInformation);
        }

        // GET: ProgramInformations/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ProgramCategories, "Id", "Name");
            return View();
        }

        // POST: ProgramInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DurationHours,StartDate,EndDate,CategoryId")] ProgramInformation programInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProgramCategories, "Id", "Name", programInformation.CategoryId);
            return View(programInformation);
        }

        // GET: ProgramInformations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programInformation = await _context.ProgramsInformation.FindAsync(id);
            if (programInformation == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ProgramCategories, "Id", "Name", programInformation.CategoryId);
            return View(programInformation);
        }

        // POST: ProgramInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,DurationHours,StartDate,EndDate,CategoryId")] ProgramInformation programInformation)
        {
            if (id != programInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramInformationExists(programInformation.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.ProgramCategories, "Id", "Name", programInformation.CategoryId);
            return View(programInformation);
        }

        // GET: ProgramInformations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programInformation = await _context.ProgramsInformation
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programInformation == null)
            {
                return NotFound();
            }

            return View(programInformation);
        }

        // POST: ProgramInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var programInformation = await _context.ProgramsInformation.FindAsync(id);
            if (programInformation != null)
            {
                _context.ProgramsInformation.Remove(programInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchDelete([FromBody] string[] ids)
        {
            if (ids.Length == 0) return NotFound();
            var programs = await _context.ProgramsInformation
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            _context.ProgramsInformation.RemoveRange(programs);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool ProgramInformationExists(string id)
        {
            return _context.ProgramsInformation.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
