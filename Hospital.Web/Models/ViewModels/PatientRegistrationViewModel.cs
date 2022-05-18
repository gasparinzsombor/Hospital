using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.ViewModels;

public class PatientRegistrationViewModel
{
    [Required(ErrorMessage = "A név megadása kötelező")]
    [DisplayName("Név")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "A felhasználónév megadása kötelező")]
    [DisplayName("Felhasználónév")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "A telefonszám megadása kötelező")]
    [DataType(DataType.PhoneNumber)]
    [DisplayName("Telefonszám")]
    [RegularExpression("[+]?[0-9]+\\s*$", ErrorMessage = "A telefonszám formátuma nem megfelelő")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "A cím megadása kötelező")]
    [DisplayName("Cím")]
    public string Address { get; set; } = null!;

    [DataType((DataType.Password))]
    [Required(ErrorMessage = "A jelszó megadása kötelező")]
    [DisplayName("Jelszó")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A jelszó újbóli megadása kötelező")]
    [DisplayName("Jelszó újra")]
    [Compare(nameof(Password), ErrorMessage = "A két jelszó nem egyezik")]
    public string ConfirmPassword { get; set; } = null!;
}