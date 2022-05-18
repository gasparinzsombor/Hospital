using Hospital.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Database;

public static class DbInitializer
{
    private static IServiceScope _serviceScope = null!;
    private static PatientAsUserContext _context = null!;
    private static UserManager<Patient> _patientUserManager = null!;

    public static async Task Initialize(IServiceScope serviceScope)
    {
        _serviceScope = serviceScope;
        await using (_context = _serviceScope.ServiceProvider.GetService<PatientAsUserContext>()!)
        using(_patientUserManager = _serviceScope.ServiceProvider.GetService<UserManager<Patient>>()!)
        {
            await _context.Database.MigrateAsync();
        
            // if (_context.Doctors.Any()) return;
            //
            // await AddPatients();
            // await AddReservations();
            // await _context.SaveChangesAsync();
        }
    }

    /*private static async Task AddPatients()
    {
        var patient = new Patient()
        {
            Name = "User 1",
            UserName = "user1",
            Address = "1115 Budapest Alma utca 22.",
            PhoneNumber = "+36301234567",
            Email = "alma@inf.elte.hu"
        };
        await _patientUserManager.CreateAsync(patient, "Jelszo1234_");
        
        var patient2 = new Patient
        {
            Name = "User 2",
            UserName = "user2",
            Address = "9065 Sopron, Körte utca 23.",
            PhoneNumber = "+36309876543",
            Email = "korte@inf.elte.hu"
        };
        await _patientUserManager.CreateAsync(patient2, "Jelszo1234_");
        await _context.SaveChangesAsync();
    }

    private static async Task AddReservations()
    {
        
        var reservation1 = new Reservation()
        {
            DateTime = DateTime.Now.Date.AddHours(10),
            Doctor = _context.Doctors.First(),
            Comment = "Ez egy komment",
            Patient = _context.Users.First(),
        };
        await _context.AddAsync(reservation1);
        await _context.SaveChangesAsync();
    }*/
}