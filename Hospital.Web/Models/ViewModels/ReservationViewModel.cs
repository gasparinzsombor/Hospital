namespace Hospital.Models.ViewModels;

public class ReservationViewModel
{
    public ReservationViewModel(DateTime dateTime, ReservationType type)
    {
        DateTime = dateTime;
        Type = type;
    }

    public DateTime DateTime { get; set; }
    public ReservationType Type { get; set; }
}

public enum ReservationType { ByUser, ByOthers }
