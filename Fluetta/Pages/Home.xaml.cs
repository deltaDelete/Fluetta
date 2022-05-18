using CmlLib.Core;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static Fluetta.Auth;

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
            if (File.Exists($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}login.json"))
            {
                ProgressBar.Visibility = Visibility.Visible;
                PlayBtn.IsEnabled = false;
                PlayBtn.Content = null;
                string instance_path = $"{Fluetta.Instances.GetFolderByName(ComboBox.SelectedItem.ToString(), Fluetta.Instances.ListDirs(Settings.MinecraftPath))}";
                Fluetta.Instances.Instance selectedProfile = JsonConvert.DeserializeObject<Fluetta.Instances.Instance>(File.ReadAllText($"{instance_path}{Path.DirectorySeparatorChar}instance_settings.json"));
                selectedProfile.LastUsed = DateTime.Now;
                File.WriteAllText($"{instance_path}{Path.DirectorySeparatorChar}instance_settings.json", JsonConvert.SerializeObject(selectedProfile));
                var launcher = new CMLauncher(FMinecraftPath.GetPath(Settings.MinecraftPath, $"{Settings.MinecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{selectedProfile.InstanceDir}"));
                launcher.FileChanged += Launcher_FileChanged;
                launcher.ProgressChanged += Launcher_ProgressChanged;
                var launchOption = new MLaunchOption
                {
                    MaximumRamMb = (!string.IsNullOrEmpty(selectedProfile.MaxRAM)) ? int.Parse(selectedProfile.MaxRAM) : int.Parse(Settings.MaxRAM),
                    Session = JsonConvert.DeserializeObject<SessionData>(File.ReadAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}login.json")).Session,
                    ScreenWidth = (!string.IsNullOrEmpty(selectedProfile.ResX)) ? int.Parse(selectedProfile.ResX) : int.Parse(Settings.ResX),
                    ScreenHeight = (!string.IsNullOrEmpty(selectedProfile.ResY)) ? int.Parse(selectedProfile.ResY) : int.Parse(Settings.ResY),
                };
                launchOption.JavaPath = (!string.IsNullOrEmpty(selectedProfile.JavaDir)) ? selectedProfile.JavaDir : Settings.JavaPath != "default" ? Settings.JavaPath : launchOption.JavaPath;
                launchOption.JVMArguments = (!string.IsNullOrEmpty(selectedProfile.JVMArgs)) ? selectedProfile.JVMArgs.Split(" ") : (!string.IsNullOrEmpty(Settings.JVMArgs)) ? Settings.JVMArgs.Split(" ") : launchOption.JVMArguments;
                var process = await launcher.CreateProcessAsync(selectedProfile.VersionId, launchOption);

                process.Start();
                PlayBtn.IsEnabled = true;
                PlayBtn.Content = Properties.Resources.PlayBtn;
                if (Settings.CloseAfterStart)
                    Application.Current.MainWindow.Close();
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
            ComboBox.ItemsSource = Fluetta.Instances.GetInstanceNames(Fluetta.Instances.ListDirs(Settings.MinecraftPath));
            if (!File.Exists($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}selected_profile.txt"))
            {
                ComboBox.ItemsSource = Fluetta.Instances.GetInstanceNames(Fluetta.Instances.ListDirs(Settings.MinecraftPath));
                try
                {
                    ComboBox.SelectedItem = "latestRelease";
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("[!] No selected_profile.txt found, first item");
                }
            }
            else
            {
                ComboBox.SelectedItem = File.ReadAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}selected_profile.txt");
            }
            ProgressBar.Visibility = Visibility.Hidden;
            PlayBtn.IsEnabled = true;
            PlayBtn.Content = Properties.Resources.PlayBtn;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            File.WriteAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}selected_profile.txt", ComboBox.SelectedItem.ToString());
            System.Diagnostics.Debug.WriteLine("[!] Profile selected");
        }
        // Event Handler. Show download progress
        bool isStarting = false;
        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!isStarting)
            {
                Application.Current.MainWindow.TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo
                {
                    ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal
                };
                ProgressBar.Maximum = 100;
                ProgressBar.IsIndeterminate = false;
                isStarting = true;
            }
            Application.Current.MainWindow.TaskbarItemInfo.ProgressValue = e.ProgressPercentage;
            ProgressBar.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100)
            {
                Application.Current.MainWindow.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Hidden;
                isStarting = false;
            }
        }

        bool isDownloading = false;
        private void Launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
        {
            if (!isDownloading)
            {
                FilesProcessedProgressBar.IsIndeterminate = false;
                FilesProcessedProgressBar.Visibility = Visibility.Visible;
                FilesProcessedTextBox.Visibility = Visibility.Visible;
                FilesProcessedProgressBar.Maximum = e.TotalFileCount;
                isDownloading = true;
            }
            FilesProcessedTextBox.Text = $"{Properties.Resources.DownloadingFiles} {e.ProgressedFileCount}/{e.TotalFileCount}";
            FilesProcessedProgressBar.Value = e.ProgressedFileCount;
            if (e.ProgressedFileCount == e.TotalFileCount)
            {
                FilesProcessedProgressBar.IsIndeterminate = true;
                FilesProcessedProgressBar.Visibility = Visibility.Hidden;
                FilesProcessedTextBox.Visibility = Visibility.Hidden;
                isDownloading = false;
            }
        }
    }
}
