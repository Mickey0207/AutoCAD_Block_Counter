using System.Windows;

namespace NameFormatDialog;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string OldFileName => OldFileNameTextBox.Text;
    public string NewExcelName => NewExcelNameTextBox.Text;
    public bool IsConfirmed { get; private set; } = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        IsConfirmed = true;
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        IsConfirmed = false;
        DialogResult = false;
        Close();
    }
}