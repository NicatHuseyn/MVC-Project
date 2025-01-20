using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.ViewComponents;

public class HeaderViewComponent:ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpContextAccessor _httpContextAccessor;
    public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager, HttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> GetUserBasketProductsCount(ClaimsPrincipal userClaims)
    {
        var user = await _userManager.GetUserAsync(userClaims);
        if (user == null) return 0;

        var basketProductCount = await _context.BasketProducts.Where(m=>m.Basket.AppUserId == user.Id).SumAsync(m=>m.Quantity);

        return basketProductCount;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return Content("HttpContext is not available.");
        }

        var user = httpContext.User;

        Dictionary<string,string> settings = await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);

        HeaderVM model = new()
        {
            Settings = settings,
            Count = await GetUserBasketProductsCount(user)
        };

        return View(model);
    }
}
