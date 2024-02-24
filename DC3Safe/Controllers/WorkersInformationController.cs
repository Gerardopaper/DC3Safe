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
    public class WorkersInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkersInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkersInformation
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkersInformation.Include(w => w.Occupation);
            return View(await applicationDbContext.ToListAsync());
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

        private bool WorkerInformationExists(string id)
        {
            return _context.WorkersInformation.Any(e => e.Id == id);
        }
    }
}
