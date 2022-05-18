 using Hospital.Data.DTO;

 namespace Hospital.Persistence.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public SpecializationDto ToDto() => new SpecializationDto()
        {
             Id = Id,
             Name = Name
        };
    }
}
