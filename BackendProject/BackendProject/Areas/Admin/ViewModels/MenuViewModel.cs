using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels;

public class MenuViewModel
{

    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string Image { get; set; }
    public int MenuCategoryId { get; set; }
}
