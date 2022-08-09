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
            public string Type { get; set; }
            public string Name { get; set; }
            public string Distance { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
        }


        public Events()
        {
            InitializeComponent();
            MySql.SelectEvents();
            DisplayEvents(MySql.event_count);
        }


        private void DisplayEvents(int count)
        {
            lstEvents.Items.Clear();
            for (int i = 0; i < count; i++)
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


        private void btnBack_Click(object sender, RoutedEventArgs e) { this.Close(); }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MySql.EventSearchQuery(cbxSearchFilter.SelectedItem.ToString(), tbxSearch.Text);
            DisplayEvents(MySql.events.Count);
        }


        private void cbxSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxSearch.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }
    }
}
