using System.Windows;

namespace Hospital.Desktop.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        PasswordBox1.Password = "";
        TextBox1.Text = "";
    }
}