using System;
using System.Windows;
using System.Windows.Controls;
using Hospital.Data.DTO;
using Hospital.Desktop.Model;
using Hospital.Desktop.Model.Util;
using ViewModel.Util;

namespace Hospital.Desktop.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private readonly HospitalService _service;
    
    public DelegateCommand LoginCommand { get; }

    public State<string> UserNameState { get; } = new("");

    public State<string> PasswordState { get; } = new("");

    public LoginViewModel(HospitalService service)
    {
        _service = service;
        LoginCommand = new DelegateCommand(LoginClickExecuted);
    }

    private async void LoginClickExecuted(object? passwordBox)
    {
        PasswordState.Value = ((PasswordBox)passwordBox!).Password;
        try
        {
            if (await _service.CheckLogin(ToLoginDto()) is false)
            {
                MessageBox.Show("Hibás felhasználónév vagy jelszó", "", MessageBoxButton.OK);
            }
            else DoctorLoggedIn?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            Error.Show(e);
        }
    }

    public void Reset()
    {
        UserNameState.Value = "";
        PasswordState.Value = "";
    }

    public LoginDto ToLoginDto() => new LoginDto(UserNameState.Value, PasswordState.Value);
    public event EventHandler? DoctorLoggedIn;
}