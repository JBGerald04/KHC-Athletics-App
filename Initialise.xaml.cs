﻿using System;
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
using System.IO;

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for Initialise.xaml
    /// </summary>
    public partial class Initialise : Window
    {
        public Initialise()
        {
            InitializeComponent();
            LoadFile();
        }


        public bool rewriteSentral;
        public  bool rewriteSql;


        private void Start()
        {
            pgbSentral.Value = 50;
            Sentral.Login();
            pgbSentral.Value = 75;
            Sentral.DownloadHouseData();
            pgbSentral.Value = 100;
            MySql.Connect();
            pgbSql.Value = 50;
            MySql.SyncHouseData();
            pgbSql.Value = 75;
            MySql.SyncStudents();
            pgbSql.Value = 100;

            if (MySql.connected == true && Sentral.connected == true)
            {
                SaveFile();
                MessageBox.Show("Success!!!");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            else { MessageBox.Show("Error Logging in. Please check that your details are correct and that you are connected to the internet. Then try again."); }
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cbxSentral.IsChecked == true) { rewriteSentral = true; }
            else { rewriteSentral = false; }
            if (cbxSql.IsChecked == true) { rewriteSql = true; }
            else { rewriteSql = false; }
            Program.sentral_username = tbxSentral_Username.Text;
            Program.sentral_password = tbxSentral_Password.Text;
            Program.mysql_server = tbxMySql_URL.Text;
            Program.mysql_database = tbxMySql_Database.Text;
            Program.mysql_username = tbxMySql_Username.Text;
            Program.mysql_password = tbxMySql_Password.Text;
            Start();
        }


        private void LoadFile()
        {
            try
            {
                var file = File.OpenText("start.txt");   // Opens the text file "start.txt"
                var line = file.ReadLine();             // Creates variables that read a line from the file, it separates it into an array 'data'
                var data = line.Split(':');

                tbxSentral_Username.Text = data[0];
                tbxSentral_Password.Text = data[1];
                tbxMySql_URL.Text = data[2];
                tbxMySql_Database.Text = data[3];
                tbxMySql_Username.Text = data[4];
                tbxMySql_Password.Text = data[5];
            }
            catch
            {
                var file = File.CreateText("start.txt");
                file.WriteLine(":::::");
                file.Close();
            }
        }


        public void SaveFile()
        {
            string line1;
            string line2;

            if (rewriteSentral == true) { line1 = $"{Program.sentral_username}:{Program.sentral_password}:"; }
            else { line1 = "::"; }
            if (rewriteSql == true) { line2 = $"{Program.mysql_server}:{Program.mysql_database}:{Program.mysql_username}:{Program.mysql_password}"; }
            else { line2 = ":::"; }
            File.WriteAllLines("Start.txt", new string[] { line1 + line2 });
        }
    }
}