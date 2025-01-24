using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Models;
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
            return BadRequest("MenuCategory tapılmadı");

        if (!menuViewModel.Image.CheckFileType(ContentType.image.ToString()))
        {
            ModelState.AddModelError("Image", "Yüklənən bir fayl olmalıdır.");
            return View();
        }

        if (menuViewModel.Image == null || menuViewModel.Image.Length == 0)
        {
            ModelState.AddModelError("Image", "Zəhmət olmasa bir şəkil yükləyin");
            return View(menuViewModel);
        }

        string fileName = $"{Guid.NewGuid()}-{menuViewModel.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);

        try
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await menuViewModel.Image.CopyToAsync(stream);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Image", $"Fayl yüklənmədi: {ex.Message}");
            return View(menuViewModel);
        }

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

    public async Task<IActionResult> Update(int id)
    {
        Menu? menu = await _context.Menus.FirstOrDefaultAsync(p => p.Id == id);
        if (menu is null)
            return NotFound();

        ViewBag.MenuCategories = _context.MenuCategories.Where(c => !c.IsDeleted);

        MenuViewModel menuViewModel = new()
        {
            Id = menu.Id,
            Name = menu.Name,
            Price = menu.Price,
            MenuCategoryId = menu.MenuCategoryId
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

        if (menuViewModel.Image != null)
        {
            if (!menuViewModel.Image.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 500 kb-dan kicik olmalidir.");
                return View(menuViewModel);
            }
            if (!menuViewModel.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                return View(menuViewModel);
            }

            // Delete old image
            if (!string.IsNullOrEmpty(menu.Image))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Admin", "images", menu.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    FileService.DeleteFile(oldPath);
                }
            }

            string fileName = $"{Guid.NewGuid()}-{menuViewModel.Image.FileName}";
            var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await menuViewModel.Image.CopyToAsync(stream);
            }

            menu.Image = fileName;
        }

        menu.Name = menuViewModel.Name;
        menu.Price = menuViewModel.Price;
        menu.MenuCategoryId = menuViewModel.MenuCategoryId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}
