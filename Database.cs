using System.Collections.Generic;
using System;

namespace KHC_Athletics_and_House_Points
{
    class Database
    {
        public static List<Students> student = new List<Students>();
        public static List<AddStudent> newStudent = new List<AddStudent>();


        public class AddStudent
        {
            // Data variables for use throughout the project with regards to students
            public string firstname;
            public string lastname;
            public string birthday;
            public string gender;
            public int house_id;
        }


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


        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;
            return age;
        }
    }
}