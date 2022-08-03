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
        public static int student_count;
        public static int[] student_ids;


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


        // Database Connection
        public static void Connect()
        {
            try
            {
                string constring = "SERVER=" + Program.mysql_server + ";" + "Database=" + Program.mysql_database + ";" + "UID=" + Program.mysql_username + ";" + "PASSWORD=" + Program.mysql_password + ";";
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
                    cmd.Dispose();
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
                cmd.Dispose();
            }
        }




        // Students
        public static void SyncStudents()
        {
            if (connected == true)
            {
                Student_id_count();
                for (int i = 0; i < student_count; i++)
                {
                    string query = "UPDATE students SET age='" + CalculateAge(DateTime.Parse(student[i].birthday)) + "' WHERE  id='" + student[i].id + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                student.Clear();
            }
        }


        public static void Student_id_count()
        {
            if (connected == true)
            {
                string query = "SELECT COUNT(id) FROM students;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) { student_count = int.Parse(reader["COUNT(id)"].ToString()); }
                reader.Close();
                cmd.Dispose();
                student_ids = new int[student_count];

                query = "SELECT * FROM students";
                cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    student_ids[i] = int.Parse(reader["id"].ToString());
                    student.Add(new MySql.Students { id = int.Parse(reader["id"].ToString()), birthday = $"{reader["birthday"]}" });
                    i++;
                }
                cmd.Dispose();
                reader.Close();
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
                    query = $"INSERT INTO students (firstname, lastname, birthday, gender, age, house_id) VALUES ('{student[index].firstname}', '{student[index].lastname}', '{student[index].birthday}', {CalculateAge(DateTime.Parse(student[index].birthday))}, '{student[index].gender}', {student[index].house_id})";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Successfully added student");
                }
                Student_id_count();
            }
        }


        public static void UpdateStudent()
        {
            if (connected == true)
            {
                string query = "UPDATE students SET firstname='" + student[0].firstname + "', lastname='" + student[0].lastname + "', birthday='" + student[0].birthday + "', age='" + CalculateAge(DateTime.Parse(student[0].birthday)) + "', gender='" + student[0].gender + "', house_id='" + student[0].house_id + "' WHERE  id='" + student[0].id + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                MessageBox.Show("Successfully updated student");
                Student_id_count();
            }
        }


        public static void SelectStudents()
        {
            string query = "SELECT * FROM students";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                student.Add(new MySql.Students { id = int.Parse(reader["id"].ToString()), firstname = $"{reader["firstname"]}", lastname = $"{reader["lastname"]}", birthday = $"{reader["birthday"]}", age = int.Parse(reader["age"].ToString()), gender = $"{reader["gender"]}", house_id = int.Parse(reader["house_id"].ToString()) });
                i++;
            }
            reader.Close();
            cmd.Dispose();
        }


        public static string SearchQuery(string column, string text)
        {
            string query = $"SELECT * FROM students WHERE {column}='" + text + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                student.Add(new MySql.Students { id = int.Parse(reader["id"].ToString()), firstname = $"{reader["firstname"]}", lastname = $"{reader["lastname"]}", birthday = $"{reader["birthday"]}", age = int.Parse(reader["age"].ToString()), gender = $"{reader["gender"]}", house_id = int.Parse(reader["house_id"].ToString()) });
                count++;
            }
            reader.Close();
            cmd.Dispose();
            return count.ToString();
        }


        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;
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