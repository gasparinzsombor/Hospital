using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Hospital.Data.DTO;
using Hospital.Desktop.Model.Util;

namespace Hospital.Desktop.Model;

public class HospitalService
{
    private readonly HttpClient _client;
    public int? UserId { get; private set; }

    public HospitalService(string baseUrl)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(baseUrl);
    
    }

    public async Task<IEnumerable<ReservationDto>> ListReservations()
    {
        var response = await _client.GetAsync($"api/Reservations/GetByDoctorIdFromToday?doctorId={UserId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<IEnumerable<ReservationDto>>();
        }

        throw new NetworkException($"Service returned response: {response.StatusCode}");
    }
    public async Task<bool> CheckLogin(LoginDto loginDto)
    {
        var response = await _client.PostAsJsonAsync("api/Doctors/Login", loginDto);
        
        if (!response.IsSuccessStatusCode) return false;
        
        UserId = await response.Content.ReadFromJsonAsync<int>();
        return true;
    }

    public async Task SaveMedicalRecord(NewMedicalRecordDto mr)
    {
        var response = await _client.PostAsJsonAsync("api/MedicalRecords/Add", mr);

        if (!response.IsSuccessStatusCode)
        {
            throw new NetworkException(response.StatusCode.ToString());
        }
    }
    
    public async Task Logout()
    {
        var response = await _client.GetAsync("api/Doctors/Logout");
        if (response.IsSuccessStatusCode)
        {
            UserId = null;
        }
        else
        {
            throw new NetworkException("Couldn't log out");
        }
    }
}