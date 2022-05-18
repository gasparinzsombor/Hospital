using Hospital.Data.DTO;
using Hospital.Desktop.Model.Util;

namespace Hospital.Desktop.ViewModel.NewMedicalRecord;

public class TreatmentViewModel : ViewModelBase
{
    public State<int> PriceState { get; } = new(0);
    public State<string> NameState { get; } = new("");

    public TreatmentDto ToDto() => new TreatmentDto
    {
        Name = NameState.Value,
        Price = PriceState.Value
    };
}