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
using Npgsql;
using NpgsqlTypes;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace StudRedactorPages
{
    public partial class MainWindow : Window

    {
        public static ColumnDefinition grid;

        public static Button specsButton;
        public static Button groupsButton;
        public static Button studButton;
        public static Button regButton;

        public static Border errorBorder;
        public static TextBlock tbWarning;
        public static DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Database.Connect("10.14.206.27", "5432", "student", "1234", "363AllahIsGreat");
            NpgsqlCommand command = Database.GetCommand("SELECT * FROM \"Employee\"");
            NpgsqlDataReader result = command.ExecuteReader();
            if (!result.HasRows)
            {
                AppFrame.Navigate(PageControl.Registration);
                menuBlock.Width = new GridLength(0);
            }
            else
            {
                AppFrame.Navigate(PageControl.Auth);
                menuBlock.Width = new GridLength(0);
            }
            result.Close();
            grid = menuBlock;

            specsButton = goToSpecs;
            groupsButton = goToGroups;
            studButton = goToStudents;
            regButton = goToReg;
            goToSpecs.Visibility = Visibility.Collapsed;
            goToGroups.Visibility = Visibility.Collapsed;
            goToStudents.Visibility = Visibility.Collapsed;
            goToReg.Visibility = Visibility.Collapsed;

            timer.Interval = new TimeSpan(0, 0, 0, 5);
            timer.Tick += Timer_Tick;
            errorBorder = borderError;
            tbWarning = tbError;
        }

        public static void ErrorShow(string error)
        {
            tbWarning.Text = error;
            errorBorder.Visibility = Visibility.Visible;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tbError.Text = "";
            borderError.Visibility = Visibility.Hidden;
            timer.Stop();
        }

        private void goToSpecs_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.Navigate(PageControl.Speciality);
            goToSpecs.IsEnabled = false;
            goToGroups.IsEnabled = true;
            goToStudents.IsEnabled = true;
            goToReg.IsEnabled = true;
        }

        private void goToGroups_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.Navigate(PageControl.Group);
            goToSpecs.IsEnabled = true;
            goToGroups.IsEnabled = false;
            goToStudents.IsEnabled = true;
            goToReg.IsEnabled = true;
        }

        private void goToStudents_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.Navigate(PageControl.Student);
            goToStudents.IsEnabled = false;
            goToSpecs.IsEnabled = true;
            goToGroups.IsEnabled = true;
            goToReg.IsEnabled = true;
        }

        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.Navigate(PageControl.Registration);
            goToStudents.IsEnabled = true;
            goToSpecs.IsEnabled = true;
            goToGroups.IsEnabled = true;
            goToReg.IsEnabled = false;
        }
    }
}
