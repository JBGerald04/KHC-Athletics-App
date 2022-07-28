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
        public Students()
        {
            InitializeComponent();
            DisplayStudents();
        }


        private void DisplayStudents()
        {
            var rowGroup = tblstudents.RowGroups.FirstOrDefault();

            //for (int i = 0; i < MySql.newid.Count; i++)
            //{

            //    TableRow row = new TableRow();
            //    TableCell cell = new TableCell();

            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].id.ToString())));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].firstname)));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].lastname)));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].birthday)));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].age.ToString())));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(MySql.student[i].gender)));
            //    row.Cells.Add(cell);

            //    cell = new TableCell();
            //    cell.Blocks.Add(new Paragraph(new Run(Sentral.houseData[MySql.student[i].house_id - 1].house_name)));
            //    row.Cells.Add(cell);

            //    rowGroup.Rows.Add(row);
            //}
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnSearchGo_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(true);
            form.ShowDialog();
        }


        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddStudent(false);
            form.ShowDialog();
        }
    }
}