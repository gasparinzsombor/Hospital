using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Data.DTO;
using Hospital.Desktop.Model;
using Hospital.Desktop.Model.Util;
using ViewModel.Util;

namespace Hospital.Desktop.ViewModel.NewMedicalRecord;

public class NewMedicalRecordViewModel : ViewModelBase
{
    private readonly HospitalService _service;
    
    public ObservableCollection<TreatmentViewModel> MedicalRecordItems { get; } = new();
    public State<int> SumState { get; }

    public State<TreatmentViewModel?> SelectedItemState { get; } = new(null);
    
    public ReservationDto Reservation { get; }
    
    public DelegateCommand NewItemCommand { get; }
    
    public DelegateCommand RemoveItemCommand { get; }

    public DelegateCommand SaveCommand { get; }
    
    public DelegateCommand CancelCommand { get; }

    public NewMedicalRecordViewModel(HospitalService service, ReservationDto reservation)
    {
        _service = service;
        Reservation = reservation;

        SumState = MedicalRecordItems.AsState(oc =>
            oc.Select(tvm => tvm.PriceState.Value)
                .Sum());

        NewItemCommand = new DelegateCommand(_ => MedicalRecordItems.Add(new TreatmentViewModel()));
        
        MedicalRecordItems.Do(tvm =>
        {
            tvm.PriceState.Subscribe(SumState, _ => 
                MedicalRecordItems
                    .Select(t => t.PriceState.Value)
                    .Sum());
        });

        MedicalRecordItems.CollectionChanged += (_, _) => SumState.Value = MedicalRecordItems
            .Select(t => t.PriceState.Value)
            .Sum();
        
        RemoveItemCommand = new DelegateCommand(
            SelectedItemState.Select(x => x is not null), 
            i => MedicalRecordItems.Remove((TreatmentViewModel) i!));

        SaveCommand = new DelegateCommand(
            MedicalRecordItems.AsState(i => i.Count != 0),
            async _ => await SaveMedicalRecord(null));

        CancelCommand = new DelegateCommand(_ => WindowClosed?.Invoke(this, EventArgs.Empty));
    }

    private async Task SaveMedicalRecord(object? _)
    {
        var result = MessageBox.Show("Biztosan szeretnéd menteni a kórlapot?", "Mentés megerősítése",
            MessageBoxButton.YesNo);

        if (result == MessageBoxResult.No) return;
        try
        {
            await _service.SaveMedicalRecord(new NewMedicalRecordDto(
                Reservation.Id, 
                MedicalRecordItems
                    .Select(i => i.ToDto())
                    .ToList()
                ));
            WindowClosed?.Invoke(this, EventArgs.Empty);
            
        }
        catch (NetworkException e)
        {
            Error.Show(e);
        }
    }

    public event EventHandler? WindowClosed;
}