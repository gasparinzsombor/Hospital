namespace Hospital.Data.DTO;

public class ReservationDto
{
    public int Id { get; set; }
        
    public DateTime DateTime { get; set; }
    
    public string? Comment { get; set; }

    public PatientDto Patient { get; set; } = null!;

    public DoctorDto Doctor { get; set; } = null!;
}