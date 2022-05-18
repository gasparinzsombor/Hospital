using Hospital.Data.DTO;
using Hospital.Persistence;
using Hospital.Persistence.Entities;
using Hospital.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly MedicalRecordService _medicalRecordService;
    private readonly ReservationService _reservationService;

    public MedicalRecordsController(MedicalRecordService medicalRecordService, ReservationService reservationService)
    {
        _medicalRecordService = medicalRecordService;
        _reservationService = reservationService;
    }

    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add(NewMedicalRecordDto medicalRecord)
    {
        var reservation = _reservationService.GetById(medicalRecord.ReservationId);
        if (reservation == null) return NotFound();

        if (medicalRecord.Treatments.Count == 0) return UnprocessableEntity();
        
        try
        {
            var mr = new MedicalRecord
            {
                Reservation = reservation,
                Treatments = medicalRecord.Treatments.Select(tdto => new Treatment
                {
                    Name = tdto.Name,
                    Price = tdto.Price
                }).ToList(),
                
            };
            foreach (var treatment in mr.Treatments)
            {
                treatment.MedicalRecord = mr;
            }
            await _medicalRecordService.AddMedicalRecordAsync(mr);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}