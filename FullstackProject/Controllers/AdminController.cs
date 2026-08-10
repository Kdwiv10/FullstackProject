using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStack_Shangri.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack_Shangri.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Index Action to display all roles
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // Create a new Role (GET)
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        // Create a new Role (POST)
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                ModelState.AddModelError("", "Role already exists");
                return View(role);
            }

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }

        // Assign Role (GET)
        [HttpGet]
        public async Task<IActionResult> AssignRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles,
                SelectedRoleIds = userRoles.ToList()
            };

            return View(model);
        }

        // Assign Role (POST)
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Remove current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // Assign new roles
            var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoleIds);
            if (addResult.Succeeded)
            {
                return RedirectToAction("Users");
            }

            foreach (var error in addResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // Display list of users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<AssignRoleViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.ToList();
                model.Add(new AssignRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles,
                    SelectedRoleIds = userRoles.ToList()
                });
            }

            return View(model);
        }

        // Display the "Create User" form (GET)
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        // Process the form submission to create a new user (POST)
        [HttpPost]
        public async Task<IActionResult> CreateUser(string userName, string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = userName,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;

                    var updateResult = await _userManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
    }
}
