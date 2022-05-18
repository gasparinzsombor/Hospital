using System;
using System.Threading.Tasks;
using Hospital.Data.DTO;
using Hospital.Desktop.Model;
using Hospital.Desktop.Model.Util;
using ViewModel.Util;

namespace Hospital.Desktop.ViewModel.Main;

public class ReservationViewModel : ViewModelBase
{
    private readonly HospitalService _service;
    private readonly ReservationDto _reservationDto;

    public ReservationViewModel(ReservationDto dto, HospitalService service)
    {
        _service = service;
        _reservationDto = dto;
        DateTime = dto.DateTime;
        Comment = _reservationDto.Comment ?? "";
        PatientNameState.Value = _reservationDto.Patient.Name;
    }

    public DateTime DateTime { get; set; }

    public State<string> PatientNameState { get; } = new("");
    public string Comment { get; set; }

    public ReservationDto ToReservationDto() => _reservationDto;
}