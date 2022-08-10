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
        public static List<Results> results = new List<Results>();
        public static List<Events> events = new List<Events>();


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


        public class Results
        {
            public int id;
            public int place;
            public float result;
            public int heat;
            public int event_id;
            public int student_id;
            public int points;
        }


        public class Events
        {
            public int id;
            public string type;
            public string name;
            public string distance;
            public int age;
            public string gender;
        }




        //// Database Connection ////
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




        //// House Points ////
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


        public static void AddHousePoints(int[] points)
        {
            if (connected == true)
            {
                string query = "";
                for (int i = 0; i < 4; i++) { query += $"UPDATE houses SET house_points=(house_points + {points[i]}) WHERE id={i + 1};"; }
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }




        //// Students ////
        public static void SyncStudents()
        {
            if (connected == true)
            {
                SelectStudents();
                string query = "";
                for (int i = 0; i < student.Count; i++) { query += $"UPDATE students SET age={CalculateAge(DateTime.Parse(student[i].birthday))}, birthday='{(DateTime.Parse(student[i].birthday)).ToString().Split(' ')[0]}' WHERE id={student[i].id};"; }
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }


        public static void AddStudent(int index)
        {
            if (connected == true)
            {
                string query = $"SELECT id FROM students WHERE firstname=\"{student[index].firstname}\" AND lastname=\"{student[index].lastname}\" AND birthday='" + student[index].birthday + "' AND gender='" + student[index].gender + "' AND house_id='" + student[index].house_id + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                if (reader.HasRows)
                {
                    reader.Read();
                    MessageBox.Show($"Student {reader["id"]} already exists in the database.");
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    query = $"INSERT INTO students (firstname, lastname, birthday, age, gender, house_id) VALUES (\"{student[index].firstname}\", \"{student[index].lastname}\", '{student[index].birthday}', {CalculateAge(DateTime.Parse(student[index].birthday))}, '{student[index].gender}', {student[index].house_id})";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
        }


        public static void UpdateStudent()
        {
            if (connected == true)
            {
                string query = $"UPDATE students SET firstname=\"{student[0].firstname}\", lastname=\"{student[0].lastname}\", birthday='" + student[0].birthday + "', age='" + CalculateAge(DateTime.Parse(student[0].birthday)) + "', gender='" + student[0].gender + "', house_id='" + student[0].house_id + "' WHERE  id='" + student[0].id + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                MessageBox.Show("Successfully updated student");
            }
        }


        public static void SelectStudents()
        {
            student.Clear();
            string query = "SELECT * FROM students";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { student.Add(new Students { id = int.Parse(reader["id"].ToString()), firstname = $"{reader["firstname"]}", lastname = $"{reader["lastname"]}", birthday = $"{reader["birthday"]}", age = int.Parse(reader["age"].ToString()), gender = $"{reader["gender"]}", house_id = int.Parse(reader["house_id"].ToString()) }); }
            reader.Close();
            cmd.Dispose();
         }


        public static void StudentSearchQuery(string column, string text)
        {
            student.Clear();
            string query = $"SELECT * FROM students WHERE ";
            for (int i = 0; i < column.Split(',').Length; i++) { query += $"{column.Split(',')[i]}='" + text.Split(',')[i] + "' AND "; }
            query = query.Substring(0, query.Length - 5);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                student.Add(new Students { id = int.Parse(reader["id"].ToString()), firstname = $"{reader["firstname"]}", lastname = $"{reader["lastname"]}", birthday = $"{reader["birthday"]}", age = int.Parse(reader["age"].ToString()), gender = $"{reader["gender"]}", house_id = int.Parse(reader["house_id"].ToString()) });
            }
            reader.Close();
            cmd.Dispose();
        }




        //// Results ////
        public static void AddResult(int count)
        {
            string query = $"INSERT INTO results (result, heat, event_id, student_id) VALUES ";
            for (int i = 0; i < count; i++) { query += $"({results[i].result}, {results[i].heat}, {results[i].event_id}, {results[i].student_id}), "; }
            query = query.Substring(0, query.Length - 2);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        public static void UpdatePoints(int event_id, int index)
        {
            string query = $"SELECT results.id, results.result, students.house_id FROM results INNER JOIN students ON student_id = students.id WHERE event_id = {event_id} AND points_added = 0 ORDER BY results.result ";
            if (events[index].type == "Field") { query += "DESC;"; }
            else { query += "ASC;"; }
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            int[] Points = new int[4];
            int points = 10;
            int count = 0;
            while (reader.Read())
            {
                results.Add(new Results { id = int.Parse($"{reader["id"]}"), points = points });
                Points[int.Parse($"{reader["house_id"]}") - 1] += points;
                if (points > 1) { points--; }
                count++;
            }
            cmd.Dispose();
            reader.Close();

            AddHousePoints(new int[] { Points[0], Points[1], Points[2], Points[3] });
            for (int j = 0; j < 4; j++) { if (Points[j] != 0) { Sentral.AddHousePoints(j, Points[j], $"{events[index].distance} {events[index].name}, {events[index].age}'s {events[index].gender} - {DateTime.Now.ToString().Split(' ')[1]}"); } } //[1]

            query = "";
            for (int i = 0; i < count; i++) { query += $"UPDATE results SET points={results[i].points}, points_added=1 WHERE id={results[i].id};"; }
            cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        public static void ResultSearchQuery(int index)
        {
            results.Clear();
            student.Clear();
            string query = $"SELECT results.id, results.result, results.heat, students.firstname, students.lastname, results.points FROM results INNER JOIN students ON student_id = students.id WHERE event_id = {events[index].id} ORDER BY results.result ";
            if (events[index].type == "Field") { query += "DESC;"; }
            else { query += "ASC;"; }
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {
                results.Add(new Results { id = int.Parse(reader["id"].ToString()), place = count, result = float.Parse(reader["result"].ToString()), heat = int.Parse(reader["heat"].ToString()), points = int.Parse(reader["points"].ToString()) });
                student.Add(new Students { firstname = reader["firstname"].ToString(), lastname = reader["lastname"].ToString() });
                count++;
            }
            reader.Close();
            cmd.Dispose();
        }


        public static void DeleteResult(int result_id)
        {
            string query = $"DELETE FROM results WHERE id = {result_id}";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        //public static void GenerateResult()
        //{
        //    string query = "SELECT `result`, `heat`, `points`, students.* FROM `results' INNER JOIN students ON student_id = students.id WHERE result_id = 1 ORDER BY result DESC;";
        //    MySqlCommand cmd = new MySqlCommand(query, connection);
        //    MySqlDataReader reader = cmd.ExecuteReader();
        //}




        //// Events ////
        public static void AddEvent(string Type, string Name, string Distance, int Age, string Gender)
        {
            if (connected == true)
            {
                int Id = 0;
                string query = "SELECT id FROM events WHERE type='" + Type + "' AND name='" + Name + "' AND distance='" + Distance + "' AND age='" + Age + "' AND gender='" + Gender + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                if (!reader.Read())
                {
                    reader.Close();
                    query = $"INSERT INTO events (type, name, distance, age, gender) VALUES ('{Type}', '{Name}', '{Distance}', {Age}, '{Gender}')";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id FROM events WHERE type = '" + Type + "' AND name = '" + Name + "' AND distance = '" + Distance + "' AND age = '" + Age + "' AND gender = '" + Gender + "'";
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { Id = int.Parse($"{reader["id"]}"); }
                    cmd.Dispose();
                    reader.Close();
                }
                else
                {
                    reader.Read();
                    Id = int.Parse($"{reader["id"]}");
                    reader.Close();
                }
                events.Add(new Events { id = Id, type = Type, name = Name, distance = Distance, age = Age, gender = Gender });
            }
        }


        public static void SelectEvents()
        {
            events.Clear();
            string query = "SELECT * FROM events";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { events.Add(new  Events { id = int.Parse(reader["id"].ToString()), type = $"{reader["type"]}", name = $"{reader["name"]}", distance = $"{reader["distance"]}", age = int.Parse(reader["age"].ToString()), gender = $"{reader["gender"]}" }); }
            reader.Close();
            cmd.Dispose();
        }


        public static void EventSearchQuery(string column, string text)
        {
            events.Clear();
            string query = $"SELECT * FROM events WHERE ";
            for (int i = 0; i < column.Split(',').Length; i++) { query += $"{column.Split(',')[i]}='" + text.Split(',')[i] + "' AND "; }
            query = query.Substring(0, query.Length - 5);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try { events.Add(new Events { id = int.Parse(reader["id"].ToString()), type = reader["type"].ToString(), name = reader["name"].ToString(), distance = reader["distance"].ToString(), age = int.Parse(reader["age"].ToString()), gender = reader["gender"].ToString() }); }
                catch { MessageBox.Show("Search yeilded no results."); }
            }
            reader.Close();
            cmd.Dispose();
        }




        //// Misc ////
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


        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;
            return age;
        }
    }
}