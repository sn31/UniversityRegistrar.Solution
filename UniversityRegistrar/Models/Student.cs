using System;
using System.Collections.Generic;
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
                bool enrollmentEquality = this.enrollmentDate == newStudent.enrollmentDate;
                return (idEquality && nameEquality && enrollmentEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
        public override string ToString()
        {
             return String.Format("{{ id={0}, name={1}, date={2}}}", Id, Name, enrollmentDate);
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
        public static List<Student> GetAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM students;";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Student> allStudents = new List<Student> { };

            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string Name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(Name, enrollmentDate, Id);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM students WHERE id = @searchId;";
            cmd.Parameters.AddWithValue("@searchId", searchId);

            MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read();
            int Id = rdr.GetInt32(0);
            string Name = rdr.GetString(1);
            DateTime enrollmentDate = rdr.GetDateTime(2);
            Student foundStudent = new Student(Name, enrollmentDate, Id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundStudent;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM courses_students WHERE student_id = @deleteId;DELETE FROM students WHERE id = @deleteId;";
            cmd.Parameters.AddWithValue("@deleteId", this.Id);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM students";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Update(string newName, DateTime newEnrollmentDate)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE students SET name = @newName,enrollment_date = @newEnrollmentDate WHERE id = @thisId;"; //This might not work!!!!
            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@newEnrollmentDate", newEnrollmentDate);
            cmd.Parameters.AddWithValue("@thisId", this.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO courses_students (`course_id`, `student_id`) VALUES (@courseId, @studentId);";
            cmd.Parameters.AddWithValue("@courseId", newCourse.Id);
            cmd.Parameters.AddWithValue("@studentId", this.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Course> GetCourses()
        {
            List<Course> allCourses = new List<Course>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT courses.* FROM students
            JOIN courses_students ON (students.id = courses_students.student_id)
            JOIN courses ON (courses_students.course_id = courses.id)
            WHERE students.id = @thisId
            ;";
            cmd.Parameters.AddWithValue("@thisId", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string CourseName = rdr.GetString(1);
                string CourseNumber = rdr.GetString(2);
                Course newCourse = new Course(CourseName, CourseNumber, Id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;

        }
    }
}