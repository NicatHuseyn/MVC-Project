using Microsoft.AspNetCore.Authorization;

namespace BackendProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator,Member")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
