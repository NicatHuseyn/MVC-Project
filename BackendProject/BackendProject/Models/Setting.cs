using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models;

public class Setting
{

	public int Id { get; set; }
	[Required]
	public string Key { get; set; }
	[Required]
	public string Value { get; set; }
}
