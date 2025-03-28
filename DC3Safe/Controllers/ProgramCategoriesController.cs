using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;
using DC3Safe.Services.DataTable;

namespace DC3Safe.Controllers
{
    public class ProgramCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramCategories
        public IActionResult Index()
        {
            return View();
        }

        // GET: ProgramCategories
        public async Task<IActionResult> List([FromQuery] DataTableRequest request)
        {
            IQueryable<ProgramCategory> query = _context.ProgramCategories;
            int total = await query.CountAsync();
            query = FilterProgramCategories(query, request);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToDataTableAsync(request, total);

            return Json(data);
        }

        private IQueryable<ProgramCategory> FilterProgramCategories(IQueryable<ProgramCategory> query, DataTableRequest request)
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

        // GET: ProgramCategories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programCategory = await _context.ProgramCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programCategory == null)
            {
                return NotFound();
            }

            return View(programCategory);
        }

        // GET: ProgramCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgramCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProgramCategory programCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programCategory);
        }

        // GET: ProgramCategories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programCategory = await _context.ProgramCategories.FindAsync(id);
            if (programCategory == null)
            {
                return NotFound();
            }
            return View(programCategory);
        }

        // POST: ProgramCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] ProgramCategory programCategory)
        {
            if (id != programCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramCategoryExists(programCategory.Id))
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
            return View(programCategory);
        }

        // GET: ProgramCategories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programCategory = await _context.ProgramCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programCategory == null)
            {
                return NotFound();
            }

            return View(programCategory);
        }

        // POST: ProgramCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var programCategory = await _context.ProgramCategories.FindAsync(id);
            if (programCategory != null)
            {
                _context.ProgramCategories.Remove(programCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchDelete([FromBody] string[] ids)
        {
            if (ids.Length == 0) return NotFound();
            var programCategories = await _context.ProgramCategories
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            _context.ProgramCategories.RemoveRange(programCategories);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool ProgramCategoryExists(string id)
        {
            return _context.ProgramCategories.Any(e => e.Id == id);
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
