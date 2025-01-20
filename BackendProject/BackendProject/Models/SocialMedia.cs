using BackendProject.Models.Common;

namespace BackendProject.Models
{
	public class SocialMedia : BaseEntity
	{
		public int Id { get; set; }
		public string Social { get; set; }

		public int ChefId { get; set; }
		public Chef Chef { get; set; }
	}
}
