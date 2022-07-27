using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        // Create all our lists (Eg. covidData[].date)
        int[] data_birthdayday = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        int[] data_birthdaymonth = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int[] data_birthdayyear = { 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };
        string[] data_gender = { "Male", "Female", "Not Specified" };
        string[] data_house = { Sentral.houseData[0].house_name, Sentral.houseData[1].house_name, Sentral.houseData[2].house_name, Sentral.houseData[3].house_name };
        int[] data_id = new int[MySql.student.Count];
        string path;
        int count;


        public AddStudent(bool edit)
        {
            InitializeComponent();
            cbxstudent_birthday_day.ItemsSource = data_birthdayday;
            cbxstudent_birthday_month.ItemsSource = data_birthdaymonth;
            cbxstudent_birthday_year.ItemsSource = data_birthdayyear;
            cbxstudent_gender.ItemsSource = data_gender;
            cbxstudent_house.ItemsSource = data_house;
            Check(edit);
        }


        public void Reset()
        {
            tbxstudent_firstname.Text = "";
            tbxstudent_lastname.Text = "";
            cbxstudent_birthday_day.SelectedItem = null;
            cbxstudent_birthday_month.SelectedItem = null;
            cbxstudent_birthday_year.SelectedItem = null;
            cbxstudent_gender.SelectedItem = null;
            cbxstudent_house.SelectedItem = null;
            cbxstudent_id.SelectedItem = null;
            MySql.student.Clear();
        }


        private void Check(bool check)
        {
            if (check == true)
            {
                cbxstudent_id.Visibility = Visibility.Visible;
                tbxstudent_firstname.IsEnabled = false;
                tbxstudent_lastname.IsEnabled = false;
                cbxstudent_birthday_day.IsEnabled = false;
                cbxstudent_birthday_month.IsEnabled = false;
                cbxstudent_birthday_year.IsEnabled = false;
                cbxstudent_gender.IsEnabled = false;
                cbxstudent_house.IsEnabled = false;
                for (int i = 0; i < MySql.student.Count; i++)
                {
                    data_id[i] = MySql.student[i].id;
                }
                cbxstudent_id.ItemsSource = data_id;
                btninsert.Visibility = Visibility.Hidden;
            }
        }


        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            if (btnsubmit.Content.ToString() == "Add")
            {
                if (tbxstudent_firstname.Text != "" && tbxstudent_lastname.Text != "" && cbxstudent_birthday_day.SelectedItem != null && cbxstudent_birthday_month.SelectedItem != null && cbxstudent_birthday_year.SelectedItem != null && cbxstudent_gender.SelectedItem != null && cbxstudent_house.SelectedItem != null)
                {
                    MySql.newStudent.Add(new MySql.AddStudent { firstname = $"{tbxstudent_firstname.Text}", lastname = $"{tbxstudent_lastname.Text}", birthday = $"{cbxstudent_birthday_day.SelectedItem}-{cbxstudent_birthday_month.SelectedItem}-{cbxstudent_birthday_year.SelectedItem}", gender = cbxstudent_gender.SelectedItem.ToString(), house_id = (cbxstudent_house.SelectedIndex + 1) });
                    MySql.AddStudent(0);
                    Reset();
                }
                else { MessageBox.Show("Error adding student. Not all boxes had a selected item."); }
            }
            else
            {
                if (tbxstudent_firstname.Text != "" && tbxstudent_lastname.Text != "" && cbxstudent_birthday_day.SelectedItem != null && cbxstudent_birthday_month.SelectedItem != null && cbxstudent_birthday_year.SelectedItem != null && cbxstudent_gender.SelectedItem != null && cbxstudent_house.SelectedItem != null)
                {
                    MySql.newStudent.Add(new MySql.AddStudent { firstname = $"{tbxstudent_firstname.Text}", lastname = $"{tbxstudent_lastname.Text}", birthday = $"{cbxstudent_birthday_day.SelectedItem}-{cbxstudent_birthday_month.SelectedItem}-{cbxstudent_birthday_year.SelectedItem}", gender = cbxstudent_gender.SelectedItem.ToString(), house_id = (cbxstudent_house.SelectedIndex + 1) });
                    int i = cbxstudent_id.SelectedIndex;
                    MySql.student[i].firstname = $"{tbxstudent_firstname.Text}";
                    MySql.student[i].lastname = $"{tbxstudent_lastname.Text}";
                    MySql.student[i].birthday = $"{cbxstudent_birthday_day.SelectedItem}-{cbxstudent_birthday_month.SelectedItem}-{cbxstudent_birthday_year.SelectedItem}";
                    MySql.student[i].gender = cbxstudent_gender.SelectedItem.ToString();
                    MySql.student[i].house_id = int.Parse(cbxstudent_house.SelectedIndex.ToString() + 1);
                    MySql.UpdateStudent(int.Parse(cbxstudent_id.SelectedItem.ToString()));
                    Reset();
                }
                else { MessageBox.Show("Error updating student. Not all boxes had a selected item."); }
            }
        }


        private void btninsert_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openStudentsFile = new OpenFileDialog();
            openStudentsFile.Filter = "csv files (*.csv)|*.csv";
            if (openStudentsFile.ShowDialog() == true)
            {
                path = openStudentsFile.FileName;
                LoadFile();
                for (int i = 0; i < count; i++) { MySql.AddStudent(i); }
                MessageBox.Show($"Added {count} students.");
                Reset();
            }
        }


        void LoadFile()
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

        private void cbxstudent_id_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbxstudent_id.SelectedItem != null)
            {
                int i = int.Parse(cbxstudent_id.SelectedItem.ToString());

                tbxstudent_firstname.Text = MySql.student[i - 1].firstname;
                tbxstudent_lastname.Text = MySql.student[i - 1].lastname;
                cbxstudent_birthday_day.SelectedItem = int.Parse(MySql.student[i - 1].birthday.Split('-')[0]);
                cbxstudent_birthday_month.SelectedItem = int.Parse(MySql.student[i - 1].birthday.Split('-')[1]);
                cbxstudent_birthday_year.SelectedItem = int.Parse(MySql.student[i - 1].birthday.Split('-')[2]);
                cbxstudent_gender.SelectedItem = MySql.student[i - 1].gender;
                cbxstudent_house.SelectedIndex = MySql.student[i - 1].house_id - 1;
                tbxstudent_firstname.IsEnabled = true;
                tbxstudent_lastname.IsEnabled = true;
                cbxstudent_birthday_day.IsEnabled = true;
                cbxstudent_birthday_month.IsEnabled = true;
                cbxstudent_birthday_year.IsEnabled = true;
                cbxstudent_gender.IsEnabled = true;
                cbxstudent_house.IsEnabled = true;
                btnsubmit.Content = "Edit";
            }
        }
    }
}