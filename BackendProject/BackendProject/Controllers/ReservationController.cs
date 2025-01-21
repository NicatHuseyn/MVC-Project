using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
