using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Utils;
using BackendProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]
public class MenuController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MenuController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
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

        if (!_context.MenuCategories.Any(mc => mc.Id == menuViewModel.MenuCategoryId))
            return BadRequest("Invalid MenuCategoryId");

        if (!menuViewModel.Image.CheckFileType(ContentType.image.ToString()))
        {
            ModelState.AddModelError("Image", "File must be an image.");
            return View();
        }

        string fileName = $"{Guid.NewGuid()}-{menuViewModel.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await menuViewModel.Image.CopyToAsync(stream);
        }

        Menu menu = new()
        {
            Name = menuViewModel.Name,
            Price = menuViewModel.Price,
            Image = fileName,
            MenuCategoryId = menuViewModel.MenuCategoryId,
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

    public IActionResult Update(int id)
    {
        Menu menu = _context.Menus.FirstOrDefault(m => m.Id == id && !m.IsDeleted);
        if (menu == null) return NotFound();

        MenuViewModel menuViewModel = new()
        {
            Name = menu.Name,
            Price = menu.Price,
            MenuCategoryId = menu.MenuCategoryId,
        };

        ViewBag.MenuCategories = _context.MenuCategories.AsEnumerable();
        return View(menuViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, MenuViewModel menuViewModel)
    {
        ViewBag.MenuCategories = _context.MenuCategories.AsEnumerable();

        if (!ModelState.IsValid)
            return View(menuViewModel);

        Menu menu = _context.Menus.FirstOrDefault(m => m.Id == id && !m.IsDeleted);
        if (menu == null) return NotFound();

        if (!_context.MenuCategories.Any(mc => mc.Id == menuViewModel.MenuCategoryId))
            return BadRequest("Invalid MenuCategoryId");

        if (menuViewModel.Image != null)
        {
            if (!menuViewModel.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "File must be an image.");
                return View(menuViewModel);
            }

            string fileName = $"{Guid.NewGuid()}-{menuViewModel.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await menuViewModel.Image.CopyToAsync(stream);
            }

            // Delete old image
            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", menu.Image);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            menu.Image = fileName;
        }

        menu.Name = menuViewModel.Name;
        menu.Price = menuViewModel.Price;
        menu.MenuCategoryId = menuViewModel.MenuCategoryId;

        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}
