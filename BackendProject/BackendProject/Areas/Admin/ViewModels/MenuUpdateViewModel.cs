using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels
{
    public class MenuUpdateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }
        public string ExistingImagePath { get; set; }
        public int MenuCategoryId { get; set; }
    }
}
