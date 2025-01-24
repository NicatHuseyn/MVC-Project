namespace BackendProject.Areas.Admin.ViewModels;

public class SliderViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public IFormFile? UserImage { get; set; }
    public string UserName { get; set; }
    public string Designation { get; set; }
}
