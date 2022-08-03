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

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for Students.xaml
    /// </summary>
    public partial class Students : Window
    {
        //public event System.Windows.Input.TextCompositionEventHandler PreviewTextInput;
        public string[] filter_data = { "id", "firstname", "lastname", "birthday", "age", "gender", "house" };

        public Students()
        {
            InitializeComponent();
            cbxSearchFilter.ItemsSource = filter_data;
            MySql.SelectStudents();
            DisplayStudents(MySql.student_count);
        }


        private void DisplayStudents(int count)
        {
            //var rowGroup = tblstudents.RowGroups.FirstOrDefault();
            for (int i = 0; i < count; i++)
            {
                var student = new Student()
                {
                    Id = MySql.student[i].id,
                    Firstname = MySql.student[i].firstname
                };
                lstStudents.Items.Add(student);





                //TableRow row = new TableRow();
                //TableCell cell = new TableCell();

                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].id.ToString())));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].firstname)));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].lastname)));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].birthday)));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].age.ToString())));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].gender)));
                //row.Cells.Add(cell);

                //cell = new TableCell();
                //cell.Blocks.Add(new Paragraph(new Run(Sentral.houseData[MySql.student[i].house_id - 1].house_name)));
                //row.Cells.Add(cell);

                //rowGroup.Rows.Add(row);
            }
            MySql.student.Clear();
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(MySql.SearchQuery(cbxSearchFilter.SelectedItem.ToString(), tbxSearch.Text));
            DisplayStudents(count);
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

        private void cbxSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxSearch.IsEnabled = true;
            btnSearch.IsEnabled = true;

            //if (cbxSearchFilter.SelectedItem.ToString() == "id")
            //{
            //    PreviewTextInput = "MaskNumericInput";
            //    tbxSearch.Text = PreviewTextInput;
            //    //tbxSearch.Handled = Utility.IsTextNumeric(e.Text);
            //    //tbxSearch.PreviewTextInput += "MaskNumericInput";
            //}
            //else if (cbxSearchFilter.SelectedItem.ToString() == "firstname")
            //{

            //}
            //else if (cbxSearchFilter.SelectedItem.ToString() == "lastname")
            //{

            //}
            //else if (cbxSearchFilter.SelectedItem.ToString() == "birthday")
            //{

            //}
            //else if (cbxSearchFilter.SelectedItem.ToString() == "age")
            //{

            //}
            //else if (cbxSearchFilter.SelectedItem.ToString() == "gender")
            //{

            //}
            //else
            //{

            //}
        }
    }
}