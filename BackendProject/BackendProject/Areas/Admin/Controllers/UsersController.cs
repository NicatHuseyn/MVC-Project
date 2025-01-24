using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class UsersController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var userName = HttpContext?.User?.Identity?.Name;

            //var users = await _userManager.Users.Where(u => u.UserName != userName).ToListAsync();

            var users = await _userManager.Users.ToListAsync();

            List<AlluserViewModel> allusers = new List<AlluserViewModel>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                allusers.Add(new AlluserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Role = userRoles.FirstOrDefault()
                });
            }

            return View(allusers);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();

            if (userRole == RoleType.Admin.ToString())
                return BadRequest();

            UserViewModel userViewModel = new()
            {
                Role = userRole
            };

            ViewBag.Roles = _roleManager.Roles.ToList();

            return View(userViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, UserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();

            if (userRole == RoleType.Admin.ToString())
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, userRole);

            await _userManager.AddToRoleAsync(user, userViewModel.Role);

            return RedirectToAction(nameof(Index));
        }
    }
}
