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
            cbxSearchFilter.ItemsSource = new string[] { "id", "firstname", "lastname", "birthday", "age", "gender", "house" };
            Refresh();
        }


        private void DisplayStudents()
        {
            lstStudents.Items.Clear();
            for (int i = 0; i < MySql.student.Count; i++)
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
        

        void Refresh()
        {
            MySql.SelectStudents();
            DisplayStudents();
        }


        private void btnBack_Click(object sender, RoutedEventArgs e) { Close(); }


        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            cbxSearchFilter.SelectedItem = null;
            tbxSearch.Text = "";
            tbxSearch.IsEnabled = false;
            btnSearch.IsEnabled = false;
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbxSearch.Text != "")
            {
                if (cbxSearchFilter.SelectedItem.ToString() == "house")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Sentral.houseData[i].house_name.Contains(tbxSearch.Text) == true)
                        {
                            MySql.StudentSearchQuery($"{cbxSearchFilter.SelectedItem}_id", $"{i + 1}");
                            DisplayStudents();
                            return;
                        }
                    }
                    MySql.StudentSearchQuery($"{cbxSearchFilter.SelectedItem}_id", "0");
                    DisplayStudents();
                }
                else
                {
                    MySql.StudentSearchQuery(cbxSearchFilter.SelectedItem.ToString(), tbxSearch.Text);
                    DisplayStudents();
                }
            }
            else { MessageBox.Show("No text was entered into the search. Please try again."); }
        }


        private void cbxSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxSearch.Text = "";
            tbxSearch.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }


        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(true);
            form.ShowDialog();
            Refresh();
        }


        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(false);
            form.ShowDialog();
            Refresh();
        }


        private void btnInsertStudents_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openStudentsFile = new OpenFileDialog();
            openStudentsFile.Filter = "csv files (*.csv)|*.csv";
            if (openStudentsFile.ShowDialog() == true)
            {
                string path = openStudentsFile.FileName;
                int count = int.Parse(LoadFile(path));
                for (int i = 365; i < count; i++) { MySql.AddStudent(i); }
                MessageBox.Show($"Added {count} students.");
            }
            Refresh();
        }


        string LoadFile(string path)
        {
            MySql.student.Clear();
            var file = File.OpenText(path);         // Opens the file following the path specified by the user
            int count = 0;
            file.ReadLine();                        // Skip the headers in the file
            while (!file.EndOfStream)               // Runs until the end of the file
            {
                // Creates variables that read a line from the file and store it as a new line of data using the class 'student'
                var line = file.ReadLine();
                var data = line.Split(',');
                var newline = new MySql.Students();

                newline.firstname = data[1];        // Reads the first value on the current line
                newline.lastname = data[2];         // Reads the seccond value on the current line
                newline.birthday = DateTime.Parse(data[3]).ToString().Split(' ')[0];
                newline.gender = data[4];
                newline.house_id = int.Parse(data[5]);

                // Saves all data collected to the list Students
                MySql.student.Add(newline);
                count++;
            }
            file.Close();   // Closes the file, we are finished with the file
            return count.ToString();
        }
    }
}