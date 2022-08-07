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
        int[] data_id = new int[MySql.student_count];


        public AddStudent(bool edit)
        {
            InitializeComponent();
            SetValues(edit);
        }


        public void SetValues(bool edit)
        {
            cbxstudent_birthday_day.ItemsSource = data_birthdayday;
            cbxstudent_birthday_month.ItemsSource = data_birthdaymonth;
            cbxstudent_birthday_year.ItemsSource = data_birthdayyear;
            cbxstudent_gender.ItemsSource = data_gender;
            cbxstudent_house.ItemsSource = data_house;
            data_id = MySql.student_ids;
            cbxstudent_id.ItemsSource = data_id;
            if (edit == true) { cbxstudent_id.Visibility = Visibility.Visible; }
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
        }


        private void btnback_Click(object sender, RoutedEventArgs e) { this.Close(); }


        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            if (tbxstudent_firstname.Text != "" && tbxstudent_lastname.Text != "" && cbxstudent_birthday_day.SelectedItem != null && cbxstudent_birthday_month.SelectedItem != null && cbxstudent_birthday_year.SelectedItem != null && cbxstudent_gender.SelectedItem != null && cbxstudent_house.SelectedItem != null)
            {
                MySql.student.Clear();
                if (btnsubmit.Content.ToString() == "Add")
                {
                    MySql.student.Add(new MySql.Students { firstname = $"{tbxstudent_firstname.Text}", lastname = $"{tbxstudent_lastname.Text}", birthday = $"{cbxstudent_birthday_day.SelectedItem}-{cbxstudent_birthday_month.SelectedItem}-{cbxstudent_birthday_year.SelectedItem}", gender = cbxstudent_gender.SelectedItem.ToString(), house_id = cbxstudent_house.SelectedIndex + 1 });
                    MySql.AddStudent(0);
                }
                else
                {
                    MySql.student.Add(new MySql.Students { id = int.Parse(cbxstudent_id.SelectedItem.ToString()), firstname = $"{tbxstudent_firstname.Text}", lastname = $"{tbxstudent_lastname.Text}", birthday = $"{cbxstudent_birthday_day.SelectedItem}-{cbxstudent_birthday_month.SelectedItem}-{cbxstudent_birthday_year.SelectedItem}", gender = cbxstudent_gender.SelectedItem.ToString(), house_id = cbxstudent_house.SelectedIndex + 1 });
                    MySql.UpdateStudent();
                }
                Reset();
            }
            else { MessageBox.Show("Error. Not all boxes had a selected value."); }
        }


        private void cbxstudent_id_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbxstudent_id.SelectedItem != null)
            {
                MySql.SelectStudents();
                int i = int.Parse(cbxstudent_id.SelectedItem.ToString()) - 1;

                tbxstudent_firstname.Text = MySql.student[i].firstname;
                tbxstudent_lastname.Text = MySql.student[i].lastname;
                cbxstudent_birthday_day.SelectedItem = int.Parse(MySql.student[i].birthday.Split('-')[0]);
                cbxstudent_birthday_month.SelectedItem = int.Parse(MySql.student[i].birthday.Split('-')[1]);
                cbxstudent_birthday_year.SelectedItem = int.Parse(MySql.student[i].birthday.Split('-')[2]);
                cbxstudent_gender.SelectedItem = MySql.student[i].gender;
                cbxstudent_house.SelectedIndex = MySql.student[i].house_id - 1;
                btnsubmit.Content = "Update";
                tbktitle.Text = "Edit Student";
            }
        }
    }
}