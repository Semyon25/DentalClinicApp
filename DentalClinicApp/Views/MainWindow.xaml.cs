using DentalClinicApp.ViewModels;
using Serilog;
using System.Windows;

namespace DentalClinicApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILogger Logger = Log.ForContext<MainWindow>();
        private MainViewModel ViewModel => (MainViewModel)DataContext;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PhotoDropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private void PhotoDropZone_Drop(object sender, DragEventArgs e)
        {
            if (ViewModel.SelectedPatient == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ViewModel.DropFilesCommand.Execute(files);
                Logger.Information($"Фото добавлено через Drag&Drop для пациента Id={ViewModel.SelectedPatient.Id}");
            }
        }
    }
}