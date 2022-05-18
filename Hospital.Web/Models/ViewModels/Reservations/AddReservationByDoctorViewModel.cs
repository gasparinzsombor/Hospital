using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Models.ViewModels.Reservations;

public class AddReservationByDoctorViewModel : ReservationTableViewModel
{
    [Required(ErrorMessage = "A dátum megadása kötelező")]
    [DataType(DataType.Date)]
    [DisplayName("Dátum")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Az időpont megadása kötelező")]
    [Range(9,17, ErrorMessage = "Csak 9 és 17 óra között lehet időpontot foglalni")]
    [DisplayName("Óra")]
    public int Hour { get; set; }
    
    [HiddenInput]
    public int DoctorId { get; set; }

    [DataType(DataType.MultilineText)]
    [MaxLength(200)]
    [DisplayName("Komment")]
    public string? Comment { get; set; }
}