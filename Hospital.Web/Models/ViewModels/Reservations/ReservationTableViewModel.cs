using Microsoft.AspNetCore.Mvc;

namespace Hospital.Models.ViewModels.Reservations;

public class ReservationTableViewModel
{
    public IEnumerable<ReservationViewModel>? ReservationViewModels { get; set; }
    
    [HiddenInput]
    public DateTime StartDate { get; set; }

    public string CellClass(DateTime date)
    {
        var rvm = ReservationViewModels!.FirstOrDefault(r => r.DateTime == date);
        if (rvm == null) return "table-success";
        return rvm.Type == ReservationType.ByUser ? "table-info" : "table-danger";
    }
}