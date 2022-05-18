using Hospital.Persistence.Entities;
using Hospital.WebAPI.Database;

namespace Hospital.WebAPI.Services;

public class PatientService
{
    private readonly DoctorAsUserContext _context;

    public PatientService(DoctorAsUserContext context)
    {
        _context = context;
    }


    public Patient? GetById(int id) => _context.Patients.FirstOrDefault(p => p.Id == id);
}