using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{

    [Authorize]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            int maxCapacity = 50;
            int currentReservationCount = await _context.Reservations.SumAsync(r => r.PersonCount);
            if (currentReservationCount + model.PersonCount > maxCapacity)
            {
                return RedirectToAction("NonReservation");
            }


            var reservation = new Reservation
            {
                Name = model.Name,
                Phone = model.Phone,
                PersonCount = model.PersonCount,
                ReservationDate = model.ReservationDate,
                ReservationTime = model.ReservationTime,
                Email = model.Email,
                AppUserId = user.Id
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index",reservation);
        }


        public IActionResult ReservationConfirmed()
        {
            return View();
        }


        public IActionResult NonReservation()
        {
            return View();
        }
    }



}
