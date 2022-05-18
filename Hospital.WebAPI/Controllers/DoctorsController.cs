using Hospital.Data.DTO;
using Hospital.Persistence;
using Hospital.Persistence.Entities;
using Hospital.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _doctorService;
    private readonly SignInManager<Doctor> _signInManager;

    public DoctorsController(DoctorService doctorService, SignInManager<Doctor> signInManager)
    {
        _doctorService = doctorService;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Login(LoginDto dto)
    {
        var doctor = await _doctorService.GetByUserNameAsync(dto.UserName);
        if (doctor == null) return Unauthorized();
        
        var result = await _signInManager.PasswordSignInAsync(doctor, dto.Password, isPersistent: false, false);
        return result.Succeeded ? doctor.Id : Unauthorized();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}