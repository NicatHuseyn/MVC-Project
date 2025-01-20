using BackendProject.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models;

public class Contact : BaseEntity
{
	public int Id { get; set; }
	public string FullName { get; set; }

	[DataType(DataType.EmailAddress)]
	public string Email { get; set; }

	public string Subject { get; set; }

	public string Message { get; set; }
}
