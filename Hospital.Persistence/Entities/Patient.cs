using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Data.DTO;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Persistence.Entities;

public class Patient : IdentityUser<int>
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;

    public PatientDto ToDto() => new PatientDto()
    {
        Id = Id,
        Name = Name,
        Address = Address,
        PhoneNumber = PhoneNumber
    };
}