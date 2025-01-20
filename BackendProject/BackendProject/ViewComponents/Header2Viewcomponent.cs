using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.ViewComponents;

public class Header2Viewcomponent:ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Header2Viewcomponent(AppDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> GetUserBasketProductsCount(ClaimsPrincipal userClaims)
    {
        var user = await _userManager.GetUserAsync(userClaims);
        if (user == null) return 0;
        var basketProductCount = await _context.BasketProducts.Where(m => m.Basket.AppUserId == user.Id).SumAsync(m => m.Quantity);
        return basketProductCount;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

        HeaderVM model = new()
        {
            Settings = settings,
            Count = await GetUserBasketProductsCount(_httpContextAccessor.HttpContext.User)
        };

        return await Task.FromResult(View(model));
    }
}
