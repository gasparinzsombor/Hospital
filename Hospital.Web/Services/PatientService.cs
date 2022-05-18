using Hospital.Database;
using Hospital.Persistence.Entities;

namespace Hospital.Services;

public class PatientService
{
    private readonly PatientAsUserContext _context;

    public PatientService(PatientAsUserContext context)
    {
        _context = context;
    }

    public Patient? GetPatientByUserName(string name)
    {
        return _context.Patients.FirstOrDefault(p => p.UserName.Equals(name));
    }

    public Patient? GetById(int id)
    {
        return _context.Patients.FirstOrDefault(p => p.Id == id);
    }
}