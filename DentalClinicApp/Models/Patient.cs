using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Models
{
  public class Patient
  {
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    public List<PatientPhoto> Photos { get; set; } = new();
  }
}
