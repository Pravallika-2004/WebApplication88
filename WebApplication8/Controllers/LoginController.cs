using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class LoginController : Controller
    {

        private readonly FEEEntities db = new FEEEntities();
        // GET: Login
        public ActionResult Loginpage()
        {
            return View();
        }

        // POST: Validate Login
        [HttpPost]
        public ActionResult ValidateLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = db.StudentTbls
                                      .FirstOrDefault(s => s.EnrollmentNo == model.EnrollmentId
                                                        && s.DOB == model.Dob);

                if (student != null)
                {
                    // You can store the student in session or use a session variable for authentication
                    Session["StudentId"] = student.Id;

                    // Redirect to profile or dashboard page after successful login
                    return RedirectToAction("studentdashboard", "Student");
                }
                else
                {
                    // If login fails, show an error message
                    ModelState.AddModelError("", "Invalid Enrollment ID or Date of Birth.");
                }
            }
            return View("Loginpage"); // Return back to login page if validation fails
        }

    }
}