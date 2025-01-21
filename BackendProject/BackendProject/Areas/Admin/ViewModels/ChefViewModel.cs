using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels;

public class ChefViewModel
{
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }

	public IFormFile Image { get; set; }

}
