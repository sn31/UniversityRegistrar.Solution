using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet("/students/update")]
        [HttpGet("/students")]
        public ActionResult Index()
        {
            Dictionary<string, object> model = new Dictionary<string, object> { };
            List<Course> allCourses = Course.GetAll();
            List<Student> allStudents = Student.GetAll();
            model.Add("courses", allCourses);
            model.Add("students", allStudents);
            return View(model);
        }
        [HttpGet("/courses/{courseId}/students/new")]
        [HttpGet("/students/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/students")]
        public ActionResult Create()
        {
            string newName = Request.Form["newName"];
            DateTime newDate = Convert.ToDateTime(Request.Form["newDate"]);
            Student newStudent = new Student(newName, newDate);
            newStudent.Save();
            return RedirectToAction("Index");
        }
        [HttpPost("/students/delete/{id}")]
        public ActionResult Delete(int id)
        {
            Student foundStudent = Student.Find(id);

            foundStudent.Delete();
            return RedirectToAction("Index");
        }
        [HttpGet("/students/update/{id}")]
        public ActionResult UpdateForm(int id)
        {
            return View();
        }
        [HttpPost("/students/update/{id}")]
        public ActionResult Update(int id)
        {
            Student foundStudent = Student.Find(id);
            string updateName= Request.Form["updateName"];
            DateTime updateDate = Convert.ToDateTime(Request.Form["updateDate"]);

           foundStudent.Update(updateName,updateDate);
           foundStudent.Save();
           
            return RedirectToAction("Index");
        }
        
    }
}