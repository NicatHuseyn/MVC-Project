using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MenuCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public MenuCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menuCategories = await _context.MenuCategories.ToListAsync();

            return View(menuCategories);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var menuCategories = await _context.MenuCategories.Include(c => c.Menus)
        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (menuCategories is null)
                return NotFound();

            return View(menuCategories);
        }
    }
}
