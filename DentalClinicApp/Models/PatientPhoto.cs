using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Models
{
  public class PatientPhoto
  {
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }

    public Patient? Patient { get; set; }
  }
}
