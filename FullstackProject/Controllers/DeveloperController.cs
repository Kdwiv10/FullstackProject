using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStack_Shangri.Models;

namespace FullStack_Shangri.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly S22024Group2ProjectContext _context;

        public DeveloperController(S22024Group2ProjectContext context)
        {
            _context = context;
        }

        // GET: Developer
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalDevelopers = await _context.Developers.CountAsync();
            var developers = await _context.Developers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalDevelopers / pageSize);
            ViewData["CurrentPage"] = page;

            return View(developers);
        }

        // GET: Developer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                .FirstOrDefaultAsync(m => m.DeveloperId == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // GET: Developer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Developer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeveloperId,Name,Email,ContactInfo")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(developer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

        // GET: Developer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        // POST: Developer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeveloperId,Name,Email,ContactInfo")] Developer developer)
        {
            if (id != developer.DeveloperId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(developer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperExists(developer.DeveloperId))
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
            return View(developer);
        }

        // GET: Developer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                .FirstOrDefaultAsync(m => m.DeveloperId == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // POST: Developer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developer = await _context.Developers.FindAsync(id);
            if (developer != null)
            {
                _context.Developers.Remove(developer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperExists(int id)
        {
            return _context.Developers.Any(e => e.DeveloperId == id);
        }
    }
}
