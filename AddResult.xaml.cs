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
    /// Interaction logic for AddResult.xaml
    /// </summary>
    public partial class AddResult : Window
    {
        public AddResult()
        {
            InitializeComponent();
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
