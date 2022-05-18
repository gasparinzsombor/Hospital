using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models.ViewModels;
using Hospital.Services;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers;

public class HomeController : BaseController
{
    private readonly DoctorService _doctorService;
    private readonly SpecializationService _specializationService;

    public HomeController(DoctorService doctorService, SpecializationService specializationService)
    {
        _doctorService = doctorService;
        _specializationService = specializationService;
    }
    
    [Authorize]
    public IActionResult Index()
    {
        return View(new HomeIndexViewModel
        {
            Doctors = _doctorService.List().OrderBy(d => d.Name).ToList(),
            Specializations = _specializationService.List().OrderBy(s => s.Name).ToList()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}