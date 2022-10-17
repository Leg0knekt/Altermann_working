using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudRedactorPages
{
    public partial class Speciality : Page
    {
        public Speciality()
        {
            InitializeComponent();
            Binding specBinding = new Binding();
            specBinding.Source = Database.specializations;
            lbSpecList.ItemsSource = Database.specializations;
            LoadSpecs();
            
        }

        private void bAddSpec_Click(object sender, RoutedEventArgs e)
        {
            string specCode = tbSpecCode.Text.Trim();
            string specName = tbSpecName.Text.Trim();
            string specCval = tbSpecCval.Text.Trim();
            if (specCode.Length == 0 || specName.Length == 0 || specCval.Length == 0)
                return;

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Specialization\"(code, name, cvalification) VALUES(@code, @name, @cvalification)");
            try
            {
                command.Parameters.AddWithValue("@code", NpgsqlDbType.Varchar, specCode);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, specName);
                command.Parameters.AddWithValue("@cvalification", NpgsqlDbType.Varchar, specCval);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadSpecs();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Такая специальность уже существует");
            }
            tbSpecCode.Clear();
            tbSpecName.Clear();
            tbSpecCval.Clear();
        }

        private void LoadSpecs()
        {
            Database.specializations.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT code, name, cvalification FROM \"Specialization\" ORDER BY code");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    Database.specializations.Add(new ClSpeciality(result.GetString(0), result.GetString(1), result.GetString(2)));
                }
            }
            result.Close();
        }

        private void bEditSpec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bDelSpec_Click(object sender, RoutedEventArgs e)
        {
            Database.GetCommand("DELETE * FROM \"Specializations\"");
            Database.specializations.Remove(lbSpecList.SelectedItem as ClSpeciality);
        }

        private void lbSpecList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bEditSpec.IsEnabled = lbSpecList.SelectedIndex != -1;
            bDelSpec.IsEnabled = lbSpecList.SelectedIndex != -1;
        }
    }
}
