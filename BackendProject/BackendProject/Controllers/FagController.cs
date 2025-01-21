using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    public class FagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
