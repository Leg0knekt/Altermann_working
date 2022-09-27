using Npgsql;
using NpgsqlTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudRedactorPages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            cbRegRole.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = Database.employees });
            NpgsqlCommand command = Database.GetCommand("SELECT role FROM \"Roles\"");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                Database.positions.Add(result.GetString(0));
                cbRegRole.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = Database.positions });
            }
            result.Close();
        }

        private void bReg_Click(object sender, RoutedEventArgs e)
        {
            string regSurname = tbRegSurname.Text.Trim();
            string regName = tbRegName.Text.Trim();
            string regPatronymic = tbRegPatronymic.Text.Trim();
            string regLogin = tbRegLog.Text.Trim();
            string regPass = pbRegPass.Password;
            string regRole = cbRegRole.SelectedItem as string;
            if (regSurname.Length == 0 || 
                regName.Length == 0 || 
                regLogin.Length == 0 ||
                regPass.Length == 0)
                return;


            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Employee\"(surname, name, patronymic, login, password, role) VALUES(@surname, @name, @patronymic, @login, @password, @role)");
            try
            {
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, regSurname);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, regName);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, regPatronymic);
                command.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, regLogin);
                command.Parameters.AddWithValue("@password", NpgsqlDbType.Varchar, regPass);
                command.Parameters.AddWithValue("@role", NpgsqlDbType.Varchar, regRole);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                }
            }
            catch (Exception k)
            {
                MessageBox.Show("Not success :("+k);
            }
            tbRegSurname.Clear();
            tbRegName.Clear();
            tbRegPatronymic.Clear();
            tbRegLog.Clear();
            pbRegPass.Clear();
            cbRegRole.SelectedItem = null;
        }
    }
}
