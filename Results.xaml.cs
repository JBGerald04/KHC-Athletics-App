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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public class Event
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Distance { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
        }


        public class Result
        {
            public int Id { get; set; }
            public int Place { get; set; }
            public string RESULT { get; set; }
            public int Heat { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public int Points { get; set; }
        }
        int eventindex;


        public Results(int event_index)
        {
            InitializeComponent();
            eventindex = event_index;
            var events = new Event() { Type = MySql.events[eventindex].type.ToString(), Name = MySql.events[eventindex].name, Distance = MySql.events[eventindex].distance.ToString(), Age = int.Parse(MySql.events[eventindex].age.ToString()), Gender = MySql.events[eventindex].gender };
            lstEvent.Items.Add(events);
            Refresh();
            int[] data_id = new int[MySql.results.Count];
            for (int i = 0; i < MySql.results.Count; i++) { data_id[i] = MySql.results[i].id; }
            cbxDelete_id.ItemsSource = data_id;
        }


        private void DisplayResults()
        {
            lstResults.Items.Clear();
            string result;
            for (int i = 0; i < MySql.results.Count; i++)
            {
                if (MySql.events[eventindex].type == "Field")
                {
                    result = $"{MySql.results[i].result}m";
                }
                else
                {
                    TimeSpan time = TimeSpan.FromSeconds(MySql.results[i].result);
                    result = time.ToString("mm':'ss'.'ff");
                }

                var results = new Result()
                {
                    Id = MySql.results[i].id,
                    Place = MySql.results[i].place,
                    RESULT = result,
                    Heat = int.Parse(MySql.results[i].heat.ToString()),
                    Firstname = MySql.student[i].firstname,
                    Lastname = MySql.student[i].lastname,
                    Points = int.Parse(MySql.results[i].points.ToString())
                };
                lstResults.Items.Add(results);
            }
        }


        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddResult(eventindex);
            form.ShowDialog();
            MySql.SelectEvents();
            Refresh();
        }


        private void cbxDelete_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
        }


        void Refresh()
        {
            MySql.ResultSearchQuery(eventindex);
            DisplayResults();
        }


        private void btnBack_Click(object sender, RoutedEventArgs e) { Close(); }


        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            cbxDelete_id.SelectedItem = null;
            btnDelete.IsEnabled = false;
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MySql.DeleteResult(MySql.results[cbxDelete_id.SelectedIndex].id);
            cbxDelete_id.SelectedItem = null;
            btnDelete.IsEnabled = false;
            Refresh();
        }
    }
}
