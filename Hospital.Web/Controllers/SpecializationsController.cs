using System.ComponentModel.DataAnnotations;
using Hospital.Models.Util;
using Hospital.Models.ViewModels;
using Hospital.Models.ViewModels.Reservations;
using Hospital.Persistence.Entities;
using Hospital.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

public class SpecializationsController : BaseController
{
    private readonly LoginService _loginService;
    private readonly ReservationService _reservationService;
    private readonly SpecializationService _specializationService;
    private readonly DoctorService _doctorService;
    
    public SpecializationsController(LoginService loginService, ReservationService reservationService, SpecializationService specializationService, DoctorService doctorService)
    {
        _loginService = loginService;
        _reservationService = reservationService;
        _specializationService = specializationService;
        _doctorService = doctorService;
    }

    public IActionResult Index()
    {
        return NotFound();
    }

    [HttpGet]
    public IActionResult AddReservation(int? id, string? dateTimeStr)
    {
        if (id == null) return NotFound();

        var specialization = _specializationService.GetById(id.Value);
        if (specialization == null) return NotFound();
        
        var startDate = dateTimeStr?.ToUrlDateTime() ?? DateTime.Now;
        
        var reservations = _reservationService.GetReservationsFor7DaysForSpecialization(startDate, specialization);

        var reservationViewModels = MakeReservationViewModels(reservations);
            
        return View(new AddReservationBySpecializationViewModel
        {
            StartDate = startDate.Date,
            ReservationViewModels = reservationViewModels,
            Date = DateTime.Now,
            Hour = 9,
            SpecializationId = specialization.Id
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReservation(AddReservationBySpecializationViewModel vm)
    {

        var patient = _loginService.GetLoggedInUser()!;

        var specialization = _specializationService.GetById(vm.SpecializationId);
        if (specialization == null) return NotFound();

        if (!Validator.TryValidateObject(vm, new ValidationContext(vm), null))
        {
            return View(vm);
        }
        
        var reservations = _reservationService.GetReservationsFor7DaysForSpecialization(vm.StartDate, specialization);
        var reservationViewModels = MakeReservationViewModels(reservations);
        

        vm.ReservationViewModels = reservationViewModels;
    
        if (!ModelState.IsValid) return View(vm);

        
        var dateTime = vm.Date.Date.AddHours(vm.Hour);

        var doctor = _doctorService.GetMostAvailableDoctorAtDateTime(dateTime);
        if(doctor == null) ModelState.AddModelError("", "Erre az időpontra már nincs hely.");
        
        var reservation = new Reservation()
        {
            Patient = patient,
            Doctor = doctor!,
            DateTime = dateTime,
            Comment = vm.Comment
        };
        
        var result = _reservationService.AddReservation(reservation);
        switch(result)
        {
            case AddReservationResult.DoctorUnavailable:
                ModelState.AddModelError("", "Ez az időpont már foglalt");
                break;
            case AddReservationResult.PatientUnavailable:
                ModelState.AddModelError("", "Itt már van egy lefoglalt időpontja");
                break;
            case AddReservationResult.UnknownError:
                ModelState.AddModelError("", "Hiba történt, kérjük próbálja újra később");
                break;
            case AddReservationResult.Success:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        reservations = _reservationService.GetReservationsFor7DaysForSpecialization(vm.StartDate, specialization);
        reservationViewModels = MakeReservationViewModels(reservations);
        

        vm.ReservationViewModels = reservationViewModels;
        
        return View(vm);
    }

    private IEnumerable<ReservationViewModel> MakeReservationViewModels(IEnumerable<Reservation> reservations)
    {
        var user = _loginService.GetLoggedInUser()!;
        var reservationsByDateTime = reservations.GroupBy(r => r.DateTime);

        var doctorCount = _doctorService.List().Count();

        var reservationViewModels = reservationsByDateTime
            .Select(g =>
            {
                if (g.FirstOrDefault(r => r.Patient.Id == user.Id) != null)
                {
                    return new ReservationViewModel(g.Key, ReservationType.ByUser);
                }

                return g.Count() == doctorCount ? new ReservationViewModel(g.Key, ReservationType.ByOthers) : null;
            })
            .Where(rvm => rvm != null)
            .Select(rvm => rvm!);

        return reservationViewModels;
    }
}