using BackendProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]
public class MenuController : Controller
{
    private readonly AppDbContext _context;

    public MenuController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var menus = await _context.Menus.Include(p => p.MenuCategory).Where(p => !p.IsDeleted).ToListAsync();
        return View(menus);
    }

    public IActionResult Create()
    {
        ViewBag.MenuCategories = _context.MenuCategories.AsEnumerable();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuViewModel menuViewModel)
    {
        ViewBag.MenuCategories = _context.MenuCategories.AsEnumerable();

        if (!ModelState.IsValid)
            return View();

        if (!_context.MenuCategories.Any(c => c.Id == menuViewModel.MenuCategoryId))
            return BadRequest();



        Menu menu = new()
        {
            Name = menuViewModel.Name,
            Price = menuViewModel.Price,
            Image = menuViewModel.Image,
            MenuCategoryId = menuViewModel.MenuCategoryId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false

        };
        await _context.Menus.AddAsync(menu);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Detail(int id)
    {
        Menu? menu = _context.Menus.AsNoTracking().FirstOrDefault(s => s.Id == id);
        if (menu is null)
            return NotFound();


        return View(menu);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var foundMenu = await _context.Menus.Include(p => p.MenuCategory).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (foundMenu == null) return NotFound();

        return View(foundMenu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeletePost(int id)
    {
        var menu = await _context.Menus.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (menu == null) return NotFound();

        menu.IsDeleted = true;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Menu? menu = await _context.Menus.FirstOrDefaultAsync(p => p.Id == id);
        if (menu is null)
            return NotFound();

        ViewBag.MenuCategories = _context.MenuCategories.Where(c => !c.IsDeleted);

        MenuViewModel menuViewModel = new()
        {
            Name = menu.Name,
            Image = menu.Image,
            Price = menu.Price,
            MenuCategoryId = menu.MenuCategoryId,
        };
        return View(menuViewModel);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, MenuViewModel menuViewModel)
    {
        ViewBag.MenuCategories = _context.MenuCategories.Where(c => !c.IsDeleted);

        if (!ModelState.IsValid)
            return View();

        if (!_context.MenuCategories.Any(c => c.Id == menuViewModel.MenuCategoryId))
            return BadRequest();

        Menu? menu = await _context.Menus.FirstOrDefaultAsync(p => p.Id == id);
        if (menu is null)
            return NotFound();

        menu.Name = menuViewModel.Name;
        menu.Price = menuViewModel.Price;
        menu.MenuCategoryId = menuViewModel.MenuCategoryId;
        menu.Image = menuViewModel.Image;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
