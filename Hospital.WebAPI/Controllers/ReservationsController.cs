using Hospital.Data.DTO;
using Hospital.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly DoctorService _doctorService;

    public ReservationsController(ReservationService reservationService, DoctorService doctorService)
    {
        _reservationService = reservationService;
        _doctorService = doctorService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<ReservationDto>> GetByDoctorIdFromToday(int doctorId)
    {
        var doctor = _doctorService.GetById(doctorId);
        if (doctor == null) return NotFound();
        
        
        return _reservationService
            .GetByDoctorIdFromToday(doctorId)
            .Select(r => r.ToDto())
            .ToList();
    }
}