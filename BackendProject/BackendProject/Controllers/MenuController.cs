using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? menuCategoyId)
        {
            IQueryable<Menu> menus = _context.Menus.AsQueryable();
            ViewBag.MenusCounts = await menus.CountAsync();

            MenuPageViewModels menuPageViewModels = new()
            {
                Menus = menuCategoyId != null
                ? await _context.Menus.Where(p => !p.IsDeleted && p.MenuCategoryId == menuCategoyId).ToListAsync()
                : await _context.Menus.Where(p => !p.IsDeleted).ToListAsync(),
                MenuCategories = await _context.MenuCategories.Include(c => c.Menus.
                Where(p => !p.IsDeleted)).Where(p => !p.IsDeleted).ToListAsync(),
            };
            ViewBag.MenusCounts = _context.Menus.Count();

            return View(menuPageViewModels);
        }

        public async Task<IActionResult> Filtercategory(int id)
        {
            List<Menu> menus = null;

            if (id != 0)
            {
                menus = await _context.Menus.
                    Where(m => m.MenuCategoryId == id)
                    .ToListAsync();
            }
            else
            {
                menus = await _context.Menus.ToListAsync();
            }

            MenuPageViewModels model = new()
            {
                Menus = menus
            };

            return PartialView("_ProductsPartial", model);
        }
    }
}
