using DentalClinicApp.Data;
using DentalClinicApp.Models;
using DentalClinicApp.Utils;
using DentalClinicApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DentalClinicApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static readonly ILogger Logger = Log.ForContext<MainViewModel>();
        public ObservableCollection<Patient> Patients { get; set; } = new();
        public ObservableCollection<PhotoViewModel> Photos { get; set; } = new();

        private Patient? _selectedPatient;
        public Patient? SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (SetProperty(ref _selectedPatient, value))
                    LoadPhotos();
            }
        }


        private string _newPatientName = "";
        public string NewPatientName
        {
            get => _newPatientName;
            set => SetProperty(ref _newPatientName, value);
        }

        public ICommand AddPatientCommand { get; }
        public ICommand AddPhotoCommand { get; }
        public ICommand DropFilesCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            return true;
        }

        private readonly AppDbContext _db;
        private readonly ImageService _imageService;

        
        public MainViewModel(AppDbContext db, ImageService imageService)
        {
            _db = db;
            _imageService = imageService;

            AddPatientCommand = new RelayCommand(AddPatient);
            AddPhotoCommand = new RelayCommand(async _ => await SelectAndAddPhoto());
            DropFilesCommand = new RelayCommand(async files => await AddDroppedFiles((string[])files));


            LoadPatients();
        }
        private void LoadPatients()
        {
            Patients.Clear();
            foreach (var p in _db.Patients.Include(p => p.Photos))
                Patients.Add(p);
        }

        private void LoadPhotos()
        {
            Photos.Clear();
            if (_selectedPatient == null)
                return;
            Logger.Information($"Загрузка фотографий для пациента Id={_selectedPatient.Id} ({_selectedPatient.FullName})");

            var baseDir = _imageService.GetPatientImageDirectory(_selectedPatient.Id);
            foreach (var p in _db.PatientPhotos
                                 .Where(ph => ph.PatientId == _selectedPatient.Id)
                                 .OrderByDescending(ph => ph.UploadedAt))
            {
                var path = Path.Combine(baseDir, p.FileName);
                if (File.Exists(path))
                {
                    Photos.Add(new PhotoViewModel
                    {
                        FilePath = Path.GetFullPath(path),
                        UploadedAt = p.UploadedAt
                    });
                }
            }
        }

        private void AddPatient(object? param)
        {
            var dialog = new AddPatientDialog();
            var result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(dialog.PatientName))
            {
                var p = new Patient { FullName = dialog.PatientName };
                _db.Patients.Add(p);
                _db.SaveChanges();
                Patients.Add(p);
                Logger.Information($"Добавлен новый пациент: {p.FullName} (Id: {p.Id})");
            }
        }

        private async Task SelectAndAddPhoto()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Multiselect = true
            };

            if (dlg.ShowDialog() == true)
                await AddDroppedFiles(dlg.FileNames);
        }

        private async Task AddDroppedFiles(string[] files)
        {
            if (_selectedPatient == null)
                return;

            foreach (var file in files)
            {
                var savedFileName = await _imageService.SaveResizedAsync(file, _selectedPatient.Id);

                var photo = new PatientPhoto
                {
                    PatientId = _selectedPatient.Id,
                    FileName = savedFileName,
                    UploadedAt = DateTime.Now
                };

                _db.PatientPhotos.Add(photo);
            }
            Logger.Information($"Загружены фото для пациента Id={_selectedPatient.Id}");

            await _db.SaveChangesAsync();
            LoadPhotos();
        }
    }

}
