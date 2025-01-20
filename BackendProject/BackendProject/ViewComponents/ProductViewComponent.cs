using Microsoft.EntityFrameworkCore;

namespace BackendProject.ViewComponents;

public class ProductViewComponent:ViewComponent
{
    private readonly AppDbContext _context;

    public ProductViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _context.Products.Take(4).ToListAsync();
        ViewBag.ProductsCount = _context.Products.Count();

        return View(products);
    }
}
