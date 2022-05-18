namespace Hospital.Data.DTO;

public class NewMedicalRecordDto
{
    public NewMedicalRecordDto(int reservationId, List<TreatmentDto> treatments)
    {
        ReservationId = reservationId;
        Treatments = treatments;
    }

    public int ReservationId { get; }
    public List<TreatmentDto> Treatments { get; }
    
}