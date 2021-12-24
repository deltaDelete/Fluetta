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
        public static ObservableCollection<Instance> Items { get; set; }
        public Instances()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetItemsSource(InstanceGrid);
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            Instance sndr = (Instance)((Button)sender).DataContext;
            ModernWpf.Controls.ContentDialog contentDialog = new()
            {
                PrimaryButtonText = Properties.Resources.Yes,
                SecondaryButtonText = Properties.Resources.No,
                Title = Properties.Resources.Delete,
                Content = Properties.Resources.DeletionDialog
            };
            ModernWpf.Controls.ContentDialogResult result = await contentDialog.ShowAsync();
            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                Directory.Delete($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{sndr.InstanceDir}", true);
                SetItemsSource(InstanceGrid);
                System.Diagnostics.Debug.WriteLine($"[!] {sndr.Name} is successfully deleted");
            }
        }
        public static void SetItemsSource(ModernWpf.Controls.GridView gridView)
        {
            Items = GetInstanceObjects(SettingsData.minecraftPath);
            gridView.ItemsSource = Items;
        }
        private async void EditClick(object sender, RoutedEventArgs e)
        {
            Instance sndr = (Instance)((Button)sender).DataContext;
            EditDialog editDialog = new();
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
                await File.WriteAllTextAsync($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{sndr.InstanceDir}{Path.DirectorySeparatorChar}instance_settings.json", JsonConvert.SerializeObject(new Instance()
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
                    Directory.Move($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{sndr.InstanceDir}", $"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{editDialog.FolderName.Text}");
                }
                SetItemsSource(InstanceGrid);
            }
        }
        private async void NewInstanceClick(object sender, RoutedEventArgs e)
        {
            EditDialog createDialog = new()
            {
                Title = Properties.Resources.CreateInstance,
                PrimaryButtonText = Properties.Resources.Create
            };
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
                    ModernWpf.Controls.ContentDialog warningDialog = new()
                    {
                        Title = Properties.Resources.Attention,
                        Content = Properties.Resources.NameIsEmpty,
                        PrimaryButtonText = Properties.Resources.Ok
                    };
                    await warningDialog.ShowAsync();
                    createDialog.Closing -= null;
                }
                else
                {
                    Instance instance = new()
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
                    Directory.CreateDirectory($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{instance.InstanceDir}");
                    await File.WriteAllTextAsync($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}{instance.InstanceDir}{Path.DirectorySeparatorChar}instance_settings.json", JsonConvert.SerializeObject(instance));
                    SetItemsSource(InstanceGrid);
                }
            }
        }

    }
}
