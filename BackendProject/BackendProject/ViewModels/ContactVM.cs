using System.ComponentModel.DataAnnotations;

namespace BackendProject.ViewModels
{
	public class ContactVM
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }
	}
}
