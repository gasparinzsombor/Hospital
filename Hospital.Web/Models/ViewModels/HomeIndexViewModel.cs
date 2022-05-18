using Hospital.Persistence.Entities;

namespace Hospital.Models.ViewModels;

public class HomeIndexViewModel
{
    public IEnumerable<Doctor> Doctors = null!;
    public IEnumerable<Specialization> Specializations = null!;

    
}