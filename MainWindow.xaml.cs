using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnStudents_Click(object sender, RoutedEventArgs e)
        {
            var form = new Students();
            form.ShowDialog();
            MySql.student.Clear();
        }


        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            var form = new Events();
            form.ShowDialog();
            MySql.events.Clear();
        }


        private void btnQuit_Click(object sender, RoutedEventArgs e) { this.Close(); }


        private void btnHouse_Points_Click(object sender, RoutedEventArgs e) { Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "localhost/points/"); }
    }
}