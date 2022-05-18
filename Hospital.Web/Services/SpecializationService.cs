using Hospital.Database;
using Hospital.Persistence.Entities;

namespace Hospital.Services;

public class SpecializationService
{
    private readonly PatientAsUserContext _context;

    public SpecializationService(PatientAsUserContext context)
    {
        _context = context;
    }

    public Specialization? GetById(int id)
    {
        return _context.Specializations.FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<Specialization> List()
    {
        return _context.Specializations.Distinct();
    }
}