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
using Microsoft.Win32;
using System.IO;

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for Students.xaml
    /// </summary>
    public partial class Students : Window
    {
        public string[] filter_data = { "id", "firstname", "lastname", "birthday", "age", "gender", "house" };
        int count;


        public class Student
        {
            public int Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Birthday { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string House { get; set; }
        }


        public Students()
        {
            InitializeComponent();
            cbxSearchFilter.ItemsSource = filter_data;
        }


        private void DisplayStudents(int count)
        {
            lstStudents.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                var student = new Student()
                {
                    Id = MySql.student[i].id,
                    Firstname = MySql.student[i].firstname,
                    Lastname = MySql.student[i].lastname,
                    Birthday = MySql.student[i].birthday,
                    Age = MySql.student[i].age,
                    Gender = MySql.student[i].gender,
                    House = Sentral.houseData[MySql.student[i].house_id - 1].house_name
                };
                lstStudents.Items.Add(student);
            }
        }


        private void btnBack_Click(object sender, RoutedEventArgs e) { this.Close(); }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cbxSearchFilter.SelectedItem.ToString() == "house")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Sentral.houseData[i].house_name.Contains(tbxSearch.Text) == true)
                    {
                        MySql.StudentSearchQuery($"{cbxSearchFilter.SelectedItem}_id", $"{i + 1}");
                        DisplayStudents(MySql.student.Count);
                        return;
                    }
                }
                MySql.StudentSearchQuery($"{cbxSearchFilter.SelectedItem}_id", "0");
                DisplayStudents(MySql.student.Count);
            }
            else
            {
                MySql.StudentSearchQuery(cbxSearchFilter.SelectedItem.ToString(), tbxSearch.Text);
                DisplayStudents(MySql.student.Count);
            }
        }


        private void cbxSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxSearch.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }


        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(true);
            form.ShowDialog();
            MySql.SelectStudents();
            DisplayStudents(MySql.student_count);
        }


        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(false);
            form.ShowDialog();
            MySql.SelectStudents();
            DisplayStudents(MySql.student_count);
        }


        private void btnInsertStudents_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openStudentsFile = new OpenFileDialog();
            openStudentsFile.Filter = "csv files (*.csv)|*.csv";
            if (openStudentsFile.ShowDialog() == true)
            {
                string path = openStudentsFile.FileName;
                LoadFile(path);
                for (int i = 0; i < count; i++) { MySql.AddStudent(i); }
                MessageBox.Show($"Added {count} students.");
            }
            MySql.SelectStudents();
            DisplayStudents(MySql.student_count);
            tbxSearch.IsEnabled = false;
            btnSearch.IsEnabled = false;
            cbxSearchFilter.SelectedItem = null;
        }


        void LoadFile(string path)
        {
            var file = File.OpenText(path);         // Opens the file following the path specified by the user
            count = 0;
            file.ReadLine();                        // Skip the headers in the file
            while (!file.EndOfStream)               // Runs until the end of the file
            {
                // Creates variables that read a line from the file and store it as a new line of data using the class 'student'
                var line = file.ReadLine();
                var data = line.Split(',');
                var newline = new MySql.Students();

                newline.firstname = data[1];        // Reads the first value on the current line
                newline.lastname = data[2];         // Reads the seccond value on the current line
                newline.birthday = data[3];
                newline.gender = data[4];
                newline.house_id = int.Parse(data[5]);

                // Saves all data collected to the list Students
                MySql.student.Add(newline);
                count++;
            }
            file.Close();   // Closes the file, we are finished with the file
        }
    }
}