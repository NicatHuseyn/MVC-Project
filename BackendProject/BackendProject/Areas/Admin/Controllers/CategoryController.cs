using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            var category = new Category
            {
                Name = categoryViewModel.Name,
                IsDeleted = categoryViewModel.IsDeleted
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category? category =_context.Categories.FirstOrDefault(x=>x.Id == id);
            if (category is null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x=>x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryViewModel.Id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryViewModel.Name;
            category.IsDeleted = categoryViewModel.IsDeleted;

            _context.Categories.Update(category);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.Categories.Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (category is null)
                return NotFound();

            return View(category);
        }
    }
}
