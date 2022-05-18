using Hospital.Database;
using Hospital.Persistence.Entities;

namespace Hospital.Services;

public class DoctorService
{
    private readonly PatientAsUserContext _context;

    public DoctorService(PatientAsUserContext context)
    {
        _context = context;
    }

    public IEnumerable<Doctor> List() => _context.Doctors;

    public Doctor? GetById(int id)
    {
        return _context.Doctors.FirstOrDefault(d => d.Id == id);
    }

    public Doctor? GetMostAvailableDoctorAtDateTime(DateTime dateTime)
    {
        var allDoctors = _context.Doctors.ToHashSet();

        var unavailableDoctors = _context.Reservations
            .Where(r => r.DateTime == dateTime)
            .Select(r => r.Doctor)
            .ToHashSet();

        var availableDoctors = allDoctors.Except(unavailableDoctors);

        return _context.Reservations
            .AsEnumerable()
            .Where(r => availableDoctors.Contains(r.Doctor))
            .GroupBy(r => r.Doctor)
            .MinBy(r => r.Count())
            ?.Key;
    }
}