using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;

        public ChefController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Chef> chefs = _context.Chefs
                .Where(m => !m.IsDeleted)
                .Include(m => m.SocialMedias)
                .ToList();

            return View(chefs);
        }
    }
}
