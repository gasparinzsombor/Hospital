using System.Security.Cryptography;
using Hospital.Data;
using Hospital.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.WebAPI.Database;

public static class DbInitializer
{
    private static IServiceScope _serviceScope = null!;
    private static DoctorAsUserContext _context = null!;
    private static UserManager<Doctor> _userManager = null!;

    public static async Task InitializeAsync(IServiceScope serviceScope)
    {
        _serviceScope = serviceScope;
        _context = _serviceScope.ServiceProvider.GetService<DoctorAsUserContext>()!;
        _userManager = _serviceScope.ServiceProvider.GetService<UserManager<Doctor>>()!;

        await _context.Database.MigrateAsync();
        
        if (_context.Users.Any()) return;

        await AddDoctors();
        await _context.SaveChangesAsync();
    }

    private static async Task AddDoctors()
    {
        var cardiology = new Specialization
        {
            Name = "Kardiológia"
        };

        var toxicology = new Specialization
        {
            Name = "Toxikológia"
        };

        await _context.Specializations.AddAsync(cardiology);
        await _context.Specializations.AddAsync(toxicology);
        await _context.SaveChangesAsync();
        
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                UserName = "doktor1",
                Name = "Dr. A",
                Specialization = cardiology,
            },
            new Doctor
            {
                UserName = "doktor2",
                Name = "Dr. B",
                Specialization = toxicology,
            },
            new Doctor
            {
                UserName = "doktor3",
                Name = "Dr. C",
                Specialization = toxicology,
            },
        };

        var password = "Jelszo1234_";

        foreach (var doctor in doctors)
        {
            await _userManager.CreateAsync(doctor, password);
        }
        
        await _context.SaveChangesAsync();
    }
}