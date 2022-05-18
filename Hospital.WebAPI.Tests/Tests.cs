using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Data.DTO;
using Hospital.Persistence.Entities;
using Hospital.WebAPI.Controllers;
using Hospital.WebAPI.Database;
using Hospital.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hospital.WebAPI.Tests;

public class Tests : IDisposable
{
    private readonly DoctorAsUserContext _context;
    private readonly MedicalRecordService _medicalRecordService;
    private readonly ReservationService _reservationService;
    private readonly DoctorService _doctorService;
    private readonly MedicalRecordsController _medicalRecordsController;
    private readonly ReservationsController _reservationsController;

    public Tests()
    {
        var options = new DbContextOptionsBuilder<DoctorAsUserContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _context = new DoctorAsUserContext(options);
        
        _medicalRecordService = new MedicalRecordService(_context);
        _reservationService = new ReservationService(_context);
        _doctorService = new DoctorService(_context);
        
        _medicalRecordsController = new MedicalRecordsController(_medicalRecordService, _reservationService);
        _reservationsController = new ReservationsController(_reservationService, _doctorService);
        
        DbInitializer.Initialize(_context);
    }
    
    [Fact]
    public async Task AddMedicalRecordTest()
    {
        var reservation = _context.Reservations.First();
        var dto = new NewMedicalRecordDto(reservation.Id, new List<TreatmentDto>
        {
            new()
            {
                Name = "gyógyszer1",
                Price = 1435
            }
        });

        var result = await _medicalRecordsController.Add(dto);
        Assert.IsAssignableFrom<OkResult>(result);

    }

    [Fact]
    public async Task AddMedicalRecordWrongReservationIdTest()
    {
        var reservationId = -1;
        var dto = new NewMedicalRecordDto(reservationId, new List<TreatmentDto>
        {
            new()
            {
                Name = "gyógyszer",
                Price = 12000
            }
        });

        var result = await _medicalRecordsController.Add(dto);
        Assert.IsAssignableFrom<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddMedicalRecordNoTreatmentTest()
    {
        var reservation = _context.Reservations.First();
        var dto = new NewMedicalRecordDto(reservation.Id, new List<TreatmentDto>());

        var result = await _medicalRecordsController.Add(dto);
        Assert.IsAssignableFrom<UnprocessableEntityResult>(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReservationsGetByDoctorIdFromToday(int doctorId)
    {
        var data = _reservationsController.GetByDoctorIdFromToday(doctorId);
        Assert.IsAssignableFrom<ActionResult<IEnumerable<ReservationDto>>>(data);
    }

    [Fact]
    public void ReservationsGetByDoctorIdFromTodayWrongDoctorId()
    {
        var data = _reservationsController.GetByDoctorIdFromToday(-1);
        Assert.IsAssignableFrom<NotFoundResult>(data.Result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReservationsGetByDoctorIdFromTodayOwnReservations(int doctorId)
    {
        var data = _reservationsController.GetByDoctorIdFromToday(doctorId);
        Assert.NotNull(data.Value);
        Assert.DoesNotContain(data.Value!, dto => dto.Doctor.Id != doctorId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReservationsGetByDoctorIdFromTodayOnlyFuture(int doctorId)
    {
        var data = _reservationsController.GetByDoctorIdFromToday(doctorId);
        Assert.NotNull(data.Value);
        Assert.DoesNotContain(data.Value!, dto => dto.DateTime < DateTime.Today);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReservationsGetByDoctorIdFromTodayOnlyWithoutMedicalRecords(int doctorId)
    {
        var data = _reservationsController.GetByDoctorIdFromToday(doctorId);
        var reservWithMRs = _context.MedicalRecords.Select(md => md.Reservation);
        Assert.NotNull(data.Value);
        Assert.Empty(data.Value!.Select(r => r.Id).Intersect(reservWithMRs.Select(r => r.Id)));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}