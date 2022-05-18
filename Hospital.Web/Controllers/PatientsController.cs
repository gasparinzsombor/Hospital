using System.ComponentModel.DataAnnotations;
using Hospital.Models.ViewModels;
using Hospital.Persistence.Entities;
using Hospital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

public class PatientsController : BaseController
{
    private readonly UserManager<Patient> _userManager;
    private readonly SignInManager<Patient> _signInManager;
    private readonly PatientService _patientService;
    private readonly ReservationService _reservationService;
    private readonly LoginService _loginService;
    
    public PatientsController(UserManager<Patient> userManager, SignInManager<Patient> signInManager, PatientService patientService, ReservationService reservationService, LoginService loginService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _patientService = patientService;
        _reservationService = reservationService;
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(PatientLoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        
        var patient = _patientService.GetPatientByUserName(vm.UserName);

        if (patient == null)
        {
            ModelState.AddModelError("", "Hibás felhasználónév vagy jelszó");
            return View(vm);
        }
        
        var result = await _signInManager.PasswordSignInAsync(patient, vm.Password, vm.StayLoggedIn, true);

        if (result.Succeeded)
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        
        ModelState.AddModelError("", "Hibás felhasználónév vagy jelszó");
        return View(vm);
    }
    
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(PatientRegistrationViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var patient = new Patient()
        {
            Name = vm.Name,
            Address = vm.Address,
            PhoneNumber = vm.PhoneNumber,
            UserName = vm.UserName
        };
        
        if (!Validator.TryValidateObject(vm, new(vm), null))
        {
            return View(vm);
        }

        var result =  await _userManager.CreateAsync(patient, vm.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(vm);
        }

        await _signInManager.PasswordSignInAsync(patient, vm.Password, false, true);
        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }
    
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [Authorize]
    public IActionResult Details()
    {
        var patient = _loginService.GetLoggedInUser()!;

        var reservations = _reservationService.GetByPatient(patient).Where(r => r.DateTime >= DateTime.Now);
        
        return View(reservations);
    }

    [Authorize]
    [HttpGet]
    public IActionResult RemoveReservation(int id)
    {
        var reservation = _reservationService.GetById(id);
        if (reservation == null) return NotFound();

        var result = _reservationService.RemoveReservation(reservation);
        if(!result) ModelState.AddModelError("", "Nem sikerült törölni a foglalást");
        return RedirectToAction("Details");
    }
}