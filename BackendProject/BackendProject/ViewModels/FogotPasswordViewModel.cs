using System.ComponentModel.DataAnnotations;

namespace BackendProject.ViewModels;

public class FogotPasswordViewModel
{
    [Required, DataType(DataType.EmailAddress), MaxLength(256)]
    public string Email { get; set; }
}
