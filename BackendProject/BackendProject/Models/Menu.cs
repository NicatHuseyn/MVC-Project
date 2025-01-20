using BackendProject.Models.Common;

namespace BackendProject.Models;

public class Menu : BaseEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
	public decimal Price { get; set; }
	public string Image { get; set; }
	public int MenuCategoryId { get; set; }
	public MenuCategory MenuCategory { get; set; }
	public bool IsDeleted { get; set; }
}
