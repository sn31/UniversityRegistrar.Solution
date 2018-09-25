using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            Dictionary<string, object> model = new Dictionary<string, object> { };
            List<Student> allStudents = Student.GetAll();
            model.Add("students", allStudents);
            return View(model);
        }

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
    }
}