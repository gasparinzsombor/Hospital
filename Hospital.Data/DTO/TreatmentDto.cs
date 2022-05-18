namespace Hospital.Data.DTO;

public class TreatmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Price { get; set; }
    
    public int MedicalRecordId { get; set; }
}