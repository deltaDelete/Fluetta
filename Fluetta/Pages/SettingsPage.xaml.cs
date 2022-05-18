using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ModernWpf;
using SamplesCommon;

namespace Fluetta.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        int onLoadIndex;
        int changedIndex;
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).ContentFrame.Navigate(typeof(Settings));
            System.Diagnostics.Debug.WriteLine("[!] Cancel pressed");
        }

        private async void ApplyClick(object sender, RoutedEventArgs e)
        {
            Settings.Theme = GetTheme();
            Settings.Accent = ThemeManager.Current.ActualAccentColor.ToString();
            Settings.Language = ComboBox.SelectedIndex == 1 ? "en-US" : ComboBox.SelectedIndex == 2 ? "ru-RU" : System.Globalization.CultureInfo.CurrentCulture.ToString();
            Settings.MinecraftPath = PathBox.Text;
            Settings.ResX = ResX.Text;
            Settings.ResY = ResY.Text;
            Settings.JavaPath = JavaPath.Text;
            Settings.JVMArgs = JVMArgs.Text;
            Settings.MaxRAM = MAXRAM.Text;
            Settings.CloseAfterStart = (bool)CloseAfterStartCheckBox.IsChecked;
            if (onLoadIndex != changedIndex) 
            {
                ModernWpf.Controls.ContentDialog contentDialog = new()
                {
                    PrimaryButtonText = Properties.Resources.Ok,
                    Title = Properties.Resources.Attention,
                    Content = Properties.Resources.ContextDialogApplied
                };
                await contentDialog.ShowAsync(); 
            }
            Settings.Save();
            System.Diagnostics.Debug.WriteLine("[!] Apply pressed");
        }
        public string GetTheme()
        {
            return ThemeMode.SelectedItem == AppTheme.Light ? "light" : ThemeMode.SelectedItem == AppTheme.Dark ? "dark" : "systemdefault";
        }
        public static void SetTheme(string theme = "light")
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                ThemeManager.Current.ApplicationTheme = theme == "light" ? ApplicationTheme.Light : theme == "dark" ? (ApplicationTheme?)ApplicationTheme.Dark : null;
            });
        }
        public static void SetAccent(string accent)
        {
            ThemeManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString(accent);
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedIndex = Settings.Language == "en-US" ? 1 : Settings.Language == "ru-RU" ? 2 : 0;
            onLoadIndex = ComboBox.SelectedIndex;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changedIndex = ComboBox.SelectedIndex;
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog folderDialog = new()
            {
                SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + $"{Path.DirectorySeparatorChar}.fluetta"
            };
            folderDialog.ShowDialog();
            PathBox.Text = folderDialog.SelectedPath;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PathBox.Text = Settings.MinecraftPath;
            ResX.Text = Settings.ResX;
            ResY.Text = Settings.ResY;
            JavaPath.Text = Settings.JavaPath;
            JVMArgs.Text = Settings.JVMArgs;
            MAXRAM.Text = Settings.MaxRAM;
            Version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            CloseAfterStartCheckBox.IsChecked = Settings.CloseAfterStart;
        }

        private void ResetResolution(object sender, RoutedEventArgs e)
        {
            ResX.Text = "856";
            ResY.Text = "482";
        }
        private void ResetPath(object sender, RoutedEventArgs e)
        {
            PathBox.Text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + $"{Path.DirectorySeparatorChar}.fluetta";
        }
        private void ResetJavaPath(object sender, RoutedEventArgs e)
        {
            JavaPath.Text = "default";
        }
        private void BrowseJavaClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog fileDialog = new()
            {
                DefaultExt = "exe"
            };
            fileDialog.ShowDialog();
            JavaPath.Text = fileDialog.FileName;
        }
    }
}
