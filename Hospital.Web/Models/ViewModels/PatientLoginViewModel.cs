using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.ViewModels;

public class PatientLoginViewModel
{
    [DisplayName("Felhasználónév")]
    [Required(ErrorMessage = "a felhasználónév megadása kötelező")]
    public string UserName { get; set; } = null!;

    [DisplayName("Jelszó")]
    [Required(ErrorMessage = "a jelszó megadása kötelező")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DisplayName("Maradjon bejelenkezve")]
    public bool StayLoggedIn { get; set; }

}