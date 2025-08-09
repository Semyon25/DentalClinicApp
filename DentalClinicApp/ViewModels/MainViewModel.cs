using DentalClinicApp.Data;
using DentalClinicApp.Models;
using DentalClinicApp.Utils;
using DentalClinicApp.Views;
using Microsoft.Win32;
using Serilog;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace DentalClinicApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private static readonly ILogger Logger = Log.ForContext<MainViewModel>();

        private readonly PatientRepository _repo;
        private readonly ImageService _imageService;

        public ObservableCollection<Patient> Patients { get; set; } = new();
        public ObservableCollection<PhotoViewModel> Photos { get; set; } = new();

        private Patient? _selectedPatient;
        public Patient? SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (SetProperty(ref _selectedPatient, value))
                    _ = LoadPhotos();
            }
        }

        private string _newPatientName = "";
        public string NewPatientName
        {
            get => _newPatientName;
            set => SetProperty(ref _newPatientName, value);
        }

        private string _statusText = "";
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private bool _hasConnection = false;
        public bool HasConnection
        {
            get => _hasConnection;
            set => SetProperty(ref _hasConnection, value);
        }

        public ICommand AddPatientCommand { get; }
        public ICommand AddPhotoCommand { get; }
        public ICommand DropFilesCommand { get; }
        
        public MainViewModel(PatientRepository repo, ImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;

            AddPatientCommand = new RelayCommand(async _ => await AddPatient());
            AddPhotoCommand = new RelayCommand(async _ => await SelectAndAddPhoto());
            DropFilesCommand = new RelayCommand(async files => await AddDroppedFiles(files));
        }

        private async Task LoadPatients()
        {
            Patients.Clear();
            try
            {
                foreach (var p in await _repo.GetAllPatients())
                    Patients.Add(p);
            }
            catch (Exception ex)
            {
                HasConnection = false;
                StatusText = "Ошибка при загрузке списка пациентов";
                Logger.Error(ex, StatusText);
            }
        }

        private async Task LoadPhotos()
        {
            Photos.Clear();
            if (_selectedPatient == null)
                return;
            Logger.Information($"Загрузка фотографий для пациента Id={_selectedPatient.Id} ({_selectedPatient.FullName})");

            var baseDir = _imageService.GetPatientImageDirectory(_selectedPatient.Id);
            foreach (var p in await _repo.GetPhotosByPatientId(_selectedPatient.Id))
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

        private async Task AddPatient()
        {
            var dialog = new AddPatientDialog();
            var result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(dialog.PatientName))
            {
                var p = new Patient { FullName = dialog.PatientName };
                await _repo.AddPatientAsync(p);
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

        private async Task AddDroppedFiles(object? obj)
        {
            if (obj is string[] files)
                await AddDroppedFiles(files);
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

                await _repo.AddPhotoASync(photo);
            }
            Logger.Information($"Загружены фото для пациента Id={_selectedPatient.Id}");

            await LoadPhotos();
        }

        public async Task Initialize()
        {
            StatusText = "Подключение к БД...";
            try
            {
                await _repo.Connect();
            }
            catch (Exception ex)
            {
                StatusText = "Нет подключения";
                Logger.Error(ex, StatusText);
                return;
            }
            StatusText = "Подключено";
            HasConnection = true;

            await LoadPatients();
        }
    }

}
