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
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void bAuth_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlCommand command = Database.GetCommand("SELECT login, password, role FROM \"Employee\" WHERE (login = @login AND password = @password)");
            command.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, tbAuthLog.Text.Trim());
            command.Parameters.AddWithValue("@password", NpgsqlDbType.Varchar, tbAuthPass.Password.Trim());
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                string role = result.GetString(2);
                NavigationService.Navigate(null);
                MainWindow.grid.Width = new GridLength(2, GridUnitType.Star);
                switch (role)
                {
                    case "Администратор":
                        MainWindow.specsButton.Visibility = Visibility.Visible;
                        MainWindow.groupsButton.Visibility = Visibility.Visible;
                        MainWindow.studButton.Visibility = Visibility.Visible;
                        MainWindow.regButton.Visibility = Visibility.Visible;
                        break;
                    case "Учебная часть":
                        MainWindow.specsButton.Visibility = Visibility.Visible;
                        MainWindow.groupsButton.Visibility = Visibility.Visible;
                        break;
                    case "Приёмная комиссия":
                        MainWindow.studButton.Visibility = Visibility.Visible;
                        break;
                    case "Отдел кадров":
                        MainWindow.regButton.Visibility = Visibility.Visible;
                        break;
                    default:
                        MessageBox.Show("Неверно указан логин или пароль");
                        break;
                }
                result.Close();
                tbAuthLog.Text = null;
                tbAuthPass.Password = null;
            }
        }
    }
}
