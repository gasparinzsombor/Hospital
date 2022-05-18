using Hospital.Data.DTO;
using Hospital.Persistence.Entities;
using Hospital.WebAPI.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.WebAPI.Services;

public class DoctorService
{
    private readonly DoctorAsUserContext _context;

    public DoctorService(DoctorAsUserContext context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByUserNameAsync(string username)
    {
        return await _context.Doctors.FirstOrDefaultAsync(d => d.UserName.Equals(username));
    }

    public Doctor? GetById(int id) => _context.Doctors.FirstOrDefault(d => d.Id == id);
}