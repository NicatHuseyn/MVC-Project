using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
