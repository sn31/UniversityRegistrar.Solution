using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{

    [TestClass]
    public class StudentTest : IDisposable
    {
        public StudentTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
        }
        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = Student.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_TrueForSameDescription_Student()
        {
            //Arrange, Act
            Student firstStudent = new Student("Cow", DateTime.MinValue);
            Student secondStudent = new Student("Cow", DateTime.MinValue);

            //Assert
            Assert.AreEqual(firstStudent, secondStudent);
        }

        [TestMethod]
        public void Save_StudentSavesToDatabase_StudentList()
        {
            //Arrange
            Student testStudent = new Student("Cow", DateTime.MinValue);
            testStudent.Save();

            //Act
            List<Student> result = Student.GetAll();
            List<Student> testList = new List<Student> { testStudent };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            //Arrange
            Student testStudent = new Student("Cow", DateTime.MinValue);
            testStudent.Save();

            //Act
            Student savedStudent = Student.GetAll() [0];

            int result = savedStudent.Id;
            int testId = testStudent.Id;

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsStudentInDatabase_Student() //Test is failing for no reason?
        {
            //Arrange
            Student testStudent = new Student("Cow", DateTime.MinValue);
            testStudent.Save();

            //Act
            Student result = Student.Find(testStudent.Id);

            //Assert
            Assert.AreEqual(testStudent, result);
        }

        [TestMethod]
        public void AddCourse_AddsCourseToStudent_CourseList()
        {
            Student testStudent = new Student("Cow", DateTime.MinValue);
            testStudent.Save();

            Course testCourse = new Course("Economics","ECON 101");
            testCourse.Save();

            testStudent.AddCourse(testCourse);
            Console.WriteLine("Im here!");
            List<Course> result = testStudent.GetCourses();
            Console.WriteLine("Im here?!");
            List<Course> testList = new List<Course> { testCourse };

            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetCourses_ReturnsAllStudentCourses_CourseList()
        {
            Student testStudent = new Student("Mow the lawn", DateTime.MinValue);
            testStudent.Save();

            Course testCourse = new Course("Home stuff", "Econ101");
            testCourse.Save();

            testStudent.AddCourse(testCourse);
            List<Course> result = testStudent.GetCourses();
            List<Course> testList = new List<Course> { testCourse };
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesStudentAssociationsFromDatabase_StudentList()
        {
            Course testCourse = new Course("Home stuff","Something");
            testCourse.Save();

            string testDescription = "Mow the lawn";
            Student testStudent = new Student(testDescription,DateTime.MinValue);
            testStudent.Save();
            testStudent.AddCourse(testCourse);
            testStudent.Delete();

            List<Student> resultCourseStudents = testCourse.GetStudents();
            List<Student> testCourseStudents = new List<Student> { };

            //Assert
            CollectionAssert.AreEqual(testCourseStudents, resultCourseStudents);
        }
    }
}