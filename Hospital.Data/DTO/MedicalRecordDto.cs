namespace Hospital.Data.DTO;

public class MedicalRecordDto
{
    public int Id { get; set; }

    public ICollection<TreatmentDto> Treatments { get; set; } = null!;
    public ReservationDto Reservation { get; set; } = null!;
}