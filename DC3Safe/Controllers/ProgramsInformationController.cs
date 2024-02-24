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
    public class ProgramsInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramsInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramInformations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProgramsInformation.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
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

        private bool ProgramInformationExists(string id)
        {
            return _context.ProgramsInformation.Any(e => e.Id == id);
        }
    }
}
