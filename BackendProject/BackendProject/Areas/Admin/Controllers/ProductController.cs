using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Models;
using BackendProject.Utils;
using BackendProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category)
           .OrderByDescending(p => p.ModifiedAt).ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();

            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
            {
                return BadRequest("Category tapılmadı");
            }

            if (productViewModel.Image == null || productViewModel.Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Zəhmət olmasa bir şəkil yükləyin");
                return View(productViewModel);
            }

            if (!productViewModel.Image.CheckFileType("image"))
            {
                ModelState.AddModelError("Image", "Yüklənən bir fayl olmalıdır.");
                return View(productViewModel);
            }

            string fileName = $"{Guid.NewGuid()}-{productViewModel.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await productViewModel.Image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", $"Fayl yüklənmədi: {ex.Message}");
                return View(productViewModel);
            }

            Product product = new()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Rating = productViewModel.Rating,
                Image = fileName,
                CategoryId = productViewModel.CategoryId,
                IsDeleted = false
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
                return NotFound();

            ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);

            ProductViewModel productViewModel = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Rating = product.Rating,
                CategoryId = product.CategoryId,
            };

            return View(productViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductViewModel productViewModel)
        {
            ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);

            if (!ModelState.IsValid)
                return View();

            if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
                return BadRequest();

            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
                return NotFound();

            if (productViewModel.Image != null)
            {
                if (!productViewModel.Image.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                    return View(productViewModel);
                }
                if (!productViewModel.Image.CheckFileType(ContentType.image.ToString()))
                {
                    ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                    return View(productViewModel);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(product.Image))
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Admin", "images", product.Image);
                    if (System.IO.File.Exists(oldPath))
                    {
                        FileService.DeleteFile(oldPath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}-{productViewModel.Image.FileName}";
                var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await productViewModel.Image.CopyToAsync(stream);
                }

                product.Image = fileName;
            }

            product.Name = productViewModel.Name;
            product.Price = productViewModel.Price;
            product.Rating = productViewModel.Rating;
            product.CategoryId = productViewModel.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var foundProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (foundProduct == null) return NotFound();

            return View(foundProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeletePost(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null) return NotFound();


            FileService.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "images", product.Image);

            product.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Detail(int id)
        {
            Product? product = _context.Products.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (product is null)
                return NotFound();


            return View(product);
        }
    }
}
