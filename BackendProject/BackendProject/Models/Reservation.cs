namespace BackendProject.Models;

public class Reservation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public int PersonCount { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
    public string Email { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
