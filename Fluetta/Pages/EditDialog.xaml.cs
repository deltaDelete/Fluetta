using System.Windows;
using ModernWpf.Controls;

namespace Fluetta.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDialog.xaml
    /// </summary>
    public partial class EditDialog : ContentDialog
    {
        public EditDialog()
        {
            InitializeComponent();
        }

        private void BrowseJavaClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog
            {
                DefaultExt = "exe"
            };
            fileDialog.ShowDialog();
            JavaDir.Text = fileDialog.FileName;
        }

        private void ResResetClick(object sender, RoutedEventArgs e)
        {
            ResX.Text = Settings.ResX;
            ResY.Text = Settings.ResY;
        }
    }
}
