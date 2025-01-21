using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            List<Contact> contacts = _context.Contacts.ToList();
            ViewBag.Count = contacts.Count;
            return View(contacts);
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

                Contact contact = await _context.Contacts.FirstOrDefaultAsync();
                contact.FullName = contactUs.FullName;
                contact.Email = contactUs.Email;
                contact.Subject = contactUs.Subject;
                contact.Message = contactUs.Message;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }

        }

        public IActionResult Detail(int id)
        {
            Contact? contact = _context.Contacts.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (contact is null)
                return NotFound();
            return View(contact);
        }

        public IActionResult Delete(int id)
        {
            if (_context.Contacts.Count() == 1)
                return BadRequest();

            Contact? contact = _context.Contacts.FirstOrDefault(s => s.Id == id);
            if (contact is null)
                return NotFound();

            return View(contact);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteContact(int id)
        {
            Contact? contact = _context.Contacts.FirstOrDefault(s => s.Id == id);
            if (contact is null)
                return NotFound();

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
