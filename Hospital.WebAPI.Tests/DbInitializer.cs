using System;
using Hospital.Persistence.Entities;
using Hospital.WebAPI.Database;

namespace Hospital.WebAPI.Tests;

public static class DbInitializer
{
    private static DoctorAsUserContext _context = null!;
    public static void Initialize(DoctorAsUserContext context)
    {
        _context = context;

        var patient1 = new Patient()
        {
            Email = "alma@gmail.com",
            Address = "1111 Budapest, Alma utca 12.",
            Name = "Gasparin Zsombor",
            UserName = "zsombor",
            PhoneNumber = "06111111111"
        };
        
        var speci = new Specialization
        {
            Name = "Specializáció neve"
        };

        var doctor1 = new Doctor
        {
            Name = "Dr. Zsombor",
            Specialization = speci
        };

        var doctor2 = new Doctor
        {
            Name = "Dr. Dánielr",
            Specialization = speci
        };
        

        var reserv = new Reservation
        {
            Doctor = doctor1,
            Comment = "Komment1",
            Patient = patient1,
            DateTime = DateTime.Today
        };
        
        var reserv2 = new Reservation
        {
            Doctor = doctor2,
            Comment = "Komment2",
            Patient = patient1,
            DateTime = DateTime.Today
        };
        
        var reserv3 = new Reservation
        {
            Doctor = doctor2,
            Comment = "Komment2",
            Patient = patient1,
            DateTime = DateTime.Today.Subtract(TimeSpan.FromDays(10))
        };
        
        _context.Patients.Add(patient1);
        _context.Specializations.Add(speci);
        _context.Doctors.Add(doctor1);
        _context.Doctors.Add(doctor2);
        _context.Reservations.Add(reserv);
        _context.Reservations.Add(reserv2);
        _context.SaveChanges();

    }
}