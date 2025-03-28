using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;
using DC3Safe.Services.DataTable;

namespace DC3Safe.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public IActionResult Index()
        {
            return View();
        }

        // GET: Trainers
        public async Task<IActionResult> List([FromQuery] DataTableRequest request)
        {
            IQueryable<Trainer> query = _context.Trainers;
            int total = await query.CountAsync();
            query = FilterTrainers(query, request);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToDataTableAsync(request, total);

            return Json(data);
        }

        private IQueryable<Trainer> FilterTrainers(IQueryable<Trainer> query, DataTableRequest request)
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

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchDelete([FromBody] string[] ids)
        {
            if (ids.Length == 0) return NotFound();
            var trainers = await _context.Trainers
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            _context.Trainers.RemoveRange(trainers);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool TrainerExists(string id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
