using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReservationsController : Controller
{
    private readonly AppDbContext _context;

    public ReservationsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var reservations = _context.Reservations.ToList();
        ViewBag.Count = reservations.Count;

        return View(reservations);
    }


    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        Reservation? reservation = _context.Reservations.FirstOrDefault(x => x.Id == id);
        if (reservation == null)
            return NotFound();

        return View(reservation);
    }

    [HttpPost]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = _context.Reservations.FirstOrDefault(x=>x.Id == id);
        if (reservation == null)
            return NotFound();
        _context.Reservations.Remove(reservation);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Moderator")]
    public IActionResult Detail(int id)
    {
        Reservation? reservation = _context.Reservations.AsNoTracking().FirstOrDefault(s => s.Id == id);
        if (reservation is null)
            return NotFound();


        return View(reservation);
    }


    [Authorize(Roles = "Admin")]
    public IActionResult DeleteAllReservation()
    {
        var reservations = _context.Reservations.ToList();
        if (!reservations.Any())
            return BadRequest("No reservations to delete.");

        return View(reservations);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteAllReservationConfirmed()
    {
        var reservations = _context.Reservations.ToList();
        if (!reservations.Any())
            return BadRequest("No reservations to delete.");

        _context.Reservations.RemoveRange(reservations);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

}
