using BackendProject.Contexts;
using BackendProject.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    public class ChefController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChefController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Chef> chefs = await _appDbContext.Chefs
                .Include(m => m.SocialMedias)
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            return View(chefs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Socials = await GetSocialsAsync();

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            Chef? chef = await _appDbContext.Chefs.FirstOrDefaultAsync(s => s.Id == id);
            if (chef is null)
                return NotFound();

            return View(chef);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletechef(int id)
        {
            Chef? chef = await _appDbContext.Chefs.FirstOrDefaultAsync(s => s.Id == id);
            if (chef is null)
                return NotFound();

            FileService.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "images", chef.Image);

            _appDbContext.Chefs.Remove(chef);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            Chef? chef = _appDbContext.Chefs.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (chef is null)
                return NotFound();


            return View(chef);
        }

        private async Task<SelectList> GetSocialsAsync()
        {
            IEnumerable<SocialMedia> socialMedias = await _appDbContext.SocialMedias
                .ToListAsync();

            return new SelectList(socialMedias, "Id", "Social");
        }

      
    }
}
