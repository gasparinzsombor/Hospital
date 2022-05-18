using Hospital.Data.DTO;

namespace Hospital.Persistence.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; } = null!;

        public virtual Reservation Reservation { get; set; } = null!;
        
        
        public MedicalRecordDto ToDto() => new MedicalRecordDto()
        {
            Id = Id,
            Treatments = Treatments.Select(t => t.ToDto()).ToList(),
            Reservation = Reservation.ToDto()
        };
    }
}
