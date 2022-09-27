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
            string groupSpec = (lbGroupSpec.SelectedItem as ClSpeciality).Code;
            string groupNum = tbGroupNum.Text.Trim();
            string groupCourse = tbGroupCourse.Text.Trim();
            if (groupNum.Length == 0 || groupCourse.Length == 0 || lbGroupSpec.SelectedItem == null)
                return;

            NpgsqlCommand command = Database.GetCommand("INSERT INTO \"Group\" (number, course, specialization) VALUES(@number, @course, @specialization)");
            try
            {

                command.Parameters.AddWithValue("@number", NpgsqlDbType.Varchar, groupNum);
                command.Parameters.AddWithValue("@course", NpgsqlDbType.Integer, int.Parse(groupCourse));
                command.Parameters.AddWithValue("@specialization", NpgsqlDbType.Varchar, groupSpec);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                    LoadGroups();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Такая группа уже существует ");
            }
            tbGroupNum.Clear();
            tbGroupCourse.Clear();
            lbGroupSpec.SelectedItem = null;
        }

        private void LoadGroups()
        {
            Database.groups.Clear();
            NpgsqlCommand command = Database.GetCommand("SELECT number, course, specialization FROM \"Group\" ORDER BY number");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    string spec = result.GetString(2);
                    ClSpeciality s = Database.specializations.Where(x => x.Code == spec).First();
                    Database.groups.Add(new ClGroup(result.GetString(0), result.GetInt32(1), s));
                }
            }
            result.Close();
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
    }
}
