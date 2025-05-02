using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class StudentController : Controller
    {
        private readonly FEEEntities1 db = new FEEEntities1();

        // GET: Student Dashboard
        public ActionResult StudentDashboard()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            string enrollmentNo = student.EnrollmentNo?.Trim().ToLower();

            bool hasPersonalDetails = db.PersonalDetailsTbls
                .Any(p => p.EnrollmentNo.Trim().ToLower() == enrollmentNo);

            ViewBag.HasPersonalDetails = hasPersonalDetails;

            return View(student);
        }






        public ActionResult PersonalDetails()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login"); // Redirect to login page if not logged in
            }

            int studentId = (int)Session["StudentId"]; // Now it's safe


            // Fetch student details based on session data (StudentId)

            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            // Try to fetch existing personal details
            var personalDetails = db.PersonalDetailsTbls.SingleOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                // If no personal details found, create a new entry to pre-fill the form
                personalDetails = new PersonalDetailsTbl
                {
                    StudentName = student.Name,
                    FathersName = student.FatherName,
                    MothersName = student.MotherName,
                    EnrollmentNo = student.EnrollmentNo,
                    //AdmitCardNo = student.StudentID,
                    Gender = student.Gender,
                    DateOfBirth = student.DOB,
                    stu_whatsappnum = student.WhatsAppNo,
                    StudentEmail = student.Email,
                    Program = student.ProgramName,
                    Caste = student.Caste,
                    AadharNumber = student.AadhaarNo,
                    StudentContactNo = student.Mobile,
                    Batch = student.Batch,
                    University = student.University
                };

                //db.PersonalDetailsTbls.Add(personalDetails);
                //db.SaveChanges();
            }

            return View(personalDetails);
        }

        // POST: Submit Personal Details

        [HttpPost]
        public ActionResult SubmitPersonalDetails(PersonalDetailsTbl personalDetails, string[] Past_Health_History, string AllergyNames)
        {
            if (ModelState.IsValid)
            {
                // Handle Past Health History
                if (Past_Health_History != null && Past_Health_History.Length > 0)
                {
                    // Join the array values into a single string (comma-separated)
                    personalDetails.Past_Health_History = string.Join(",", Past_Health_History.Where(h => !string.IsNullOrWhiteSpace(h)));
                }
                else
                {
                    personalDetails.Past_Health_History = null;
                }

                // Handle Allergy Names
                if (!string.IsNullOrEmpty(AllergyNames))
                {
                    personalDetails.AllergyNames = AllergyNames; // This will be a comma-separated string
                }
                else
                {
                    personalDetails.AllergyNames = null;
                }

                // Debugging: Log the form data being received
                Debug.WriteLine("Submitting Personal Details...");
                Debug.WriteLine($"Past_Health_History: {personalDetails.Past_Health_History}");
                Debug.WriteLine($"AllergyNames: {personalDetails.AllergyNames}");

                // Debugging: Check the values in Past_Health_History
                Debug.WriteLine("Past_Health_History values:");
                foreach (var item in Past_Health_History)
                {
                    Debug.WriteLine($"Item: {item}");
                }

                // Check if the personal details already exist for the student
                var existingPersonalDetails = db.PersonalDetailsTbls.SingleOrDefault(p => p.EnrollmentNo == personalDetails.EnrollmentNo);

                if (existingPersonalDetails != null)
                {
                    // Update all relevant fields
                    existingPersonalDetails.AdmitCardNo = personalDetails.AdmitCardNo;
                    existingPersonalDetails.AdmissionNo = personalDetails.AdmissionNo;
                    existingPersonalDetails.RegistrationNo = personalDetails.RegistrationNo;
                    existingPersonalDetails.StudentName = personalDetails.StudentName;
                    existingPersonalDetails.FathersName = personalDetails.FathersName;
                    existingPersonalDetails.MothersName = personalDetails.MothersName;
                    existingPersonalDetails.Father_Age = personalDetails.Father_Age;
                    existingPersonalDetails.Mother_Age = personalDetails.Mother_Age;
                    existingPersonalDetails.LocalGuardianAge = personalDetails.LocalGuardianAge;
                    existingPersonalDetails.AdmissionYear = personalDetails.AdmissionYear;
                    existingPersonalDetails.Program = personalDetails.Program;
                    existingPersonalDetails.Branch = personalDetails.Branch;
                    existingPersonalDetails.Year = personalDetails.Year;
                    existingPersonalDetails.RoomNo = personalDetails.RoomNo;
                    existingPersonalDetails.Age = personalDetails.Age;
                    existingPersonalDetails.Height = personalDetails.Height;
                    existingPersonalDetails.Weight = personalDetails.Weight;
                    existingPersonalDetails.Gender = personalDetails.Gender;
                    existingPersonalDetails.BloodGroup = personalDetails.BloodGroup;
                    existingPersonalDetails.DateOfBirth = personalDetails.DateOfBirth;
                    existingPersonalDetails.PresentResidentialAddress = personalDetails.PresentResidentialAddress;
                    existingPersonalDetails.StudentAddress_Communication = personalDetails.StudentAddress_Communication;
                    existingPersonalDetails.Nationality = personalDetails.Nationality;
                    existingPersonalDetails.Religion = personalDetails.Religion;
                    existingPersonalDetails.Father_Mobile_No = personalDetails.Father_Mobile_No;
                    existingPersonalDetails.Mother_Mobile_No = personalDetails.Mother_Mobile_No;
                    existingPersonalDetails.StudentContactNo = personalDetails.StudentContactNo;
                    existingPersonalDetails.FathersEmail = personalDetails.FathersEmail;
                    existingPersonalDetails.MothersEmail = personalDetails.MothersEmail;
                    existingPersonalDetails.StudentEmail = personalDetails.StudentEmail;
                    existingPersonalDetails.LocalGuardianName = personalDetails.LocalGuardianName;
                    existingPersonalDetails.LocalGuardianResidentialAddress = personalDetails.LocalGuardianResidentialAddress;
                    existingPersonalDetails.LocalGuardianContactNo = personalDetails.LocalGuardianContactNo;
                    existingPersonalDetails.LocalGuardianEmail = personalDetails.LocalGuardianEmail;
                    existingPersonalDetails.LocalGuardianRelationWithStudent = personalDetails.LocalGuardianRelationWithStudent;
                    existingPersonalDetails.LocalGuardianOccupation = personalDetails.LocalGuardianOccupation;
                    existingPersonalDetails.Parents_Income_Category = personalDetails.Parents_Income_Category;
                    existingPersonalDetails.Work_Experience = personalDetails.Work_Experience;
                    existingPersonalDetails.Past_Health_History = personalDetails.Past_Health_History;
                    existingPersonalDetails.Details_Of_Past_Health_History = personalDetails.Details_Of_Past_Health_History;
                    existingPersonalDetails.AllergyNames = personalDetails.AllergyNames;  // Updated allergy names
                    existingPersonalDetails.History_Of_Substance_Allergy = personalDetails.History_Of_Substance_Allergy;
                    existingPersonalDetails.History_of_drug_allergy = personalDetails.History_of_drug_allergy;


                    db.SaveChanges();
                    Debug.WriteLine("Personal details updated successfully.");
                    TempData["Message"] = "Personal details Updated Successfully";
                    TempData["AlertType"] = "SUCCESS";
                }
                else
                {
                    db.PersonalDetailsTbls.Add(personalDetails);
                    db.SaveChanges();
                    Debug.WriteLine("Personal details inserted successfully.");
                    TempData["Message"] = "Personal details Inserted Successfully";
                    TempData["AlertType"] = "SUCCESS";
                }
                /*TempData["Message"] = "Personal details Updated Successfully";
                TempData["AlertType"] = "SUCCESS";*/
                return RedirectToAction("StudentDashboard");
            }
            TempData["Message"] = "Failed to Update Profile";
            TempData["AlertType"] = "DANGER";
            // If the model is invalid, return the form with entered values
            return View(personalDetails);
        }

        // GET: MbaStudent/PersonalData
        public ActionResult MBAstuPersonalData()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var enrollmentNo = student.EnrollmentNo; // Enrollment No is STRING

            var data = db.PersonalDetailsTbls
                         .Where(p => p.EnrollmentNo == enrollmentNo)  // ✅ String == String
                         .Select(p => new mba_student_data
                         {
                             name = p.StudentName,
                             enrollment_no = p.EnrollmentNo,
                             date_of_birth = p.DateOfBirth,
                             mobile_no = p.stu_whatsappnum,
                             email_id = p.StudentEmail,
                             blood_group = p.BloodGroup,
                             nationality = p.Nationality,
                             religion = p.Religion,
                             father_name = p.FathersName,
                             father_mobile_no = p.Father_Mobile_No,
                             father_email_id = p.FathersEmail,
                             mother_name = p.MothersName,
                             mother_mobile_no = p.Mother_Mobile_No,
                             mother_email_id = p.MothersEmail,
                             address_for_communication = p.StudentAddress_Communication,
                             local_guardian_name = p.LocalGuardianName,
                             local_guardian_mobile = p.LocalGuardianContactNo,
                             local_guardian_email_id = p.LocalGuardianEmail,
                             local_guardian_address = p.LocalGuardianResidentialAddress,
                             parents_income_category = p.Parents_Income_Category,
                             work_experience = p.Work_Experience
                         })
                         .FirstOrDefault();

            if (data == null)
            {
                ViewBag.Message = "No data found for this user.";
                return View("Error");
            }

            return View("MBAstuPersonalData", data);
        }


        // GET: MbaStudent/UndertakingForm
        public ActionResult UndertakingFormbyParent()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            var viewModel = new Undertakingbyparent
            {
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                // Place and Date are already defaulted in ViewModel
            };

            return View(viewModel);
        }



        // GET: MbaStudent/UndertakingFormbyParentandguardian
        public ActionResult UndertakingFormbyParent_gau()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            // Prefill view model
            var viewModel = new undertakingbypar_gua
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",

                // Leave these fields blank
                StudentSignature1 = "",
                VerificationPlace = "",
                VerificationDay = DateTime.Now.Day.ToString(),      // Default to today's day
                VerificationMonth = DateTime.Now.ToString("MMMM"),  // Default to current month name
                VerificationYear = DateTime.Now.Year.ToString(),    // Default to current year
                StudentSignature2 = ""
            };

            return View(viewModel);

        }


        // GET: MbaStudent/UndertakingFormbyParentandguardian
        public ActionResult undertakingbyparVC()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            var viewModel = new undertakingbyparV_C_
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                Program = personalDetails.Program,
                Branch = personalDetails.Branch,
                Year = personalDetails.Year,
                RoomNo = personalDetails.RoomNo,

                // Guardian fields left blank for user to fill
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianRelation = personalDetails.LocalGuardianRelationWithStudent,
                LocalGuardianOccupation = personalDetails.LocalGuardianOccupation,
                LocalGuardianAddress = personalDetails.LocalGuardianResidentialAddress,
                LocalGuardianPhone = personalDetails.LocalGuardianContactNo,

                // Signatures
                StudentSignature1 = "",
                VerificationPlace = "",
                VerificationDay = DateTime.Now.Day.ToString(),
                VerificationMonth = DateTime.Now.ToString("MMMM"),
                VerificationYear = DateTime.Now.Year.ToString(),
                StudentSignature2 = "",

                ParentEmail = personalDetails.FathersEmail ?? personalDetails.MothersEmail ?? "Parent/Guardian",
                ParentMobile = personalDetails.Father_Mobile_No ?? personalDetails.Mother_Mobile_No ?? "Parent/Guardian",
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            return View(viewModel);
        }


        // GET: MbaStudent/UndertakingFormbyParentandguardian
        public ActionResult UndertakingFormbystuVF()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            // Prefill view model
            var viewModel = new undertakingbystuV_F_
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                StudentAddress=personalDetails.StudentAddress_Communication,
                StudentContact=personalDetails.StudentContactNo,
                // Leave these fields blank
                StudentSignature1 = "",
                VerificationPlace = "",
                VerificationDay = DateTime.Now.Day.ToString(),      // Default to today's day
                VerificationMonth = DateTime.Now.ToString("MMMM"),  // Default to current month name
                VerificationYear = DateTime.Now.Year.ToString(),    // Default to current year
                StudentSignature2 = ""
            };

            return View(viewModel);

        }

        // GET: MbaStudent/UndertakingFormbyParentandguardian
        public ActionResult UndertakingFormbystuVA()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            // Prefill view model
            var viewModel = new UndertakingbyparVA
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                ParentAddressLine1=personalDetails.PresentResidentialAddress,
                ParentAge=personalDetails.Father_Age?? personalDetails.Mother_Age,
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianAge=personalDetails.LocalGuardianAge,
                LocalGuardianAddressLine1=personalDetails.LocalGuardianResidentialAddress

                //StudentAddress = personalDetails.StudentAddress_Communication,
                //StudentContact = personalDetails.StudentContactNo,
                // Leave these fields blank

            };

            return View(viewModel);

        }



        // GET: MbaStudent/HostelAllotmentRequestForm
        public ActionResult HostelAllotmentRequestForm()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            // Prefill view model
            var viewModel = new Hostel_allot_form
            {
                AdmitCardNo = personalDetails.AdmitCardNo,   // assuming you have it
                EnrollmentNo = personalDetails.EnrollmentNo,
                StudentName = personalDetails.StudentName,
                FatherName = personalDetails.FathersName,
                MotherName = personalDetails.MothersName,
                AdmissionYear = personalDetails.Year, // assuming you have admission year field
                BloodGroup = personalDetails.BloodGroup,       // assuming you have it
                DateOfBirth = personalDetails.DateOfBirth,
                PresentResidentialAddress = personalDetails.PresentResidentialAddress,
                ContactNumber = personalDetails.StudentContactNo,
                FatherEmailAddress= personalDetails.FathersEmail,     // assuming you have these fields
                MotherEmailAddress = personalDetails.MothersEmail,
                StudentEmailAddress = personalDetails.StudentEmail,
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianAddress = personalDetails.LocalGuardianResidentialAddress,
                LocalGuardianContactNumber = personalDetails.LocalGuardianContactNo,
                LocalGuardianEmailAddress = personalDetails.LocalGuardianEmail
            };

            return View(viewModel);
        }


        // GET: MbaStudent/HealthRecord
        public ActionResult HealthRecord()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            var viewModel = new Healthhistoryform
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                RoomNo = personalDetails.RoomNo,
                StudentContactNo = personalDetails.StudentContactNo,
                Age = personalDetails.Age,
                Gender = personalDetails.Gender,
                Height = personalDetails.Height,
                Weight = personalDetails.Weight,
                BloodGroup = personalDetails.BloodGroup,
                Past_Health_History = personalDetails.Past_Health_History ?? "",   // safe fallback
                Details_Of_Past_Health_History = personalDetails.Details_Of_Past_Health_History ?? "",
                //AllergyNames = personalDetails.AllergyNames ?? "",
                History_Of_sub_Allergies = personalDetails.History_Of_Substance_Allergy ?? "",
                History_Of_drug_Allergies = personalDetails.History_of_drug_allergy ?? "",
                //Date = Date.Now.ToString("dd/MM/yyyy")

            };

            return View(viewModel);
        }

        public ActionResult HostelRulesRegulations2018()
        {
            return View();
        }

        public ActionResult Antiragging()
        {
            return View();
        }

        public ActionResult sis()
        {
            return View();
        }

        public ActionResult WLD()
        {
            return View();
        }

    }

}
