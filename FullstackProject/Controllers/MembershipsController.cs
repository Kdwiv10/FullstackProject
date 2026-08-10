using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FullStack_Shangri.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FullStack_Shangri.Controllers
{
    public class MembershipsController : Controller
    {
    private readonly S22024Group2ProjectContext _context;

    public MembershipsController(S22024Group2ProjectContext context)
    {
        _context = context;
    }

        // 27/10/2024 Kanemana Witana: Created and dsiplayed the Memberships

        // GET: Memberships
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var userMembership = await _context.UserMembership
                    .FirstOrDefaultAsync(um => um.UserId == userId);

                if (userMembership != null)
                {
                    var remainingDays = (userMembership.MembershipExpiryDate - DateTime.Now).Days;
                    ViewData["MembershipTimeLeft"] = remainingDays > 0 ? remainingDays : 0;
                }
                else
                {
                    ViewData["MembershipTimeLeft"] = "No active membership";
                }
            }

            return View(await _context.Memberships.ToListAsync());
        }

        // GET: Memberships/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Memberships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,Type,Price,Description,DiscountRate")] Memberships membership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Memberships/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

        // POST: Memberships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,Type,Price,Description,DiscountRate")] Memberships membership)
        {
            if (id != membership.MembershipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.MembershipId))
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
            return View(membership);
        }

        // GET: Memberships/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.MembershipId == id);
        }
    }
}
