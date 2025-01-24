using BackendProject.Areas.Admin.ViewModels;
using BackendProject.Models;
using BackendProject.Utils;
using BackendProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            ViewBag.Count = sliders.Count;
            return View(sliders);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderViewModel sliderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderViewModel);
            }

            if (sliderViewModel.UserImage == null || sliderViewModel.UserImage.Length == 0)
            {
                ModelState.AddModelError("Image", "Zəhmət olmasa bir şəkil yükləyin");
                return View(sliderViewModel);
            }

            if (!sliderViewModel.UserImage.CheckFileType("image"))
            {
                ModelState.AddModelError("Image", "Yüklənən bir fayl olmalıdır.");
                return View(sliderViewModel);
            }

            string fileName = $"{Guid.NewGuid()}-{sliderViewModel.UserImage.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Admin", "images", fileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await sliderViewModel.UserImage.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", $"Fayl yüklənmədi: {ex.Message}");
                return View(sliderViewModel);
            }


            Slider slider = new Slider()
            {
                Description = sliderViewModel.Description,
                Designation = sliderViewModel.Designation,
                UserImage = fileName,
                UserName = sliderViewModel.UserName,
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Detail(int id)
        {
            Slider? slider = _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();


            return View(slider);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (_context.Sliders.Count() == 1)
                return BadRequest();

            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }


        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSlider(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            SliderViewModel sliderViewModel = new()
            {
                Id = slider.Id,
                Description = slider.Description,
                Designation = slider.Designation,
                UserName = slider.UserName,
            };

            return View(sliderViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(SliderViewModel sliderViewModel, int id)
        {

            if (!ModelState.IsValid)
                return View(sliderViewModel);

            Slider? slider = _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            if (sliderViewModel.UserImage != null)
            {
                if (!sliderViewModel.UserImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                    return View(sliderViewModel);
                }
                if (!sliderViewModel.UserImage.CheckFileType(ContentType.image.ToString()))
                {
                    ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                    return View(sliderViewModel);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(slider.UserImage))
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Admin", "images", slider.UserImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        FileService.DeleteFile(oldPath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}-{sliderViewModel.UserImage.FileName}";
                var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await sliderViewModel.UserImage.CopyToAsync(stream);
                }

                slider.UserImage = fileName;
            }

            slider.UserName = sliderViewModel.UserName;
            slider.Description = sliderViewModel.Description;
            slider.Designation = sliderViewModel.Designation;
            

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
