using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.IO;

namespace KHC_Athletics_and_House_Points
{
    class MySql
    {
        static MySqlConnection connection;
        public static bool connected = false;


        public static void Connect()
        {
            try
            {
                string constring = "SERVER=" + Program.mysql_server + ";" + "DATABASE=" + Program.mysql_database + ";" + "UID=" + Program.mysql_username + ";" + "PASSWORD=" + Program.mysql_password + ";";
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


        public static void SyncStudents()
        {
            if (connected == true)
            {
                string query = "SELECT * FROM students";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                string id;
                string houseid;
                while (reader.Read())
                {
                    id = $"{reader["id"]}";
                    houseid = $"{reader["house_id"]}";
                    Database.student.Add(new Database.Students { id = int.Parse(id), firstname = $"{reader["firstname"]}", lastname = $"{reader["lastname"]}", birthday = $"{reader["birthday"]}", age = Database.CalculateAge(DateTime.Parse($"{reader["birthday"]}")), gender = $"{reader["gender"]}", house_id = int.Parse(houseid) });
                }
                cmd.Dispose();
                reader.Close();
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


        public static void AddStudent(int index)
        {
            if (connected == true)
            {
                string query = "SELECT id FROM students WHERE firstname='" + Database.newStudent[index].firstname + "' AND lastname='" + Database.newStudent[index].lastname + "' AND birthday='" + Database.newStudent[index].birthday + "' AND gender='" + Database.newStudent[index].gender + "' AND house_id='" + Database.newStudent[index].house_id + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    MessageBox.Show("This student already exists in the database.");
                    cmd.Dispose();
                    reader.Close();
                }
                else
                {
                    cmd.Dispose();
                    reader.Close();
                    query = $"INSERT INTO students (firstname, lastname, birthday, gender, house_id) VALUES ('{Database.newStudent[index].firstname}', '{Database.newStudent[index].lastname}', '{Database.newStudent[index].birthday}', '{Database.newStudent[index].gender}', {Database.newStudent[index].house_id})";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    reader.Close();
                    query = "SELECT * FROM students WHERE firstname='" + Database.newStudent[index].firstname + "' AND lastname='" + Database.newStudent[index].lastname + "' AND birthday='" + Database.newStudent[index].birthday + "' AND gender='" + Database.newStudent[index].gender + "' AND house_id='" + Database.newStudent[index].house_id + "'";
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string id = $"{reader["id"]}";
                        Database.student.Add(new Database.Students { id = int.Parse(id), firstname = Database.newStudent[index].firstname, lastname = Database.newStudent[index].lastname, birthday = Database.newStudent[index].birthday, age = Database.CalculateAge(DateTime.Parse(Database.newStudent[index].birthday)), gender = Database.newStudent[index].gender, house_id = Database.newStudent[index].house_id });
                    }
                    MessageBox.Show("Successfully added student");
                }
            }
        }


        public static void UpdateStudent(int index)
        {
            if (connected == true)
            {
                string query = "UPDATE students SET firstname='" + Database.newStudent[0].firstname + "', lastname='" + Database.newStudent[0].lastname + "', birthday='" + Database.newStudent[0].birthday + "', gender='" + Database.newStudent[0].gender + "',house_id='" + Database.newStudent[0].house_id + "' WHERE  id='" + index + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated student");
            }
        }


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