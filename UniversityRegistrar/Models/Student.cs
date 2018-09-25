using System;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime enrollmentDate { get; set; }

        public Student(string newName, DateTime newEnrollmentDate, int id = 0)
        {
            Name = newName;
            enrollmentDate = newEnrollmentDate;
            Id = id;
        }
        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = this.Id == newStudent.Id;
                bool nameEquality = this.Name == newStudent.Name;
                return (idEquality && nameEquality);
            }
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO students (`name`,`enrollment_date`) VALUES (@newName,@newEnrollmentDate);";
            cmd.Parameters.AddWithValue("@newName", this.Name);
            cmd.Parameters.AddWithValue("@newEnrollmentDate", this.enrollmentDate);
            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
