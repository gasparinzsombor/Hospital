using Hospital.Persistence.Entities;
using Hospital.WebAPI.Database;

namespace Hospital.WebAPI.Services;

public class ReservationService
{
    private readonly DoctorAsUserContext _context;

    public ReservationService(DoctorAsUserContext context)
    {
        _context = context;
    }

    public Reservation? GetById(int id) => _context
        .Reservations
        .FirstOrDefault(r => r.Id == id);
    
    public IEnumerable<Reservation> GetByDoctorIdFromToday(int doctorId)
    {
        var reservations = _context.Reservations.
            Where(r => 
                r.Doctor.Id == doctorId &&
                r.DateTime >= DateTime.Now)
            .ToList();

        var reservationsWithMedicalRecords = _context
            .MedicalRecords
            .Where(m => m.Reservation.Doctor.Id == doctorId)
            .Select(m => m.Reservation);

        return reservations.Except(reservationsWithMedicalRecords).ToList();
    }
}