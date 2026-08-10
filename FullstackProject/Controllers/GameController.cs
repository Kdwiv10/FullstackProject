using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FullStack_Shangri.Models;
using Microsoft.AspNetCore.Authorization;// 27/10/2024 Karman Dwivedi: Added for Authorize attribute
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace FullStack_Shangri.Controllers
{
    public class GameController : Controller
    {
        private readonly S22024Group2ProjectContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public GameController(S22024Group2ProjectContext context, IWebHostEnvironment webHostEnvironment, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var u = _userManager.Users.Where(u => u.NormalizedEmail == "ADMIN@EXAMPLE.COM").FirstOrDefault();
            _userManager.AddToRoleAsync(u, "Admin");

            var genres = await _context.Genres
                .Include(g => g.Games)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                genres.ForEach(g =>
                {
                    g.Games = g.Games
                        .Where(game => game.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                       game.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                });
            }

            return View(genres);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // Create action - Only accessible by Admins
        [Authorize(Roles = "Admin")] // 27/10/2024 Karman Dwivedi: Restricted Create action to Admin role
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // 27/10/2024 Karman Dwivedi: Restricted Create action to Admin role
        public async Task<IActionResult> Create([Bind("GameId,Title,Description,ReleaseDate,Price,DeveloperId,Stock,ImageFileName,GenreId")] Game game, IFormFile imageFileName)
        {
            if (ModelState.IsValid)
            {
                if (imageFileName != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string uniqueName = Guid.NewGuid().ToString() + "_" + imageFileName.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFileName.CopyToAsync(fileStream);
                    }
                    game.ImageFileName = uniqueName;
                }

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "Name", game.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", game.GenreId);
            return View(game);
        }

        // Edit action - Only accessible by Admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "Name", game.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", game.GenreId);
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Title,Description,ReleaseDate,Price,DeveloperId,Stock,ImageFileName,GenreId")] Game game, IFormFile imageFileName)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (imageFileName != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string uniqueName = Guid.NewGuid().ToString() + "_" + imageFileName.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFileName.CopyToAsync(fileStream);
                    }
                    game.ImageFileName = uniqueName;
                }

                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "Name", game.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", game.GenreId);
            return View(game);
        }

        // Delete action - Only accessible by Admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
        public async Task<IActionResult> TopTen()
        {
            // Retrieve the top 10 games sorted by price 23/11/24 Karman Dwivedi
            var topTenGames = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Genre)
                .OrderByDescending(g => g.Price) 
                .Take(10)
                .ToListAsync();

            return View(topTenGames); 
        }

    }
}
