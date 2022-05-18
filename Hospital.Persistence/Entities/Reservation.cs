using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hospital.Data.DTO;

namespace Hospital.Persistence.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        
        [DisplayName("Időpont")]
        public DateTime DateTime { get; set; }
        
        [DisplayName("Komment")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }
        
        [DisplayName("Páciens")]
        public virtual Patient Patient { get; set; } = null!;
        
        [DisplayName("Doktor")]
        public virtual Doctor Doctor { get; set; } = null!;

        public ReservationDto ToDto() => new ReservationDto()
        {
            Id = Id,
            Comment = Comment,
            Patient = Patient.ToDto(),
            DateTime = DateTime
        };
    }
}
