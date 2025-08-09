using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.ViewModels
{
    public class PhotoViewModel
    {
        public string FilePath { get; set; } = string.Empty; // полный путь к файлу
        public DateTime UploadedAt { get; set; }
    }
}
