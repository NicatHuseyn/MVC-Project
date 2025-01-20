using BackendProject.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models;

public class Chef : BaseEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Image { get; set; }
	[NotMapped]
	public IFormFile Photo { get; set; }
	[NotMapped]
	public List<int>? SocialIds { get; set; }
	public int ExperienceInYears { get; set; }
	public bool IsDeleted { get; set; }
	public ICollection<SocialMedia> SocialMedias { get; set; }

}
