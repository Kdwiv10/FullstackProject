using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FullstackProject.Model;

namespace FullstackProject.Controllers
{
    public class GameLibrariesController : Controller
    {
        private readonly S22024Group2ProjectContext _context;

        public GameLibrariesController(S22024Group2ProjectContext context)
        {
            _context = context;
        }

        // GET: GameLibraries
        public async Task<IActionResult> Index()
        {
            var s22024Group2ProjectContext = _context.GameLibraries.Include(g => g.Game).Include(g => g.User);
            return View(await s22024Group2ProjectContext.ToListAsync());
        }

        // GET: GameLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameLibrary = await _context.GameLibraries
                .Include(g => g.Game)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.LibraryId == id);
            if (gameLibrary == null)
            {
                return NotFound();
            }

            return View(gameLibrary);
        }

        // GET: GameLibraries/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: GameLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryId,UserId,GameId,DateAdded,PricePaid")] GameLibrary gameLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameLibrary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameLibrary.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", gameLibrary.UserId);
            return View(gameLibrary);
        }

        // GET: GameLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameLibrary = await _context.GameLibraries.FindAsync(id);
            if (gameLibrary == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameLibrary.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", gameLibrary.UserId);
            return View(gameLibrary);
        }

        // POST: GameLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibraryId,UserId,GameId,DateAdded,PricePaid")] GameLibrary gameLibrary)
        {
            if (id != gameLibrary.LibraryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameLibraryExists(gameLibrary.LibraryId))
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
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameLibrary.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", gameLibrary.UserId);
            return View(gameLibrary);
        }

        // GET: GameLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameLibrary = await _context.GameLibraries
                .Include(g => g.Game)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.LibraryId == id);
            if (gameLibrary == null)
            {
                return NotFound();
            }

            return View(gameLibrary);
        }

        // POST: GameLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameLibrary = await _context.GameLibraries.FindAsync(id);
            if (gameLibrary != null)
            {
                _context.GameLibraries.Remove(gameLibrary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameLibraryExists(int id)
        {
            return _context.GameLibraries.Any(e => e.LibraryId == id);
        }
    }
}
