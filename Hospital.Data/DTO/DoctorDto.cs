namespace Hospital.Data.DTO;

public class DoctorDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public SpecializationDto Specialization { get; set; } = null!;

}