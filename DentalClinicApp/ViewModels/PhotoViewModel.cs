namespace DentalClinicApp.ViewModels
{
    class PhotoViewModel
    {
        public string FilePath { get; set; } = string.Empty; // полный путь к файлу
        public DateTime UploadedAt { get; set; }
    }
}
