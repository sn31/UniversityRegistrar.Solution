using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar.Models;

namespace UniversityRegistrar
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
        public Course(string courseName, string courseNumber, int id = 0)
        {
            Id = id;
            CourseName = courseName;
            CourseNumber = courseNumber;
        }
        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = this.Id == newCourse.Id;
                bool courseNameEquality = this.CourseName == newCourse.CourseName;
                bool courseNumberEquality = this.CourseNumber == newCourse.CourseNumber;
                return (idEquality && courseNameEquality && courseNumberEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.CourseName.GetHashCode();
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO courses (`course_name`, `course_number`) VALUES (@newCourseName, @newCourseNumber);";
            cmd.Parameters.AddWithValue("@newCourseName", this.CourseName);
            cmd.Parameters.AddWithValue("@newCourseNumber", this.CourseNumber);
            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Course> GetAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM courses;";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Course> allCourses = new List<Course> { };

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
        public static Course Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM courses WHERE id = @searchID;";
            cmd.Parameters.AddWithValue("@searchId", searchId);

            MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read();
            int Id = rdr.GetInt32(0);
            string CourseName = rdr.GetString(1);
            string CourseNumber = rdr.GetString(2);
            Course newCourse = new Course(CourseName, CourseNumber, Id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCourse;
        }
        public void Update(string newCourseName, string newCourseNumber)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE courses SET course_name = @newCourseName, course_number = @newCourseNumber WHERE id = @thisId;";
            cmd.Parameters.AddWithValue("@newCourseName", this.CourseName);
            cmd.Parameters.AddWithValue("@newCourseNumber", this.CourseNumber);
            cmd.Parameters.AddWithValue("@thisId", this.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@courseId, @studentId);
            INSERT INTO students (student_id) VALUES @studentId);";
            cmd.Parameters.AddWithValue("@courseId", this.Id);
            cmd.Parameters.AddWithValue("@studentId", newStudent.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
         public List<Student> GetStudents()
        {
            List<Student> allStudents = new List<Student>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT students.* FROM students
            JOIN courses_students ON (courses_students.course_id = courses.id)
            JOIN students ON (courses_students.student_id = students.id)
            WHERE course.id = @thisId
            ;";
            cmd.Parameters.AddWithValue("@thisId", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader();

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
        public static void DeleteSingular(int deleteId)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"DELETE FROM courses WHERE id = @thisId;";
                cmd.Parameters.AddWithValue("@deleteId", deleteId);

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            public static void DeleteAll(int deleteId)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"DELETE FROM courses;";

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

        }
    }