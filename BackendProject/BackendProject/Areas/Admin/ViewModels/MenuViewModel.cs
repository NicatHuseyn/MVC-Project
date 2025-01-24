using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels;

public class MenuViewModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }
    public int MenuCategoryId { get; set; }

}
