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
    
    public partial class Group : Page
    {
        
        public Group()
        {
            InitializeComponent();
            lbGroupList.SetBinding(ListBox.ItemsSourceProperty, new Binding() { Source = Database.groups });
            lbGroupSpec.SetBinding(ListBox.ItemsSourceProperty, new Binding() { Source = Database.specializations });
            LoadSpecs();
            LoadGroups();
            
        }

        private void bAddGroup_Click(object sender, RoutedEventArgs e)
        {
            int groupSpec = (lbGroupSpec.SelectedItem as ClSpeciality).Id;
            string groupNum = tbGroupNum.Text.Trim();
            string groupCourse = tbGroupCourse.Text.Trim();
            if (groupNum.Length == 0 || groupCourse.Length == 0 || lbGroupSpec.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Group\" (number, course, specialization) VALUES(@number, @course, @specialization)");
            try
            {

                command.Parameters.AddWithValue("@number", NpgsqlDbType.Varchar, groupNum);
                command.Parameters.AddWithValue("@course", NpgsqlDbType.Integer, int.Parse(groupCourse));
                command.Parameters.AddWithValue("@specialization", NpgsqlDbType.Integer, groupSpec);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadGroups();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Такая группа уже существует");
            }
            tbGroupNum.Clear();
            tbGroupCourse.Clear();
            lbGroupSpec.SelectedItem = null;
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

        private void bGroupDelete_Click(object sender, RoutedEventArgs e)
        {
            ClGroup groupForDel = lbGroupList.SelectedItem as ClGroup;
            NpgsqlCommand command = Database.GetCommand("DELETE FROM \"Group\" WHERE id = @id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, groupForDel.Id);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    Database.groups.Remove(lbGroupList.SelectedItem as ClGroup);
                    MessageBox.Show("Successful deletion!");
                    LoadSpecs();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Что-то пошло не так");
            }
            lbGroupList.SelectedItem = null;
        }

        private void bGroupEdit_Click(object sender, RoutedEventArgs e)
        {
            int groupSpec = (lbGroupSpec.SelectedItem as ClSpeciality).Id;
            int groupId = (lbGroupList.SelectedItem as ClGroup).Id;
            string groupNum = tbGroupNum.Text.Trim();
            string groupCourse = tbGroupCourse.Text.Trim();
            if (groupNum.Length == 0 || groupCourse.Length == 0 || lbGroupSpec.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("UPDATE \"Group\" SET number=@number, course=@course, specialization=@specialization WHERE id=@id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, groupId);
                command.Parameters.AddWithValue("@number", NpgsqlDbType.Varchar, groupNum);
                command.Parameters.AddWithValue("@course", NpgsqlDbType.Integer, int.Parse(groupCourse));
                command.Parameters.AddWithValue("@specialization", NpgsqlDbType.Integer, groupSpec);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadGroups();
                }
            }
            catch (Exception)
            {
                MainWindow.ErrorShow("Error");
            }
            tbGroupNum.Clear();
            tbGroupCourse.Clear();
            lbGroupSpec.SelectedItem = null;
        }

        private void lbGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bGroupEdit.IsEnabled = lbGroupList.SelectedIndex != -1;
            bGroupDelete.IsEnabled = lbGroupList.SelectedIndex != -1;
            bAddGroup.IsEnabled = lbGroupList.SelectedIndex == -1;
            bGroupCancel.IsEnabled = lbGroupList.SelectedIndex != -1;

            if (lbGroupList.SelectedIndex == -1)
                return;
            var jo = lbGroupList.SelectedItem as ClGroup;
            for (int i = 0; i < lbGroupList.Items.Count; i++)
            {

                if ((lbGroupSpec.Items[i] as ClSpeciality).Code == jo.Spec.Code)
                {
                    lbGroupSpec.SelectedIndex = i;
                    return;
                }
            }
        }

        private void bGroupCancel_Click(object sender, RoutedEventArgs e)
        {
            lbGroupList.SelectedItem = null;
            lbGroupSpec.SelectedItem = null;
        }
    }
}
