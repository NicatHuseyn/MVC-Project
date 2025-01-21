namespace BackendProject.Areas.Admin.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Product>? Products { get; set; }
}
