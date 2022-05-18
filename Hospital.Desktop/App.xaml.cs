using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Data.DTO;
using Hospital.Desktop.Model;
using Hospital.Desktop.Model.Util;
using Hospital.Desktop.View;
using Hospital.Desktop.ViewModel;
using Hospital.Desktop.ViewModel.Main;
using Hospital.Desktop.ViewModel.NewMedicalRecord;

namespace Hospital.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MainWindow _mainWindow = new();
        private readonly LoginWindow _loginWindow = new();
        private readonly NewMedicalRecordWindow _newMedicalRecordWindow = new();

        private readonly MainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewModel;
        private readonly State<NewMedicalRecordViewModel?> _newMedicalRecordViewModelState = new(null);

        
        private readonly HospitalService _service;
        
        public App()
        {
            _service = new HospitalService("https://localhost:7033/");

            _mainViewModel = new MainViewModel(_service);
            _loginViewModel = new LoginViewModel(_service);
            _newMedicalRecordViewModelState.Do(vm =>
            {
                if (vm is not null) vm.WindowClosed += NewMedicalRecordViewModelOnWindowClosed;
            });

            Startup += OnStartup;
            
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _mainViewModel.Logout += MainViewModelOnLogout;
            _mainViewModel.NewMedicalRecordClicked += MainViewModelOnNewMedicalRecordClicked;
            
            _mainWindow.DataContext = _mainViewModel;
            _mainWindow.Show();
            _loginViewModel.DoctorLoggedIn += (_, _) =>
            {
                _loginWindow.Hide();
                _mainViewModel.Refresh();
            };

            _loginWindow.DataContext = _loginViewModel;
            _loginWindow.Owner = _mainWindow;
            _loginWindow.Closing += (_, _) =>
            {
                Current.Shutdown();
            };
            _loginWindow.ShowDialog();

            _newMedicalRecordWindow.Owner = _mainWindow;
        }

        private void NewMedicalRecordViewModelOnWindowClosed(object? sender, EventArgs e)
        {
            _newMedicalRecordWindow.Hide();
            _mainWindow.Show();
            _mainViewModel.Refresh();
        }

        private void MainViewModelOnNewMedicalRecordClicked(object? sender, ReservationDto e)
        {
            _newMedicalRecordViewModelState.Value = new NewMedicalRecordViewModel(_service, e);
            _newMedicalRecordWindow.DataContext = _newMedicalRecordViewModelState.Value;
            _newMedicalRecordWindow.ShowDialog();
        }

        private async void MainViewModelOnLogout(object? sender, EventArgs e)
        {
            try
            {
                await _service.Logout();
                _mainViewModel.Reset();
                _loginWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }
    }
}