using BackendProject.Models.Common;

namespace BackendProject.Models;

public class MenuCategory : BaseEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; }
	public ICollection<Menu> Menus { get; set; }

}
