using Hospital.Data.DTO;

namespace Hospital.Persistence.Entities
{
    public class Treatment
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; } = null!;

        public TreatmentDto ToDto() => new TreatmentDto()
        {
            Id = Id,
            Name = Name,
            Price = Price,
            MedicalRecordId = MedicalRecord.Id
        };
    }
}
