using Hospital.Database;
using Hospital.Persistence.Entities;

namespace Hospital.Services;

public class ReservationService
{
    private readonly PatientAsUserContext _context;

    public ReservationService(PatientAsUserContext context)
    {
        _context = context;
    }

    public IEnumerable<Reservation> GetByPatient(Patient patient)
    {
        return _context.Reservations.Where(r => r.Patient == patient);
    }

    

    public IEnumerable<Reservation> GetReservationsFor7DaysForSpecialization(DateTime start,
        Specialization specialization)
    {
        return _context.Reservations
            .Where(r =>
                start <= r.DateTime &&
                r.DateTime <= start.AddDays(7) &&
                r.Doctor.Specialization.Id == specialization.Id)
            .ToList();
    }

    public IEnumerable<Reservation> GetReservationsFor7DaysForDoctor(DateTime start, Doctor doctor)
    {
        return _context.Reservations
            .Where(r =>
                start <= r.DateTime &&
                r.DateTime <= start.AddDays(7) &&
                r.Doctor.Id == doctor.Id)
            .ToList();
    }

    public AddReservationResult AddReservation(Reservation reservation)
    {
        if (
            _context.Reservations.Any(r =>
                Equals(r.Doctor, reservation.Doctor) &&
                r.DateTime == reservation.DateTime)
        )
        {
            return AddReservationResult.DoctorUnavailable;
        }
        else if(
            _context.Reservations.Any(r => 
                r.DateTime == reservation.DateTime && 
                r.Patient.Id == reservation.Patient.Id)
            )
        {
            return AddReservationResult.PatientUnavailable;
        }

        try
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return AddReservationResult.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return AddReservationResult.UnknownError;
        }
    }

    public Reservation? GetById(int id)
    {
        return _context.Reservations.FirstOrDefault(r => r.Id == id);
    }

    public bool RemoveReservation(Reservation reservation)
    {
        try
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
}

public enum AddReservationResult {Success, DoctorUnavailable, PatientUnavailable, UnknownError}