using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Fluetta.Pages.Settings;
using static Fluetta.Instances;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace Fluetta.Pages
{
    /// <summary>
    /// Логика взаимодействия для Instances.xaml
    /// </summary>
    public partial class Instances : Page
    {
        public Instances()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //ListView.ItemsSource = GetInstanceNames(ListDirs(SettingsData.minecraftPath));
            SetItemsSource(InstanceGrid);
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            Instance sndr = (Instance)((Button)sender).DataContext;
            ModernWpf.Controls.ContentDialog contentDialog = new ModernWpf.Controls.ContentDialog()
            {
                PrimaryButtonText = Properties.Resources.Yes,
                SecondaryButtonText = Properties.Resources.No,
                Title = Properties.Resources.Delete,
                Content = Properties.Resources.DeletionDialog
            };
            ModernWpf.Controls.ContentDialogResult result = await contentDialog.ShowAsync();
            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                Directory.Delete($"{SettingsData.minecraftPath}\\instances\\{sndr.InstanceDir}", true);
                SetItemsSource(InstanceGrid);
                System.Diagnostics.Debug.WriteLine($"[!] {sndr.Name} is successfully deleted");
            }
        }
        public static void SetItemsSource(ModernWpf.Controls.GridView gridView)
        {
            List<Instance> tempList = GetInstanceObjects(SettingsData.minecraftPath);
            ObservableCollection<Instance> Items = new ObservableCollection<Instance>(tempList);
            gridView.ItemsSource = Items;
        }
        private async void EditClick(object sender, RoutedEventArgs e)
        {
            Instance sndr = (Instance)((Button)sender).DataContext;
            EditDialog editDialog = new EditDialog();
            editDialog.Name.Text = sndr.Name;
            editDialog.Version.ItemsSource = MainWindow.versionIds; //new CmlLib.Core.CMLauncher(new CmlLib.Core.MinecraftPath(SettingsData.minecraftPath)).GetAllVersions();
            editDialog.Version.SelectedItem = sndr.VersionId;
            editDialog.FolderName.Text = sndr.InstanceDir;
            editDialog.JavaDir.Text = sndr.JavaDir;
            editDialog.ResX.Text = sndr.ResX;
            editDialog.ResY.Text = sndr.ResY;
            editDialog.JVMArgs.Text = sndr.JVMArgs;
            editDialog.MaxRAM.Text = sndr.MaxRAM;
            ModernWpf.Controls.ContentDialogResult result = await editDialog.ShowAsync();
            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                System.Diagnostics.Debug.WriteLine("[!] Primary button pressed");
                File.WriteAllText($"{SettingsData.minecraftPath}\\instances\\{sndr.InstanceDir}\\instance_settings.json", JsonConvert.SerializeObject(new Instance()
                {
                    Name = editDialog.Name.Text,
                    VersionId = editDialog.Version.Text,
                    Created = sndr.Created,
                    LastUsed = sndr.LastUsed,
                    InstanceDir = editDialog.FolderName.Text,
                    JavaDir = editDialog.JavaDir.Text,
                    ResX = editDialog.ResX.Text,
                    ResY = editDialog.ResY.Text,
                    JVMArgs = editDialog.JVMArgs.Text,
                    MaxRAM = editDialog.MaxRAM.Text
                }
                ));
                if (editDialog.FolderName.Text != sndr.InstanceDir)
                {
                    Directory.Move($"{SettingsData.minecraftPath}\\instances\\{sndr.InstanceDir}", $"{SettingsData.minecraftPath}\\instances\\{editDialog.FolderName.Text}");
                }
                SetItemsSource(InstanceGrid);
            }
        }
        private async void NewInstanceClick(object sender, RoutedEventArgs e)
        {
            EditDialog createDialog = new EditDialog();
            createDialog.Title = Properties.Resources.CreateInstance;
            createDialog.PrimaryButtonText = Properties.Resources.Create;
            createDialog.Version.ItemsSource = MainWindow.versionIds;
            createDialog.Version.SelectedItem = MainWindow.latestRelease;
            ModernWpf.Controls.ContentDialogResult result = await createDialog.ShowAsync();
            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                if (string.IsNullOrEmpty(createDialog.FolderName.Text))
                {
                    createDialog.FolderName.Text = createDialog.Name.Text;
                }
                if (string.IsNullOrEmpty(createDialog.Name.Text))
                {
                    ModernWpf.Controls.ContentDialog warningDialog = new ModernWpf.Controls.ContentDialog();
                    warningDialog.Title = Properties.Resources.Attention;
                    warningDialog.Content = Properties.Resources.NameIsEmpty;
                    warningDialog.PrimaryButtonText = Properties.Resources.Ok;
                    await warningDialog.ShowAsync();
                    createDialog.Closing -= null;
                }
                else
                {
                    Instance instance = new Instance
                    {
                        Name = createDialog.Name.Text,
                        VersionId = createDialog.Version.Text,
                        Created = DateTime.Now,
                        LastUsed = DateTime.Now,
                        InstanceDir = createDialog.FolderName.Text,
                        JavaDir = createDialog.JavaDir.Text,
                        ResX = createDialog.ResX.Text,
                        ResY = createDialog.ResY.Text,
                        JVMArgs = createDialog.JVMArgs.Text,
                        MaxRAM = createDialog.MaxRAM.Text
                    };
                    Directory.CreateDirectory($"{SettingsData.minecraftPath}\\instances\\{instance.InstanceDir}");
                    File.WriteAllText($"{SettingsData.minecraftPath}\\instances\\{instance.InstanceDir}\\instance_settings.json", JsonConvert.SerializeObject(instance));
                    SetItemsSource(InstanceGrid);
                }
            }
        }

    }
}
