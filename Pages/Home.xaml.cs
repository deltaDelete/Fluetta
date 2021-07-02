using CmlLib.Core;
using CmlLib.Core.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Fluetta.Auth;
using static Fluetta.Pages.Settings;

namespace Fluetta.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void PlayClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@".\login_settings.json"))
            {
                ProgressBar.Visibility = Visibility.Visible;
                PlayBtn.IsEnabled = false;
                PlayBtn.Content = null;
                string instance_path = $"{Fluetta.Instances.GetFolderByName(ComboBox.SelectedItem.ToString(), Fluetta.Instances.ListDirs(SettingsData.minecraftPath))}";
                Fluetta.Instances.Instance selectedProfile = JsonConvert.DeserializeObject<Fluetta.Instances.Instance>(File.ReadAllText($"{instance_path}\\instance_settings.json"));
                selectedProfile.LastUsed = DateTime.Now;
                File.WriteAllText($"{instance_path}\\instance_settings.json", JsonConvert.SerializeObject(selectedProfile));
                var launcher = new CMLauncher(FMinecraftPath.GetPath(SettingsData.minecraftPath, $"{SettingsData.minecraftPath}\\instances\\{selectedProfile.InstanceDir}"));
                launcher.FileChanged += Launcher_FileChanged;
                launcher.ProgressChanged += Launcher_ProgressChanged;
                var launchOption = new MLaunchOption
                {
                    MaximumRamMb = (!string.IsNullOrEmpty(selectedProfile.MaxRAM)) ? int.Parse(selectedProfile.MaxRAM) : int.Parse(SettingsData.maxRAM),
                    Session = JsonConvert.DeserializeObject<SessionData>(File.ReadAllText(@".\login_settings.json")).Session,
                    ScreenWidth = (!string.IsNullOrEmpty(selectedProfile.ResX)) ? int.Parse(selectedProfile.ResX) : int.Parse(SettingsData.resX),
                    ScreenHeight = (!string.IsNullOrEmpty(selectedProfile.ResY)) ? int.Parse(selectedProfile.ResY) : int.Parse(SettingsData.resY),
                };
                launchOption.JavaPath = (!string.IsNullOrEmpty(selectedProfile.JavaDir)) ? selectedProfile.JavaDir : SettingsData.javaPath != "default" ? SettingsData.javaPath : launchOption.JavaPath;
                launchOption.JVMArguments = (!string.IsNullOrEmpty(selectedProfile.JVMArgs)) ? selectedProfile.JVMArgs.Split(" ") : (!string.IsNullOrEmpty(SettingsData.jvmArgs)) ? SettingsData.jvmArgs.Split(" ") : launchOption.JVMArguments;
                var process = await launcher.CreateProcessAsync(selectedProfile.VersionId, launchOption);

                process.Start();
                ProgressBar.Visibility = Visibility.Hidden;
                ProgressBar.IsIndeterminate = true;
                PlayBtn.IsEnabled = true;
                PlayBtn.Content = Properties.Resources.PlayBtn;
            }
            else
            {
                Account account = new Account();
                account.Show();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /* Getting versions list
            var path = new MinecraftPath(SettingsData.minecraftPath);
            var launcher = new CMLauncher(path);
            List<string> vervar = new List<string> { null };
            CmlLib.Core.Version.MVersionCollection versions = await launcher.GetAllVersionsAsync();
            foreach (var item in versions)
            {
                vervar.Add(item.Name);
            }
            ComboBox.ItemsSource = vervar;
            */
            ComboBox.ItemsSource = Fluetta.Instances.GetInstanceNames(Fluetta.Instances.ListDirs(SettingsData.minecraftPath));
            if (!File.Exists(@"selected_profile.txt"))
            {
                ComboBox.SelectedItem = ComboBox.Items.GetItemAt(0);
                System.Diagnostics.Debug.WriteLine("[!] No selected_profile.txt found, first item");
            }
            else
            {
                ComboBox.SelectedItem = File.ReadAllText(@"selected_profile.txt");
            }
            ProgressBar.Visibility = Visibility.Hidden;
            PlayBtn.IsEnabled = true;
            PlayBtn.Content = Properties.Resources.PlayBtn;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            File.WriteAllText(@"selected_profile.txt", ComboBox.SelectedItem.ToString());
            System.Diagnostics.Debug.WriteLine("[!] Profile selected");
        }
        // Event Handler. Show download progress
        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[!] Download progress - {e}");
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Maximum = 100;
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void Launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[!] File changed progress - {e}");
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Maximum = e.TotalFileCount;
            ProgressBar.Value = e.ProgressedFileCount;
        }
    }
}
