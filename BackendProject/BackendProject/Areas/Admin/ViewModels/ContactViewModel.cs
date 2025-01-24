using System.ComponentModel.DataAnnotations;

namespace BackendProject.Areas.Admin.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
