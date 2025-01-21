using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels;

public class ServiceViewModel
{
	public int Id { get; set; }
	[Required]
	[MaxLength(100)]
	public string Title { get; set; }
	[MaxLength(200), Required]
	public string Description { get; set; }
	[Required]
	public IFormFile? Image { get; set; }
	public int Number { get; set; }

}
