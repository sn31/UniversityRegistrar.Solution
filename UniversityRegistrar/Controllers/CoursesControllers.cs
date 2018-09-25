using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            Dictionary<string, object> model = new Dictionary<string, object> { };
            List<Course> allCourses = Course.GetAll();
            List<Student> allStudents = Student.GetAll();
            model.Add("courses", allCourses);
            model.Add("students", allStudents);
            return View(model);
        }

        [HttpGet("/courses/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/courses")]
        public ActionResult Create()
        {
            string newName = Request.Form["newName"];
            string newNumber = Request.Form["newNumber"];
            Course newCourse = new Course(newName, newNumber);
            newCourse.Save();
            return RedirectToAction("Index");
        }

        // [HttpPost("/courses")]
        // public ActionResult CreateForm(int courseId)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Student newStudent = Student.Find(courseId);
        //     return  RedirectToAction("Index");
        // }
    }
}