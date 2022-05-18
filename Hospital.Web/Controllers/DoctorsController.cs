using System.ComponentModel.DataAnnotations;
using Hospital.Models.Util;
using Hospital.Models.ViewModels;
using Hospital.Models.ViewModels.Reservations;
using Hospital.Persistence.Entities;
using Hospital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

[Authorize]
public class DoctorsController : BaseController
{
    private readonly DoctorService _doctorService;
    private readonly ReservationService _reservationService;
    private readonly LoginService _loginService;
    
    public DoctorsController(DoctorService doctorService, ReservationService reservationService, LoginService loginService)
    {
        _doctorService = doctorService;
        _reservationService = reservationService;
        _loginService = loginService;
    }


    [HttpGet]
    public IActionResult AddReservation(int? id, string? dateTimeStr)
    {
        if (id == null) return NotFound();
        
        var startDate = dateTimeStr?.ToUrlDateTime() ?? DateTime.Now.Date;

        var doctor = _doctorService.GetById(id.Value);
        if (doctor == null) return NotFound();
        
        var patient = _loginService.GetLoggedInUser()!;
        
        var reservations = _reservationService.GetReservationsFor7DaysForDoctor(startDate, doctor);
        var reservationViewModels = reservations
            .Select(r => new ReservationViewModel(
                r.DateTime,
                r.Patient.Id == patient.Id ? ReservationType.ByUser : ReservationType.ByOthers)
            );
            
        return View(new AddReservationByDoctorViewModel
        {
            StartDate = startDate.Date,
            ReservationViewModels = reservationViewModels,
            Date = DateTime.Now,
            Hour = 9,
            DoctorId = doctor.Id
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReservation(AddReservationByDoctorViewModel vm)
    {

        var patient = _loginService.GetLoggedInUser()!;

        var doctor = _doctorService.GetById(vm.DoctorId);
        if (doctor == null) return NotFound();
        
        if (!Validator.TryValidateObject(vm, new ValidationContext(vm), null))
        {
            return View(vm);
        }
        
        var reservations = _reservationService.GetReservationsFor7DaysForDoctor(vm.StartDate, doctor);
        var reservationViewModels = reservations
            .Select(r => new ReservationViewModel(
                r.DateTime,
                r.Patient.Id == patient.Id ? ReservationType.ByUser : ReservationType.ByOthers)
            );

        vm.ReservationViewModels = reservationViewModels;
    
        if (!ModelState.IsValid) return View(vm);

        
        var dateTime = vm.Date.Date.AddHours(vm.Hour);

        var reservation = new Reservation()
        {
            Patient = patient,
            Doctor = doctor,
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
        reservations = _reservationService.GetReservationsFor7DaysForDoctor(vm.StartDate, doctor);
        reservationViewModels = reservations
            .Select(r => new ReservationViewModel(
                r.DateTime,
                r.Patient.Id == patient.Id ? ReservationType.ByUser : ReservationType.ByOthers)
            );

        vm.ReservationViewModels = reservationViewModels;
        
        return View(vm);
    }
}