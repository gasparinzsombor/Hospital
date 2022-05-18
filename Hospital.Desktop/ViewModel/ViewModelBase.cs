using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hospital.Desktop.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? memberCalling = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberCalling));
        }
    }
}
