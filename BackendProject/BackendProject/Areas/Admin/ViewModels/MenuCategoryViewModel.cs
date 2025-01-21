using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels
{
    public class MenuCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You should be enter name")]
        [StringLength(100, ErrorMessage = "Enter max 100 char")]
        public string Name { get; set; }
    }
}
