using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace UniversityRegistrar.Controllers
{
    public class CoursesController:Controller{
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
