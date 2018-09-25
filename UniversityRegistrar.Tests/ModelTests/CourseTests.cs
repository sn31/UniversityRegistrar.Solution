using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{

    [TestClass]
    public class CourseTest : IDisposable
    {
        public CourseTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
        }
        public void Dispose()
        {
            Course.DeleteAll();
            Student.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = Course.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_TrueForSameDescription_Course()
        {
            //Arrange, Act
            Course firstCourse = new Course("School work", "Econ101");
            Course secondCourse = new Course("School work", "Econ101");

            //Assert
            Assert.AreEqual(firstCourse, secondCourse);
        }

        [TestMethod]
        public void Save_CourseSavesToDatabase_CourseList()
        {
            //Arrange
            Course testCourse = new Course("Mow the lawn", "Econ101");
            testCourse.Save();

            //Act
            List<Course> result = Course.GetAll();
            List<Course> testList = new List<Course> { testCourse };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            //Arrange
            Course testCourse = new Course("Mow the lawn", "Econ101");
            testCourse.Save();

            //Act
            Course savedCourse = Course.GetAll() [0];

            int result = savedCourse.Id;
            int testId = testCourse.Id;

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsCourseInDatabase_Course() //Test is failing for no reason?
        {
            //Arrange
            Course testCourse = new Course("Mow the lawn", "Econ101");
            testCourse.Save();

            //Act
            Course result = Course.Find(testCourse.Id);

            //Assert
            Assert.AreEqual(testCourse, result);
        }

        [TestMethod]
        public void Delete_DeletesCourseAssociationsFromDatabase_CourseList()
        {
            Student testStudent = new Student("Mow the lawn", DateTime.MinValue);
            testStudent.Save();

            string testName = "Home stuff";
            Course testCourse = new Course(testName, "Econ101");
            testCourse.Save();

            testCourse.AddStudent(testStudent);
            testCourse.Delete();

            List<Course> resultStudentCourses = testStudent.GetCourses();
            List<Course> testStudentCourses = new List<Course> { };
            CollectionAssert.AreEqual(testStudentCourses, resultStudentCourses);
        }

        [TestMethod]
        public void Test_AddStudent_AddsStudentToCourse()
        {
            Course testCourse = new Course("Household chores", "Econ101");
            testCourse.Save();

            Student testStudent = new Student("Mow the lawn", DateTime.MinValue);
            testStudent.Save();

            Student testStudent2 = new Student("Water the garden", DateTime.MinValue);
            testStudent2.Save();

            //Act
            testCourse.AddStudent(testStudent);
            testCourse.AddStudent(testStudent2);
            List<Student> result = testCourse.GetStudents();
            List<Student> testList = new List<Student> { testStudent, testStudent2 };
            Console.WriteLine(result.Count);
            Console.WriteLine(testList.Count);
            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetStudents_ReturnsAllCourseStudents_StudentList()
        {
            //Arrange
            Course testCourse = new Course("Household chores", "Econ101");
            testCourse.Save();

            Student testStudent1 = new Student("Mow the lawn", DateTime.MinValue);
            testStudent1.Save();

            Student testStudent2 = new Student("Buy plane ticket", DateTime.MinValue);
            testStudent2.Save();

            //Act
            testCourse.AddStudent(testStudent1);
            List<Student> savedStudents = testCourse.GetStudents();
            List<Student> testList = new List<Student> { testStudent1 };

            //Assert
            CollectionAssert.AreEqual(testList, savedStudents);
        }
    }
}