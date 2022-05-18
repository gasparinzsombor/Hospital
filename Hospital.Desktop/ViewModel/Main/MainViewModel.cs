using System;
using System.Collections.ObjectModel;
using System.Linq;
using Hospital.Data.DTO;
using Hospital.Desktop.Model;
using Hospital.Desktop.Model.Util;
using ViewModel.Util;

namespace Hospital.Desktop.ViewModel.Main;

public class MainViewModel
{
    private readonly HospitalService _service;

    public ObservableCollection<ReservationViewModel> Reservations { get; } = new();

    public State<ReservationViewModel?> SelectedRow { get; } = new(null);
    
    public DelegateCommand LogoutCommand { get; }
    public DelegateCommand RefreshCommand { get; }
    
    public DelegateCommand NewMedicalRecordCommand { get; }
    
    public MainViewModel(HospitalService service)
    {
        _service = service;
        LogoutCommand = new DelegateCommand(_ => Logout?.Invoke(this, EventArgs.Empty));
        RefreshCommand = new DelegateCommand(_ => Refresh());
        NewMedicalRecordCommand = new DelegateCommand(
            SelectedRow.Select(x => x is not null),
            _ => NewMedicalRecordClicked?.Invoke(this, SelectedRow.Value!.ToReservationDto()));
    }

    public void Reset()
    {
        Reservations.Clear();
    }
    
    public async void Refresh()
    {
        Reservations.Clear();
        try
        {
            (await _service.ListReservations())
                .ToList()
                .ForEach(r => Reservations.Add(new ReservationViewModel(r, _service)));
        }
        catch (Exception e)
        {
            Error.Show(e);
            throw;
        }
    }

    public event EventHandler? Logout;
    public event EventHandler<ReservationDto>? NewMedicalRecordClicked;
}