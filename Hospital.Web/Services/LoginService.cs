using System.Security.Claims;
using Hospital.Database;
using Hospital.Persistence.Entities;

namespace Hospital.Services;

public class LoginService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PatientAsUserContext _context;
    
    public LoginService(IHttpContextAccessor httpContextAccessor, PatientAsUserContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public Patient? GetLoggedInUser()
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _context.Patients.FirstOrDefault(u => u.Id == userId);
    }
}