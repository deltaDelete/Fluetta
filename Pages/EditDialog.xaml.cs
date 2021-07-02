using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernWpf.Controls;
using static Fluetta.Pages.Settings;

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
            ResX.Text = SettingsData.resX;
            ResY.Text = SettingsData.resY;
        }
    }
}
