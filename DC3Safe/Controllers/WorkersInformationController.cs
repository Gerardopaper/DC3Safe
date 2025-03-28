using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;
using DC3Safe.Services.DataTable;

namespace DC3Safe.Controllers
{
    public class WorkersInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkersInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkersInformation
        public IActionResult Index()
        {
            return View();
        }

        // GET: WorkersInformation
        public async Task<IActionResult> List([FromQuery] DataTableRequest request)
        {
            IQueryable<WorkerInformation> query = _context.WorkersInformation;
            int total = await query.CountAsync();
            query = FilterWorkersInformation(query, request);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    first_name = x.FirstName,
                    last_name = x.LastName,
                    last_name2 = x.LastName2,
                    curp = x.Curp,
                    occupation = x.Occupation!.Name,
                    position = x.Position
                })
                .ToDataTableAsync(request, total);

            return Json(data);
        }

        private IQueryable<WorkerInformation> FilterWorkersInformation(IQueryable<WorkerInformation> query, DataTableRequest request)
        {
            if (!string.IsNullOrEmpty(request.search))
            {
                string trimmedSearch = request.search.Trim();
                query = query
                    .Where(x => EF.Functions.Like(x.FirstName, $"%{trimmedSearch}%")
                    || EF.Functions.Like(x.LastName, $"%{trimmedSearch}%")
                    || EF.Functions.Like(x.LastName2, $"%{trimmedSearch}%")
                    || EF.Functions.Like(x.Curp, $"%{trimmedSearch}%")
                    || EF.Functions.Like(x.Position, $"%{trimmedSearch}%")
                    || EF.Functions.Like(x.Occupation!.Name, $"%{trimmedSearch}%"));
            }

            switch ((request.column, request.dir))
            {
                case (1, "asc"):
                    query = query.OrderBy(x => x.LastName);
                    break;
                case (1, "desc"):
                    query = query.OrderByDescending(x => x.LastName);
                    break;
                case (2, "asc"):
                    query = query.OrderBy(x => x.LastName2);
                    break;
                case (2, "desc"):
                    query = query.OrderByDescending(x => x.LastName2);
                    break;
                case (3, "asc"):
                    query = query.OrderBy(x => x.FirstName);
                    break;
                case (3, "desc"):
                    query = query.OrderByDescending(x => x.FirstName);
                    break;
                case (4, "asc"):
                    query = query.OrderBy(x => x.Curp);
                    break;
                case (4, "desc"):
                    query = query.OrderByDescending(x => x.Curp);
                    break;
                case (5, "asc"):
                    query = query.OrderBy(x => x.Occupation!.Name);
                    break;
                case (5, "desc"):
                    query = query.OrderByDescending(x => x.Occupation!.Name);
                    break;
                case (6, "asc"):
                    query = query.OrderBy(x => x.Position);
                    break;
                case (6, "desc"):
                    query = query.OrderByDescending(x => x.Position);
                    break;
                default:
                    query = query.OrderBy(x => x.LastName)
                        .ThenBy(x => x.LastName2)
                        .ThenBy(x => x.FirstName);
                    break;
            }
            return query;
        }

        // GET: WorkersInformation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerInformation = await _context.WorkersInformation
                .Include(w => w.Occupation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerInformation == null)
            {
                return NotFound();
            }

            return View(workerInformation);
        }

        // GET: WorkersInformation/Create
        public IActionResult Create()
        {
            ViewData["OccupationId"] = new SelectList(_context.Occupations, "Id", "Name");
            return View();
        }

        // POST: WorkersInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,LastName2,Curp,OccupationId,Position")] WorkerInformation workerInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workerInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OccupationId"] = new SelectList(_context.Occupations, "Id", "Name", workerInformation.OccupationId);
            return View(workerInformation);
        }

        // GET: WorkersInformation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerInformation = await _context.WorkersInformation.FindAsync(id);
            if (workerInformation == null)
            {
                return NotFound();
            }
            ViewData["OccupationId"] = new SelectList(_context.Occupations, "Id", "Name", workerInformation.OccupationId);
            return View(workerInformation);
        }

        // POST: WorkersInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,LastName2,Curp,OccupationId,Position")] WorkerInformation workerInformation)
        {
            if (id != workerInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workerInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerInformationExists(workerInformation.Id))
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
            ViewData["OccupationId"] = new SelectList(_context.Occupations, "Id", "Name", workerInformation.OccupationId);
            return View(workerInformation);
        }

        // GET: WorkersInformation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerInformation = await _context.WorkersInformation
                .Include(w => w.Occupation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerInformation == null)
            {
                return NotFound();
            }

            return View(workerInformation);
        }

        // POST: WorkersInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var workerInformation = await _context.WorkersInformation.FindAsync(id);
            if (workerInformation != null)
            {
                _context.WorkersInformation.Remove(workerInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchDelete([FromBody] string[] ids)
        {
            if (ids.Length == 0) return NotFound();
            var workersInformation = await _context.WorkersInformation
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            _context.WorkersInformation.RemoveRange(workersInformation);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool WorkerInformationExists(string id)
        {
            return _context.WorkersInformation.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
