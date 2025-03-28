using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;
using DC3Safe.Services.DataTable;

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
        public IActionResult Index()
        {
            return View();
        }

        // GET: Occupations
        public async Task<IActionResult> List([FromQuery] DataTableRequest request)
        {
            IQueryable<Occupation> query = _context.Occupations;
            int total = await query.CountAsync();
            query = FilterOccupations(query, request);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToDataTableAsync(request, total);

            return Json(data);
        }

        private IQueryable<Occupation> FilterOccupations(IQueryable<Occupation> query, DataTableRequest request)
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
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }
            return query;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchDelete([FromBody] string[] ids)
        {
            if (ids.Length == 0) return NotFound();
            var occupations = await _context.Occupations
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            _context.Occupations.RemoveRange(occupations);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool OccupationExists(string id)
        {
            return _context.Occupations.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
