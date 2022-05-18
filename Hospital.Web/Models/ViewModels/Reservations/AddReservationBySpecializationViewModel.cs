using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Models.ViewModels.Reservations;

public class AddReservationBySpecializationViewModel : ReservationTableViewModel
{
    [HiddenInput]
    public int SpecializationId { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Dátum")]
    public DateTime Date { get; set; }

    [Required]
    [Range(9,17)]
    [DisplayName("Óra")]
    public int Hour { get; set; }
    
    [DataType(DataType.MultilineText)]
    [MaxLength(200)]
    [DisplayName("Komment")]
    public string? Comment { get; set; }
}