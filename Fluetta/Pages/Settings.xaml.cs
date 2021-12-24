using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ModernWpf;
using Newtonsoft.Json;
using SamplesCommon;

namespace Fluetta.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        int onLoadIndex;
        int changedIndex;
        public Settings()
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
            SettingsData.theme = GetTheme();
            SettingsData.accent = ThemeManager.Current.ActualAccentColor.ToString();
            SettingsData.language = ComboBox.SelectedIndex == 1 ? "en-US" : ComboBox.SelectedIndex == 2 ? "ru-RU" : System.Globalization.CultureInfo.CurrentCulture.ToString();
            SettingsData.minecraftPath = PathBox.Text;
            SettingsData.resX = ResX.Text;
            SettingsData.resY = ResY.Text;
            SettingsData.javaPath = JavaPath.Text;
            SettingsData.jvmArgs = JVMArgs.Text;
            if (onLoadIndex != changedIndex) 
            {
                ModernWpf.Controls.ContentDialog contentDialog = new ModernWpf.Controls.ContentDialog()
                {
                    PrimaryButtonText = Properties.Resources.Ok,
                    Title = Properties.Resources.Attention,
                    Content = Properties.Resources.ContextDialogApplied
                };
                await contentDialog.ShowAsync(); 
            }
            File.WriteAllText(@".\launcher_settings.json", JsonConvert.SerializeObject(SettingsData.ToObject()));
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

        public static class SettingsData
        {
            public static string accent = "#FF0078D7";
            public static string theme = "systemdefault";
            public static string language = "en-US";
            public static string minecraftPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.fluetta";
            public static string resX = "856";
            public static string resY = "482";
            public static string javaPath = "default";
            public static string jvmArgs;
            public static string maxRAM = "2048";
            public class SettingsDataObject
            {
                public string ThemeData { get; set; }
                public string AccentData { get; set; }
                public string LanguageData { get; set; }
                public string MinecraftPathData { get; set; }
                public string ResX { get; set; }
                public string ResY { get; set; }
                public string JavaPath { get; set; }
                public string JVMArgs { get; set; }
                public string MaxRAM { get; set; }
            }

            public static SettingsDataObject ToObject()
            {
                SettingsDataObject settings = new SettingsDataObject()
                {
                    ThemeData = theme,
                    AccentData = accent,
                    LanguageData = language,
                    MinecraftPathData = minecraftPath,
                    ResX = resX,
                    ResY = resY,
                    JavaPath = javaPath,
                    JVMArgs = jvmArgs,
                    MaxRAM = maxRAM
                };
                return settings;
            }
            public static void SetFromObject(SettingsDataObject settings)
            {
                accent = settings.AccentData;
                theme = settings.ThemeData;
                language = settings.LanguageData;
                minecraftPath = settings.MinecraftPathData;
                resX = settings.ResX;
                resY = settings.ResY;
                jvmArgs = settings.JVMArgs;
                maxRAM = settings.MaxRAM;
                SetTheme(settings.ThemeData);
                SetAccent(settings.AccentData);
                if (settings.LanguageData != "systemdefault")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(settings.LanguageData);
                }
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedIndex = SettingsData.language == "en-US" ? 1 : SettingsData.language == "ru-RU" ? 2 : 0;
            onLoadIndex = ComboBox.SelectedIndex;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changedIndex = ComboBox.SelectedIndex;
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.fluetta"
            };
            folderDialog.ShowDialog();
            PathBox.Text = folderDialog.SelectedPath;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PathBox.Text = SettingsData.minecraftPath;
            ResX.Text = SettingsData.resX;
            ResY.Text = SettingsData.resY;
            JavaPath.Text = SettingsData.javaPath;
            JVMArgs.Text = SettingsData.jvmArgs;
            MAXRAM.Text = SettingsData.maxRAM;
            Version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void ResetResolution(object sender, RoutedEventArgs e)
        {
            ResX.Text = "856";
            ResY.Text = "482";
        }
        private void ResetPath(object sender, RoutedEventArgs e)
        {
            PathBox.Text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.fluetta";
        }
        private void ResetJavaPath(object sender, RoutedEventArgs e)
        {
            JavaPath.Text = "default";
        }
        private void BrowseJavaClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog
            {
                DefaultExt = "exe"
            };
            fileDialog.ShowDialog();
            JavaPath.Text = fileDialog.FileName;
        }
    }
}
