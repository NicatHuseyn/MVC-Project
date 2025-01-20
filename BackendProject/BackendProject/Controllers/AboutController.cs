using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
