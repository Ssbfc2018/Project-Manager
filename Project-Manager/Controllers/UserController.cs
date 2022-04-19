using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Manager.Data;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext DB, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _db = DB;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult AddRoleToUser()
        {
            ViewBag.users = new SelectList(_db.Users, "Id", "Email");
            ViewBag.roles = new SelectList(_db.Roles, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(string? userId, string? roleId)
        {
            try
            {
                if (userId != null && roleId != null)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    IdentityRole role = await _roleManager.FindByIdAsync(roleId);

                    if (user != null && role != null)
                    {
                        bool userAlreadyInRole = await _userManager.IsInRoleAsync(user, role.Name);

                        if (userAlreadyInRole)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                            _db.SaveChanges();

                            ViewBag.SuccessMessage = $"Assigned {user.Email} as {role.Name}";
                        }
                        else
                        {
                            ViewBag.SuccessMessage = $"{user.Email} is already in {role.Name}";
                        }

                        ViewBag.users = new SelectList(_db.Users, "Id", "Email");
                        ViewBag.roles = new SelectList(_db.Roles, "Id", "Name");

                        return View();
                    }
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        public IActionResult CheckUserAlreadyInRole()
        {
            ViewBag.users = new SelectList(_db.Users, "Id", "Email");
            ViewBag.roles = new SelectList(_db.Roles, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckUserAlreadyInRole(string? userId, string? roleId)
        {
            try
            {
                if (userId != null && roleId != null)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    IdentityRole role = await _roleManager.FindByIdAsync(roleId);

                    if (user != null && role != null)
                    {
                        bool userAlreadyInRole = await _userManager.IsInRoleAsync(user, role.Name);

                        if (userAlreadyInRole)
                        {
                            ViewBag.SuccessMessage = $"{user.Email} is already in {role.Name}";
                        }
                        else
                        {
                            ViewBag.SuccessMessage = $"{user.Email} is not in {role.Name}";
                        }

                        ViewBag.users = new SelectList(_db.Users, "Id", "Email");
                        ViewBag.roles = new SelectList(_db.Roles, "Id", "Name");

                        return View();
                    }
                }

                return NotFound();

            }catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }

            return View();
        }

        public IActionResult GetAllRolesForUser()
        {
            ViewBag.users = new SelectList(_db.Users, "Id", "Email");
            ViewBag.role = new List<string>() { };

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetAllRolesForUser(string? userId)
        {
            try
            {
                if (userId != null)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    ViewBag.userName=user.UserName;
                    List<IdentityRole> roles=_db.Roles.ToList();
                    ViewBag.roles = new List<string>();
                    if (user != null)
                    {
                        foreach(IdentityRole ir in roles)
                        {
                            if(await _userManager.IsInRoleAsync(user, ir.Name)){
                                ViewBag.roles.Add(ir.Name);
                            }
                        }
                    }

                    ViewBag.users = new SelectList(_db.Users, "Id", "Email");

                    return View();
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }
    }
}
