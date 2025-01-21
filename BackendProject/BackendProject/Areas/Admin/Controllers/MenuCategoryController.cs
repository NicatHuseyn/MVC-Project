using BackendProject.Areas.Admin.ViewModels;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            MenuCategory category = new()
            {
                Name = viewModel.Name,
                IsDeleted = false
            };

            await _context.MenuCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            MenuCategoryViewModel viewModel = new()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MenuCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var category = await _context.MenuCategories.FindAsync(viewModel.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = viewModel.Name;

            _context.MenuCategories.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;

            _context.MenuCategories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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
