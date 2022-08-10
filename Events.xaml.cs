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
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : Window
    {
        public class Event
        {
            public int Id { get; set; }
            public int Place { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Distance { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
        }


        public Events()
        {
            InitializeComponent();
            cbxSearchFilter.ItemsSource = new string[] { "id", "type", "name", "distance", "age", "gender" };
            Refresh();
        }


        private void DisplayEvents()
        {
            lstEvents.Items.Clear();
            for (int i = 0; i < MySql.events.Count; i++)
            {
                var events = new Event()
                {
                    Id = MySql.events[i].id,
                    Type = MySql.events[i].type,
                    Name = MySql.events[i].name,
                    Distance = MySql.events[i].distance,
                    Age = MySql.events[i].age,
                    Gender = MySql.events[i].gender,
                };
                lstEvents.Items.Add(events);
            }
        }


        void Refresh()
        {
            MySql.SelectEvents();
            DisplayEvents();
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
                MySql.EventSearchQuery(cbxSearchFilter.SelectedItem.ToString(), tbxSearch.Text);
                DisplayEvents();
            }
            else { MessageBox.Show("No text was entered into the search. Please try again."); }
        }


        private void cbxSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxSearch.Text = "";
            tbxSearch.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }


        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddResult(-1);
            form.ShowDialog();
            Refresh();
        }


        private void lstEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var form = new Results(int.Parse(lstEvents.SelectedIndex.ToString()));
            form.ShowDialog();
            MySql.results.Clear();
            lstEvents.SelectedItem = null;
        }
    }
}