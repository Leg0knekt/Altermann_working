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

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Specialization\"(code, name, qualification) VALUES(@code, @name, @qualification)");
            try
            {
                command.Parameters.AddWithValue("@code", NpgsqlDbType.Varchar, specCode);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, specName);
                command.Parameters.AddWithValue("@qualification", NpgsqlDbType.Varchar, specCval);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadSpecs();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Такая специальность уже существует");
            }
            tbSpecCode.Clear();
            tbSpecName.Clear();
            tbSpecCval.Clear();
        }

        private void LoadSpecs()
        {
            Database.specializations.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT id, code, name, qualification FROM \"Specialization\" ORDER BY code");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    Database.specializations.Add(new ClSpeciality(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetString(3)));
                }
            }
            result.Close();
        }

        private void bEditSpec_Click(object sender, RoutedEventArgs e)
        {
            int specId = (lbSpecList.SelectedItem as ClSpeciality).Id;
            string specCode = tbSpecCode.Text.Trim();
            string specName = tbSpecName.Text.Trim();
            string specQual = tbSpecCval.Text.Trim();
            if (specCode.Length == 0 || specName.Length == 0 || specQual.Length == 0)
                return;

            NpgsqlCommand command = Database.GetCommand("UPDATE \"Specialization\" SET code=@code, name=@name, qualification=@qualification WHERE id=@id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, specId);
                command.Parameters.AddWithValue("@code", NpgsqlDbType.Varchar, specCode);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, specName);
                command.Parameters.AddWithValue("@qualification", NpgsqlDbType.Varchar, specQual);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Такая группа уже существует");
            }
            tbSpecCode.Clear();
            tbSpecName.Clear();
            lbSpecList.SelectedItem = null;
        }

        private void bDelSpec_Click(object sender, RoutedEventArgs e)
        {
            ClSpeciality specForDel = lbSpecList.SelectedItem as ClSpeciality;
            NpgsqlCommand command = Database.GetCommand("DELETE FROM \"Specialization\" WHERE code = @code AND name = @name AND qualification = @qualification");
            try
            {
                command.Parameters.AddWithValue("@code", NpgsqlDbType.Varchar, specForDel.Code);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, specForDel.Specname);
                command.Parameters.AddWithValue("@qualification", NpgsqlDbType.Varchar, specForDel.Qualification);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    Database.specializations.Remove(lbSpecList.SelectedItem as ClSpeciality);
                    MessageBox.Show("Successful deletion!");
                    LoadSpecs();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Что-то пошло не так");
            }
            lbSpecList.SelectedItem = null;
        }

        private void lbSpecList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bEditSpec.IsEnabled = lbSpecList.SelectedIndex != -1;
            bDelSpec.IsEnabled = lbSpecList.SelectedIndex != -1;
            bSpecCancel.IsEnabled = lbSpecList.SelectedIndex != -1;
            bAddSpec.IsEnabled = lbSpecList.SelectedIndex == -1;
        }

        private void bSpecCancel_Click(object sender, RoutedEventArgs e)
        {
            lbSpecList.SelectedItem = null;
        }
    }
}
