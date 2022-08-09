using System;
using System.Windows;

namespace KHC_Athletics_and_House_Points
{
    /// <summary>
    /// Interaction logic for AddResult.xaml
    /// </summary>
    public partial class AddResult : Window
    {
        string[] data_type = { "*Event Type", "Swim", "Track", "Field" };
        string[] data_distance = { "*Distance", "50m", "100m", "200m", "400m", "800m" };
        string[] data_name_swim = { "*Event Name", "Breaststroke", "Backstroke", "Butterfly", "Freestyle", "Medley", "Relay" };
        string[] data_name_field = { "*Event Name", "High Jump", "Long Jump", "Discus", "Shot Put" };
        string[] data_heat = { "*Heat", "1", "2", "3", "4", "5" };
        string[] data_age = { "*Age", "12", "13", "14", "15", "16", "17", "18" };
        string[] data_gender = { "*Gender", "Male", "Female", "Not Specified" };
        string mask = "-";
        bool Load = false;
        string[] data;
        int[] index;
        int readcount = 0;
        float[] Result;
        int event_count = 0;


        public AddResult()
        {
            InitializeComponent();
            Start();
            Load = true;
        }


        public void Start()
        {
            cbxtype.ItemsSource = data_type;
            cbxevent.ItemsSource = new string[] { "*Event Name" };
            cbxdistance.ItemsSource = data_distance;
            cbxheat.ItemsSource = data_heat;
            cbxage.ItemsSource = data_age;
            cbxgender.ItemsSource = data_gender;
            cbxtype.SelectedIndex = 0;
            cbxevent.SelectedIndex = 0;
            cbxdistance.SelectedIndex = 0;
            cbxheat.SelectedIndex = 0;
            cbxage.SelectedIndex = 0;
            cbxgender.SelectedIndex = 0;
        }


        void LoadStudents()
        {
            data = null;
            data = new string[MySql.student.Count];
            if (MySql.student.Count == 0) { data = new string[] { "No results" }; }
            else { for (int i = 0; i < MySql.student.Count; i++) { data[i] = $"{MySql.student[i].firstname} {MySql.student[i].lastname}"; } }

            tbxtime_distance1.Text = "";
            tbxtime_distance2.Text = "";
            tbxtime_distance3.Text = "";
            tbxtime_distance4.Text = "";
            tbxtime_distance5.Text = "";
            tbxtime_distance6.Text = "";
            tbxtime_distance7.Text = "";
            tbxtime_distance8.Text = "";
            tbxtime_distance9.Text = "";
            tbxtime_distance10.Text = "";

            tbxtime_distance1.Mask = mask;
            tbxtime_distance2.Mask = mask;
            tbxtime_distance3.Mask = mask;
            tbxtime_distance4.Mask = mask;
            tbxtime_distance5.Mask = mask;
            tbxtime_distance6.Mask = mask;
            tbxtime_distance7.Mask = mask;
            tbxtime_distance8.Mask = mask;
            tbxtime_distance9.Mask = mask;
            tbxtime_distance10.Mask = mask;

            cbxstudentname1.ItemsSource = data;
            cbxstudentname2.ItemsSource = data;
            cbxstudentname3.ItemsSource = data;
            cbxstudentname4.ItemsSource = data;
            cbxstudentname5.ItemsSource = data;
            cbxstudentname6.ItemsSource = data;
            cbxstudentname7.ItemsSource = data;
            cbxstudentname8.ItemsSource = data;
            cbxstudentname9.ItemsSource = data;
            cbxstudentname10.ItemsSource = data;
        }


        bool Read()
        {
            index = new int[10];
            Result = new float[10];
            readcount = 0;
            if (cbxstudentname1.SelectedItem != null) { index[0] = cbxstudentname1.SelectedIndex; readcount++; }
            if (cbxstudentname2.SelectedItem != null) { index[1] = cbxstudentname2.SelectedIndex; readcount++; }
            if (cbxstudentname3.SelectedItem != null) { index[2] = cbxstudentname3.SelectedIndex; readcount++; }
            if (cbxstudentname4.SelectedItem != null) { index[3] = cbxstudentname4.SelectedIndex; readcount++; }
            if (cbxstudentname5.SelectedItem != null) { index[4] = cbxstudentname5.SelectedIndex; readcount++; }
            if (cbxstudentname6.SelectedItem != null) { index[5] = cbxstudentname6.SelectedIndex; readcount++; }
            if (cbxstudentname7.SelectedItem != null) { index[6] = cbxstudentname7.SelectedIndex; readcount++; }
            if (cbxstudentname8.SelectedItem != null) { index[7] = cbxstudentname8.SelectedIndex; readcount++; }
            if (cbxstudentname9.SelectedItem != null) { index[8] = cbxstudentname9.SelectedIndex; readcount++; }
            if (cbxstudentname10.SelectedItem != null) { index[9] = cbxstudentname10.SelectedIndex; readcount++; }

            if (mask == "00.00")
            {
                if (tbxtime_distance1.Text != "__.__") { try { Result[0] = float.Parse(tbxtime_distance1.Text); } catch { return true; } }
                if (tbxtime_distance2.Text != "__.__") { try { Result[1] = float.Parse(tbxtime_distance2.Text); } catch { return true; } }
                if (tbxtime_distance3.Text != "__.__") { try { Result[2] = float.Parse(tbxtime_distance3.Text); } catch { return true; } }
                if (tbxtime_distance4.Text != "__.__") { try { Result[3] = float.Parse(tbxtime_distance4.Text); } catch { return true; } }
                if (tbxtime_distance5.Text != "__.__") { try { Result[4] = float.Parse(tbxtime_distance5.Text); } catch { return true; } }
                if (tbxtime_distance6.Text != "__.__") { try { Result[5] = float.Parse(tbxtime_distance6.Text); } catch { return true; } }
                if (tbxtime_distance7.Text != "__.__") { try { Result[6] = float.Parse(tbxtime_distance7.Text); } catch { return true; } }
                if (tbxtime_distance8.Text != "__.__") { try { Result[7] = float.Parse(tbxtime_distance8.Text); } catch { return true; } }
                if (tbxtime_distance9.Text != "__.__") { try { Result[8] = float.Parse(tbxtime_distance9.Text); } catch { return true; } }
                if (tbxtime_distance10.Text != "__.__") { try { Result[9] = float.Parse(tbxtime_distance10.Text); } catch { return true; } }
            }
            else
            {
                if (tbxtime_distance1.Text != "__:__.__") { try { Result[0] = float.Parse($"{int.Parse(tbxtime_distance1.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance1.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance2.Text != "__:__.__") { try { Result[1] = float.Parse($"{int.Parse(tbxtime_distance2.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance2.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance3.Text != "__:__.__") { try { Result[2] = float.Parse($"{int.Parse(tbxtime_distance3.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance3.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance4.Text != "__:__.__") { try { Result[3] = float.Parse($"{int.Parse(tbxtime_distance4.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance4.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance5.Text != "__:__.__") { try { Result[4] = float.Parse($"{int.Parse(tbxtime_distance5.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance5.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance6.Text != "__:__.__") { try { Result[5] = float.Parse($"{int.Parse(tbxtime_distance6.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance6.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance7.Text != "__:__.__") { try { Result[6] = float.Parse($"{int.Parse(tbxtime_distance7.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance7.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance8.Text != "__:__.__") { try { Result[7] = float.Parse($"{int.Parse(tbxtime_distance8.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance8.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance9.Text != "__:__.__") { try { Result[8] = float.Parse($"{int.Parse(tbxtime_distance9.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance9.Text.Split(':')[1]); } catch { return true; } }
                if (tbxtime_distance10.Text != "__:__.__") { try { Result[9] = float.Parse($"{int.Parse(tbxtime_distance10.Text.Split(':')[0]) * 60}") + float.Parse(tbxtime_distance10.Text.Split(':')[1]); } catch { return true; } }
            }
            return false;
        }


        void Reset()
        {
            cbxtype.SelectedIndex = 0;
            cbxevent.SelectedIndex = 0;
            cbxdistance.SelectedIndex = 0;
            cbxheat.SelectedIndex = 0;
            cbxage.SelectedIndex = 0;
            cbxgender.SelectedIndex = 0;

            cbxstudentname1.SelectedItem = null;
            cbxstudentname2.SelectedItem = null;
            cbxstudentname3.SelectedItem = null;
            cbxstudentname4.SelectedItem = null;
            cbxstudentname5.SelectedItem = null;
            cbxstudentname6.SelectedItem = null;
            cbxstudentname7.SelectedItem = null;
            cbxstudentname8.SelectedItem = null;
            cbxstudentname9.SelectedItem = null;
            cbxstudentname10.SelectedItem = null;
        }


        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < event_count; i++) { MySql.UpdatePoints(MySql.events[i].id, i); }
            MessageBox.Show($"Added results and point for {event_count} events.");
            this.Close();
        }


        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbxtype.SelectedItem.ToString() != "*Event Type" && cbxdistance.SelectedItem.ToString() != "*Distance" && cbxevent.SelectedItem.ToString() != "*Event Name" && cbxheat.SelectedItem.ToString() != "*Heat" && cbxage.SelectedItem.ToString() != "*Age" && cbxgender.SelectedItem.ToString() != "*Gender")
            {
                bool exit = Read();
                if (exit == true) { MessageBox.Show("Error reading results. \nEnsure that all values are filled properly eg. 05.39 NOT _5.39.\nClick submit to try again."); return; }
                MySql.AddEvent(cbxtype.SelectedItem.ToString(), cbxevent.SelectedItem.ToString(), cbxdistance.SelectedItem.ToString(), int.Parse(cbxage.SelectedItem.ToString()), cbxgender.SelectedItem.ToString());
                for (int i = 0; i < readcount; i++) { MySql.results.Add(new MySql.Results { result = Result[i], heat = int.Parse(cbxheat.SelectedItem.ToString()), event_id = MySql.events[event_count].id, student_id = MySql.student[index[i]].id }); }
                MySql.AddResult(readcount);
                event_count++;
                MessageBox.Show("Successfully added event, and results.");
            }
            else { MessageBox.Show("Error. Not all boxes had a selected value."); }
            MySql.student.Clear();
            MySql.results.Clear();
            Reset();
        }


        private void cbxtype_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbxtype.SelectedItem.ToString() == "Field")
            {
                mask = "00.00";
                cbxdistance.ItemsSource = new string[] { "N/A" };
                cbxevent.ItemsSource = data_name_field;
                cbxdistance.IsEnabled = false;
                cbxevent.IsEnabled = true;
                cbxdistance.SelectedIndex = 0;
                cbxevent.SelectedIndex = 0;
            }
            else if (cbxtype.SelectedItem.ToString() == "Swim" || cbxtype.SelectedItem.ToString() == "Track")
            {
                mask = "00:00.00";
                if (cbxtype.SelectedItem.ToString() == "Track")
                {
                    cbxevent.ItemsSource = new string[] { "Sprint" };
                    cbxevent.IsEnabled = false;
                }
                else
                {
                    cbxevent.ItemsSource = data_name_swim;
                    cbxevent.IsEnabled = true;
                }
                cbxdistance.ItemsSource = data_distance;
                cbxdistance.IsEnabled = true;
                cbxevent.SelectedIndex = 0;
                cbxdistance.SelectedIndex = 0;
            }
            else { mask = "-"; }
            cbxgender.SelectedIndex = 0;
        }


        private void cbxgender_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbxtype.SelectedItem.ToString() != "*Event Type" && cbxdistance.SelectedItem.ToString() != "*Distance" && cbxevent.SelectedItem.ToString() != "*Event Name" && cbxheat.SelectedItem.ToString() != "*Heat" && cbxage.SelectedItem.ToString() != "*Age" && cbxgender.SelectedItem.ToString() != "*Gender")
            {
                MySql.StudentSearchQuery("age,gender", $"{int.Parse(cbxage.SelectedItem.ToString())},{cbxgender.SelectedItem}");
                LoadStudents();
            }
            else if (Load == true && cbxgender.SelectedIndex != 0)
            {
                MessageBox.Show("Error. Not all boxes had a selected value.");
                cbxgender.SelectedIndex = 0;
            }
            else { LoadStudents(); }
        }


        private void cbxevent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { cbxgender.SelectedIndex = 0; }


        private void cbxdistance_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { cbxgender.SelectedIndex = 0; }


        private void cbxheat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { cbxgender.SelectedIndex = 0; }


        private void cbxage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { cbxgender.SelectedIndex = 0; }
    }
}