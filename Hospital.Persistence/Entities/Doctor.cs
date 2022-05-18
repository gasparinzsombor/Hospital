using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Data.DTO;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Persistence.Entities
{
    public class Doctor : IdentityUser<int>
    {
        [Display(Name = "Név")]
        public string Name { get; set; } = null!;
        
        [Display(Name = "Szakterület")]
        public virtual Specialization Specialization { get; set; } = null!;


        public DoctorDto ToDto() => new DoctorDto()
        {
            Id = Id,
            Name = Name,
            Specialization = Specialization.ToDto()
        };
        

        public override bool Equals(object? obj)
        {
            if (obj is not Doctor doctor) return false;
            return Id == doctor.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
