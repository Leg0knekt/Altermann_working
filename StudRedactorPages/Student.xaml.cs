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
    public partial class Student : Page
    {
        public Student()
        {
            InitializeComponent();
            LoadGroups();
            LoadStudents();
        }

        private void bAddStud_Click(object sender, RoutedEventArgs e)
        {
            string studID = tbStudID.Text.Trim();
            string studSurname = tbStudSurname.Text.Trim();
            string studName = tbStudName.Text.Trim();
            string studPatronymic = tbStudPatronymic.Text.Trim();
            string studGroup = cbStudGroup.SelectedItem as string;
            if (studID.Length == 0 || studSurname.Length == 0 || studPatronymic.Length == 0 || cbStudGroup.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Student\"(\"ID\", surname, name, patronymic, \"group\") VALUES(@ID, @surname, @name, @patronymic, @group)");
            try
            {
                command.Parameters.AddWithValue("@ID", NpgsqlDbType.Varchar, studID);
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, studSurname);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, studName);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, studPatronymic);
                command.Parameters.AddWithValue("@group", NpgsqlDbType.Varchar, studGroup);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadStudents();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Такой студент уже существует");
            }
            tbStudID.Clear();
            tbStudSurname.Clear();
            tbStudName.Clear();
            tbStudPatronymic.Clear();
            cbStudGroup.SelectedItem = null;
        }

        private void LoadStudents()
        {
            Database.students.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT * FROM \"Student\" ORDER BY \"ID\"");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    string group = result.GetString(4);
                    Database.students.Add(new ClStudent(result.GetString(0), result.GetString(1), result.GetString(2), result.GetString(3), group));
                }
            }
            result.Close();
        }

        private void LoadGroups()
        {
            NpgsqlCommand command = Database.GetCommand("SELECT number, course, specialization FROM \"Group\" ORDER BY number");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    cbStudGroup.Items.Add(result.GetString(0));
                }
            }
            result.Close();
        }
    }
}
