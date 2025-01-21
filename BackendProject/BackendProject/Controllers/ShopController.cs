using BackendProject.ViewModels.BasketViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{
    public class ShopController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            IQueryable<Product> products = _context.Products.AsQueryable();

            ViewBag.ProductsCount = await products.CountAsync();

            ShopViewModel shopViewModel = new()
            {
                Products = categoryId != null
                ? await products.Where(p => p.CategoryId == categoryId).ToListAsync()
                : await products.ToListAsync(),
                Categories = await _context.Categories.Include(c => c.Products).Where(p => !p.IsDeleted).ToListAsync()
            };

            ViewBag.ProductsCount = _context.Products.Count();

            return View(shopViewModel);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = null;

            if (search != null && search != "")
            {
                products = await _context.Products
                  .Where(m => m.Name.ToLower().Contains(search.ToLower()))
                  .ToListAsync();
            }
            else
            {
                products = await _context.Products.ToListAsync();
            }

            ShopViewModel model = new()
            {
                Products = products
            };

            return PartialView("_ProductsPartial", model);
        }

        public async Task<IActionResult> Filtercategory(int id)
        {
            List<Product> products = null;

            if (id != 0)
            {
                products = await _context.Products.
                    Where(m => m.CategoryId == id)
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products.ToListAsync();
            }

            ShopViewModel model = new()
            {
                Products = products
            };

            return PartialView("_ProductsPartial", model);
        }

        public IActionResult Detail()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            AppUser user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();
            var basket = await _context.Baskets
               .Include(m => m.BasketProducts)
               .ThenInclude(m => m.Product)
               //.Include(m => m.BasketProducts)
               //.ThenInclude(m => m.Product)
               .FirstOrDefaultAsync(m => m.AppUserId == user.Id);

            BasketListVM model = new();

            if (basket == null) return View(model);

            foreach (var dbBasketProduct in basket.BasketProducts)
            {
                BasketProductVM basketProduct = new()
                {
                    Id = dbBasketProduct.Id,
                    ProductId = dbBasketProduct.ProductId,
                    Name = dbBasketProduct.Product.Name,
                    Image = dbBasketProduct.Product.Image,
                    Quantity = dbBasketProduct.Quantity,
                    Price = dbBasketProduct.Product.Price,
                    Total = (dbBasketProduct.Product.Price * dbBasketProduct.Quantity),
                };

                model.BasketProducts.Add(basketProduct);
            }

            return View(model);
        }

        public async Task<IActionResult> Checkout()
        {
            AppUser user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();
            var basket = await _context.Baskets
               .Include(m => m.BasketProducts)
               .ThenInclude(m => m.Product)
               .Include(m => m.BasketProducts)
               .ThenInclude(m => m.Product)
               .FirstOrDefaultAsync(m => m.AppUserId == user.Id);

            double totalPrice = 0;

            foreach (var dbBasket in basket.BasketProducts)
            {
                totalPrice += (dbBasket.Product.Price * dbBasket.Quantity);
            }
            ViewBag.TotalPrice = totalPrice;
            return View();
        }
    }
}
