using System;
using System.Windows;

namespace Hospital.Desktop.Model.Util;

public static class Error
{
    public static void Show(Exception ex)
    {
        MessageBox.Show($"Hiba történt: {ex.Message}");
    }
}