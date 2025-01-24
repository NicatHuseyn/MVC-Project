using BackendProject.Areas.Admin.ViewModels;
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
            var contacts = await _context.Contacts
            .Select(c => new ContactViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Subject = c.Subject,
                Message = c.Message
            }).ToListAsync();

            return View(contacts);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(ContactVM contactUs)
        //{
        //    try
        //    {
        //        bool isExist = await _context.Contacts.AnyAsync(m =>
        //        m.Email.Trim() == contactUs.Email.Trim());

        //        if (isExist)
        //        {
        //            ModelState.AddModelError("Email", "Email already exist!");
        //        }

        //        Contact contact = await _context.Contacts.FirstOrDefaultAsync();
        //        contact.FullName = contactUs.FullName;
        //        contact.Email = contactUs.Email;
        //        contact.Subject = contactUs.Subject;
        //        contact.Message = contactUs.Message;

        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception)
        //    {
        //        return View();
        //    }

        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Subject = viewModel.Subject,
                    Message = viewModel.Message
                };

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {
            Contact? contact = _context.Contacts.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (contact is null)
                return NotFound();
            return View(contact);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var viewModel = new ContactViewModel
            {
                Id = contact.Id,
                FullName = contact.FullName,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ContactViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contact = await _context.Contacts.FindAsync(id);
                    if (contact == null)
                    {
                        return NotFound();
                    }

                    contact.FullName = viewModel.FullName;
                    contact.Email = viewModel.Email;
                    contact.Subject = viewModel.Subject;
                    contact.Message = viewModel.Message;

                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Where(c => c.Id == id)
                .Select(c => new ContactViewModel
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Subject = c.Subject,
                    Message = c.Message
                }).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
