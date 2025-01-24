using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Models;
using BackendProject.Utils;
using BackendProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private int _count = 0;

        public ServiceController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();
            _count = services.Count();
        }

        public IActionResult Index()
        {
            IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();


            ViewBag.Count = _count;

            return View(services);
        }

        public IActionResult Create()
        {
            if (_count == 3)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceViewModel)
        {

            if (_count == 3)
                return BadRequest();

            if (!ModelState.IsValid) return View();

            if (serviceViewModel.Image == null)
            {
                ModelState.AddModelError("Image", "Image bos ola bilmez");
                return View();
            }
            //if (!serviceViewModel.Image.CheckFileSize(100))
            //{
            //	ModelState.AddModelError("Image", "Faylin hecmi 100kb dan kicik olmalidir");
            //	return View();
            //}
            if (!serviceViewModel.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi sekil olmalidir");
                return View();

            }
            if (await _appDbContext.Services.AnyAsync(s => s.Title == serviceViewModel.Title))
            {
                ModelState.AddModelError("Name", "Hal hazirda bu adda bir service movcuddur");
                return View();
            }

            string fileName = $"{Guid.NewGuid()}-{serviceViewModel.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await serviceViewModel.Image.CopyToAsync(stream);
            }

            Service service = new()
            {
                Title = serviceViewModel.Title,
                Description = serviceViewModel.Description,
                Number = serviceViewModel.Number,
                Image = fileName
            };

            await _appDbContext.Services.AddAsync(service);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            ServiceViewModel serviceViewModel = new()
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Number = service.Number

            };
            return View(serviceViewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ServiceViewModel serviceViewModel)
        {
            if (!ModelState.IsValid)
                return View();
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            if (serviceViewModel.Image != null)
            {
                if (!serviceViewModel.Image.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Faylin hecmi 500 kb-dan kicik olmalidir.");
                    return View(serviceViewModel);
                }
                if (!serviceViewModel.Image.CheckFileType(ContentType.image.ToString()))
                {
                    ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                    return View(serviceViewModel);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(service.Image))
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Admin", "images", service.Image);
                    if (System.IO.File.Exists(oldPath))
                    {
                        FileService.DeleteFile(oldPath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}-{serviceViewModel.Image.FileName}";
                var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await serviceViewModel.Image.CopyToAsync(stream);
                }

                service.Image = fileName;
            }

            service.Title = serviceViewModel.Title;
            service.Description = serviceViewModel.Description;
            service.Number = serviceViewModel.Number;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            FileService.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "images", service.Image);

            _appDbContext.Services.Remove(service);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Detail(int id)
        {

            Service? service = _appDbContext.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (service is null)
                return NotFound();
            return View(service);
        }
    }
}
