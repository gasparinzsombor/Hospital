using Hospital.Persistence.Entities;
using Hospital.WebAPI.Database;

namespace Hospital.WebAPI.Services;

public class MedicalRecordService
{
    private readonly DoctorAsUserContext _context;

    public MedicalRecordService(DoctorAsUserContext context)
    {
        _context = context;
    }

    public async Task AddMedicalRecordAsync(MedicalRecord medicalRecord)
    {
        await _context.MedicalRecords.AddAsync(medicalRecord);
        await _context.SaveChangesAsync();
    }
}