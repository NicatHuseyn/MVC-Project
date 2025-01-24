namespace BackendProject.ViewModels;

public class ReservationViewModel
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public int PersonCount { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
    public string Email { get; set; }
}
