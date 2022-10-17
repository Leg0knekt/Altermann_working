using Npgsql;
using NpgsqlTypes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StudRedactorPages
{
    public partial class Student : Page
    {
        public Student()
        {
            InitializeComponent();
            lbStudGroup.SetBinding(ListBox.ItemsSourceProperty, new Binding() { Source = Database.groups });
            lbStudList.SetBinding(ListBox.ItemsSourceProperty, new Binding() { Source = Database.students });
            LoadSpecs();
            LoadGroups();
            LoadStudents();
        }

        private void bAddStud_Click(object sender, RoutedEventArgs e)
        {
            string studID = tbStudID.Text.Trim();
            string studSurname = tbStudSurname.Text.Trim();
            string studName = tbStudName.Text.Trim();
            string studPatronymic = tbStudPatronymic.Text.Trim();
            int studGroup = (lbStudGroup.SelectedItem as ClGroup).Id;
            if (studID.Length == 0 || studSurname.Length == 0 || studName.Length == 0 || lbStudGroup.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Student\"(\"ID\", surname, name, patronymic, \"group\") VALUES(@ID, @surname, @name, @patronymic, @group)");
            try
            {
                command.Parameters.AddWithValue("@ID", NpgsqlDbType.Varchar, studID);
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, studSurname);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, studName);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, studPatronymic);
                command.Parameters.AddWithValue("@group", NpgsqlDbType.Integer, studGroup);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadStudents();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Такой студент уже существует");
            }
            tbStudID.Clear();
            tbStudSurname.Clear();
            tbStudName.Clear();
            tbStudPatronymic.Clear();
            lbStudGroup.SelectedItem = null;
        }

        private void LoadStudents()
        {
            Database.students.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT \"ID\", surname, name, patronymic, number, id FROM \"Student\", \"Group\" WHERE \"group\"=id ORDER BY \"ID\"");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    Database.students.Add(new ClStudent(result.GetString(0), result.GetString(1), result.GetString(2), result.GetString(3), result.GetString(4), result.GetInt32(5)));
                }
            }
            result.Close();
        }

        private void LoadGroups()
        {
            Database.groups.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT id, number, course, specialization FROM \"Group\" ORDER BY number");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    int spec = result.GetInt32(3);
                    ClSpeciality s = Database.specializations.Where(x => x.Id == spec).First();
                    Database.groups.Add(new ClGroup(result.GetInt32(0), result.GetString(1), result.GetInt32(2), s));
                }
                result.Close();
            }
            result.Close();
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

        private void bEditStud_Click(object sender, RoutedEventArgs e)
        {
            string studID = tbStudID.Text.Trim();
            string studSurname = tbStudSurname.Text.Trim();
            string studName = tbStudName.Text.Trim();
            string studPatronymic = tbStudPatronymic.Text.Trim();
            int studGroup = (lbStudGroup.SelectedItem as ClGroup).Id;
            if (studID.Length == 0 || studSurname.Length == 0 || studName.Length == 0 || lbStudGroup.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("UPDATE \"Student\" SET surname=@surname, name=@name, patronymic=@patronymic, \"group\"=@group WHERE \"ID\"=@ID");
            try
            {
                command.Parameters.AddWithValue("@ID", NpgsqlDbType.Varchar, studID);
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, studSurname);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, studName);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, studPatronymic);
                command.Parameters.AddWithValue("@group", NpgsqlDbType.Integer, studGroup);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadStudents();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Я не смог");
            }
            tbStudID.Clear();
            tbStudSurname.Clear();
            tbStudName.Clear();
            tbStudPatronymic.Clear();
            lbStudGroup.SelectedItem = null;
            lbStudList.SelectedIndex = -1;
        }

        private void bDeleteStud_Click(object sender, RoutedEventArgs e)
        {
            ClStudent studForDel = lbStudList.SelectedItem as ClStudent;
            NpgsqlCommand command = Database.GetCommand("DELETE FROM \"Student\" WHERE \"ID\" = @id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, studForDel.ID);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    Database.students.Remove(lbStudList.SelectedItem as ClStudent);
                    MessageBox.Show("Successful deletion!");
                    LoadStudents();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Что-то пошло не так");
            }
            lbStudList.SelectedItem = null;
        }

        private void lbStudList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bEditStud.IsEnabled = lbStudList.SelectedIndex != -1;
            bDeleteStud.IsEnabled = lbStudList.SelectedIndex != -1;
            bStudCancel.IsEnabled = lbStudList.SelectedIndex != -1;
            bAddStud.IsEnabled = lbStudList.SelectedIndex == -1;

            if (lbStudList.SelectedIndex == -1)
                return;
            var jojo = lbStudList.SelectedItem as ClStudent;
            for (int i = 0; i < lbStudGroup.Items.Count; i++)
            {

                if ((lbStudGroup.Items[i] as ClGroup).Id == jojo.GroupID)
                {
                    lbStudGroup.SelectedIndex = i;
                    return;
                }
            }
        }

        private void bStudCancel_Click(object sender, RoutedEventArgs e)
        {
            lbStudList.SelectedItem = null;
            lbStudGroup.SelectedItem = null;
        }
    }
}
