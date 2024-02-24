using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DC3Safe.Data;
using DC3Safe.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProgramCategories.ToListAsync());
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

        private bool ProgramCategoryExists(string id)
        {
            return _context.ProgramCategories.Any(e => e.Id == id);
        }
    }
}
