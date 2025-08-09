using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DentalClinicApp.Views
{
    /// <summary>
    /// Interaction logic for AddPatientDialog.xaml
    /// </summary>
    public partial class AddPatientDialog : Window
    {
        public string? PatientName { get; private set; }

        public AddPatientDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                PatientName = NameTextBox.Text.Trim();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите имя пациента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
