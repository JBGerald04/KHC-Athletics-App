using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.IO;
using System.Collections.Generic;

namespace KHC_Athletics_and_House_Points
{
    class MySql
    {
        static MySqlConnection connection;
        public static bool connected = false;
        public static List<Students> student = new List<Students>();


        public class Students
        {
            // Data variables for use throughout the project with regards to students
            public int id;
            public string firstname;
            public string lastname;
            public string birthday;
            public int age;
            public string gender;
            public int house_id;
        }


        // Establish a connection with database
        public static void Connect()
        {
            try
            {
                string constring = "SERVER=" + Program.mysql_server + ";" + "MySql=" + Program.mysql_database + ";" + "UID=" + Program.mysql_username + ";" + "PASSWORD=" + Program.mysql_password + ";";
                connection = new MySqlConnection(constring);
                connection.Open();
                connected = true;
            }
            catch
            {
                MessageBox.Show("Error connecting to mysql!");
                connected = false;
            }
        }


        // House Points

        public static void SyncHouseData()
        {
            if (connected == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    string query = "UPDATE houses SET house_name='" + Sentral.houseData[i].house_name + "',house_colour='" + Sentral.houseData[i].house_colour + "',house_points='" + Sentral.houseData[i].house_points + "',house_sentralid='" + Sentral.houseData[i].house_sentralid + "' WHERE id='" + (i + 1) + "';";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void AddHousePoints(int index, int points)
        {
            if (connected == true)
            {
                string query = "UPDATE houses SET id='" + index + "',house_points='" + points + "' WHERE  id='" + index + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }




        // Students

        public static void SyncStudents()
        {
            if (connected == true)
            {
                for (int i = 0; i < ; i++)
                {
                    string query = "UPDATE houses SET house_name='" + Sentral.houseData[i].house_name + "',house_colour='" + Sentral.houseData[i].house_colour + "',house_points='" + Sentral.houseData[i].house_points + "',house_sentralid='" + Sentral.houseData[i].house_sentralid + "' WHERE id='" + (i + 1) + "';";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void AddStudent(int index)
        {
            if (connected == true)
            {
                string query = "SELECT id FROM students WHERE firstname='" + student[index].firstname + "' AND lastname='" + student[index].lastname + "' AND birthday='" + student[index].birthday + "' AND gender='" + student[index].gender + "' AND house_id='" + student[index].house_id + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                reader.Close();

                if (reader.HasRows) { MessageBox.Show("This student already exists in the database."); }
                else
                {
                    query = $"INSERT INTO students (firstname, lastname, birthday, gender, house_id) VALUES ('{student[index].firstname}', '{student[index].lastname}', '{student[index].birthday}', '{student[index].gender}', {student[index].house_id})";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    reader.Close();
                    MessageBox.Show("Successfully added student");
                }
            }
        }


        public static void UpdateStudent(int index)
        {
            if (connected == true)
            {
                string query = "UPDATE students SET firstname='" + student[0].firstname + "', lastname='" + student[0].lastname + "', birthday='" + student[0].birthday + "', gender='" + student[0].gender + "',house_id='" + student[0].house_id + "' WHERE  id='" + index + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated student");
            }
        }


        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;
            return age;
        }





        // Results

        public static void ResultsRequest(string event_name, int age, string gender)
        {
            if (connected == true)
            {
                //string query = "SELECT * FROM students WHERE age='" + age + "',gender='" + gender
            }
        }









        // Misc

        public static void GenerateReport()
        {
            string query = "SELECT * FROM houses";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            var file = File.CreateText("points.txt");
            while (reader.Read())
            {
                file.WriteLine($"{reader["house_name"]} - {reader["house_points"]}");
                file.WriteLine();
            }
            reader.Close();
            file.Close();
        }
    }
}