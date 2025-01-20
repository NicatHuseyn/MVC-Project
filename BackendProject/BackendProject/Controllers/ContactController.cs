using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactVM contactUs)
        {
            try
            {
                bool isExist = await _context.Contacts.AnyAsync(m =>
                m.Email.Trim() == contactUs.Email.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("Email", "Email already exist!");
                }

                Contact contact = new();
                contact.FullName = contactUs.FullName;
                contact.Email = contactUs.Email;
                contact.Subject = contactUs.Subject;
                contact.Message = contactUs.Message;
                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
