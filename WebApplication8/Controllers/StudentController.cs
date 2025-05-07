using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Rotativa;
using WebApplication8.Models;

using System.IO.Compression;


using Ionic.Zip; // or use System.IO.Compression

using iTextSharp.text.pdf;
using System.Net;

namespace WebApplication8.Controllers
{
    public class StudentController : Controller
    {
        private readonly FEEEntities db = new FEEEntities();

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
            // Log session state for debugging
            Debug.WriteLine($"StudentDashboard: StudentId={studentId}, EnrollmentNo={enrollmentNo}, HasDownloadedForms={Session["HasDownloadedForms"]?.ToString() ?? "NULL"}");
            return View(student);
        }







        // GET: PersonalDetails
        public ActionResult PersonalDetails()
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

            bool hasDownloadedForms = Session["HasDownloadedForms"]?.ToString() == "true";


            // Try to fetch existing personal details
            var personalDetails = db.PersonalDetailsTbls.SingleOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);

            if (personalDetails == null)
            {
                personalDetails = new PersonalDetailsTbl
                {
                    StudentName = student.Name,
                    Student_Name__as_per_10th_grade_sheet_ = student.Name,
                    FathersName = student.FatherName,
                    MothersName = student.MotherName,
                    EnrollmentNo = student.EnrollmentNo,
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
            }

            Debug.WriteLine($"PersonalDetails: Rendering form for StudentId={studentId}, EnrollmentNo={student.EnrollmentNo}");
            return View(personalDetails);
        }

        // POST: Submit Personal Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitPersonalDetails(PersonalDetailsTbl personalDetails, string[] Past_Health_History, string AllergyNames)
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId = (int)Session["StudentId"];

            // Check if editing is blocked
            if (Session["HasDownloadedForms"]?.ToString() == "true")
            {
                TempData["Message"] = "You cannot edit your personal details after downloading the forms.";
                TempData["AlertType"] = "danger";
                Debug.WriteLine($"SubmitPersonalDetails: Edit blocked for StudentId={studentId}, HasDownloadedForms=true");
                return RedirectToAction("StudentDashboard");
            }

            if (ModelState.IsValid)
            {
                // Handle Past Health History
                if (Past_Health_History != null && Past_Health_History.Length > 0)
                {
                    personalDetails.Past_Health_History = string.Join(",", Past_Health_History.Where(h => !string.IsNullOrWhiteSpace(h)));
                }
                else
                {
                    personalDetails.Past_Health_History = null;
                }

                // Handle Allergy Names
                if (!string.IsNullOrEmpty(AllergyNames))
                {
                    personalDetails.AllergyNames = AllergyNames;
                }
                else
                {
                    personalDetails.AllergyNames = null;
                }

                // Log form data for debugging
                Debug.WriteLine($"SubmitPersonalDetails: StudentId={studentId}, EnrollmentNo={personalDetails.EnrollmentNo}, Past_Health_History={personalDetails.Past_Health_History}, AllergyNames={personalDetails.AllergyNames}");

                // Check if personal details already exist
                var existingPersonalDetails = db.PersonalDetailsTbls.SingleOrDefault(p => p.EnrollmentNo == personalDetails.EnrollmentNo);

                if (existingPersonalDetails != null)
                {
                    // Update all relevant fields
                    existingPersonalDetails.AdmitCardNo = personalDetails.AdmitCardNo;
                    existingPersonalDetails.AdmissionNo = personalDetails.AdmissionNo;
                    existingPersonalDetails.RegistrationNo = personalDetails.RegistrationNo;
                    existingPersonalDetails.StudentName = personalDetails.StudentName;
                    existingPersonalDetails.Student_Name__as_per_10th_grade_sheet_ = personalDetails.Student_Name__as_per_10th_grade_sheet_;
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
                    existingPersonalDetails.AllergyNames = personalDetails.AllergyNames;
                    existingPersonalDetails.History_Of_Substance_Allergy = personalDetails.History_Of_Substance_Allergy;
                    existingPersonalDetails.History_of_drug_allergy = personalDetails.History_of_drug_allergy;

                    db.SaveChanges();
                    TempData["Message"] = "Personal details updated successfully.";
                    TempData["AlertType"] = "success";
                    Debug.WriteLine($"SubmitPersonalDetails: Updated details for StudentId={studentId}");
                }
                else
                {
                    db.PersonalDetailsTbls.Add(personalDetails);
                    db.SaveChanges();
                    TempData["Message"] = "Personal details inserted successfully.";
                    TempData["AlertType"] = "success";
                    Debug.WriteLine($"SubmitPersonalDetails: Inserted details for StudentId={studentId}");
                }

                return RedirectToAction("StudentDashboard");
            }

            TempData["Message"] = "Failed to update personal details. Please check the form.";
            TempData["AlertType"] = "danger";
            Debug.WriteLine($"SubmitPersonalDetails: ModelState invalid for StudentId={studentId}");
            return View("PersonalDetails", personalDetails);
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

            var enrollmentNo = student.EnrollmentNo;

            var data = db.PersonalDetailsTbls
                         .Where(p => p.EnrollmentNo == enrollmentNo)
                         .Select(p => new mba_student_data
                         {
                             Student_Name__as_per_10th_grade_sheet_ = p.Student_Name__as_per_10th_grade_sheet_,
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

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("MBAstuPersonalData", data)
                {
                    FileName = "MBAstuPersonalData.pdf"
                };
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
                EnrollmentNo = personalDetails.EnrollmentNo
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("UndertakingFormbyParent", viewModel)
                {
                    FileName = "UndertakingFormbyParent.pdf"
                };
            }

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

            var viewModel = new undertakingbypar_gua
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                StudentSignature1 = "",
                VerificationPlace = "",
                VerificationDay = DateTime.Now.Day.ToString(),
                VerificationMonth = DateTime.Now.ToString("MMMM"),
                VerificationYear = DateTime.Now.Year.ToString(),
                StudentSignature2 = ""
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("UndertakingFormbyParent_gau", viewModel)
                {
                    FileName = "UndertakingFormbyParent_gau.pdf"
                };
            }

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
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianRelation = personalDetails.LocalGuardianRelationWithStudent,
                LocalGuardianOccupation = personalDetails.LocalGuardianOccupation,
                LocalGuardianAddress = personalDetails.LocalGuardianResidentialAddress,
                LocalGuardianPhone = personalDetails.LocalGuardianContactNo,
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

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("undertakingbyparVC", viewModel)
                {
                    FileName = "undertakingbyparVC.pdf"
                };
            }

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

            var viewModel = new undertakingbystuV_F_
            {
                StudentName = personalDetails.StudentName,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                StudentAddress = personalDetails.StudentAddress_Communication,
                StudentContact = personalDetails.StudentContactNo,
                StudentSignature1 = "",
                VerificationPlace = "",
                VerificationDay = DateTime.Now.Day.ToString(),
                VerificationMonth = DateTime.Now.ToString("MMMM"),
                VerificationYear = DateTime.Now.Year.ToString(),
                StudentSignature2 = ""
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("UndertakingFormbystuVF", viewModel)
                {
                    FileName = "UndertakingFormbystuVF.pdf"
                };
            }

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

            var viewModel = new UndertakingbyparVA
            {
                StudentName = personalDetails.StudentName,
                StudentProgram = personalDetails.Program,
                EnrollmentNo = personalDetails.EnrollmentNo,
                ParentName = personalDetails.FathersName ?? personalDetails.MothersName ?? "Parent/Guardian",
                ParentAddressLine1 = personalDetails.PresentResidentialAddress,
                ParentAge = personalDetails.Father_Age ?? personalDetails.Mother_Age,
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianAge = personalDetails.LocalGuardianAge,
                LocalGuardianAddressLine1 = personalDetails.LocalGuardianResidentialAddress
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("UndertakingFormbystuVA", viewModel)
                {
                    FileName = "UndertakingFormbystuVA.pdf"
                };
            }

            return View(viewModel);
        }





        // GET: MbaStudent/HostelAllotmentRequestForm
        public ActionResult HostelAllotmentRequestForm(int? studentId)
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentId1 = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentId1);

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

            var viewModel = new Hostel_allot_form
            {
                AdmitCardNo = personalDetails.AdmitCardNo,
                EnrollmentNo = personalDetails.EnrollmentNo,
                StudentName = personalDetails.StudentName,
                FatherName = personalDetails.FathersName,
                MotherName = personalDetails.MothersName,
                AdmissionYear = personalDetails.Year,
                BloodGroup = personalDetails.BloodGroup,
                DateOfBirth = personalDetails.DateOfBirth,
                PresentResidentialAddress = personalDetails.PresentResidentialAddress,
                ContactNumber = personalDetails.StudentContactNo,
                FatherEmailAddress = personalDetails.FathersEmail,
                MotherEmailAddress = personalDetails.MothersEmail,
                StudentEmailAddress = personalDetails.StudentEmail,
                LocalGuardianName = personalDetails.LocalGuardianName,
                LocalGuardianAddress = personalDetails.LocalGuardianResidentialAddress,
                LocalGuardianContactNumber = personalDetails.LocalGuardianContactNo,
                LocalGuardianEmailAddress = personalDetails.LocalGuardianEmail
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("HostelAllotmentRequestForm", viewModel)
                {
                    FileName = "HostelAllotmentRequestForm.pdf"
                };
            }

            return View(viewModel);
        }




        // GET: MbaStudent/HealthRecord
        // GET: MbaStudent/HealthRecord
        public ActionResult HealthRecord(int? studentId)
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentIdFromSession = (int)Session["StudentId"];
            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentIdFromSession);

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
                Past_Health_History = personalDetails.Past_Health_History ?? "",
                Details_Of_Past_Health_History = personalDetails.Details_Of_Past_Health_History ?? "",
                History_Of_sub_Allergies = personalDetails.History_Of_Substance_Allergy ?? "",
                History_Of_drug_Allergies = personalDetails.History_of_drug_allergy ?? ""
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("HealthRecord", viewModel)
                {
                    FileName = "HealthRecord.pdf"
                };
            }

            return View(viewModel);
        }

        public ActionResult HostelRulesRegulations2018()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentIdFromSession = (int)Session["StudentId"];

            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentIdFromSession);
            if (student == null)
            {
                return HttpNotFound();
            }

            // OPTIONAL: If you still want personalDetails for something else
            var personalDetails = db.PersonalDetailsTbls.FirstOrDefault(p => p.EnrollmentNo == student.EnrollmentNo);
            if (personalDetails == null)
            {
                ViewBag.Message = "Personal details not found.";
                return View("Error");
            }

            // Populate the view model from StudentTbl
            var viewModel = new HostelRules2018
            {
                Name = student.Name,
                ProgramName = student.ProgramName,
                AppNo = student.AppNo
            };
            if (Request.QueryString["generatePdf"] == "true")
            {
                var pdfResult = new Rotativa.ViewAsPdf("HostelRulesRegulations2018", viewModel)
                {
                    FileName = "HostelRulesRegulations2018.pdf"
                };
                return pdfResult;
            }

            return View(viewModel);
        }


        public ActionResult Antiragging()
        {
            if (Request.QueryString["generatePdf"] == "true")
            {
                var pdfResult = new Rotativa.ViewAsPdf("Antiragging")
                {
                    FileName = "Antiragging.pdf"
                };
                return pdfResult;
            }

            return View();
        }


        public ActionResult sis()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentIdFromSession = (int)Session["StudentId"];

            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentIdFromSession);
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

            var viewModel = new StudentInfoSystem
            {
                StudentName = personalDetails.StudentName
            };
            if (Request.QueryString["generatePdf"] == "true")
            {
                var pdfResult = new Rotativa.ViewAsPdf("sis", viewModel)
                {
                    FileName = "sis.pdf"
                };
                return pdfResult;
            }

            return View(viewModel);
        }

        public ActionResult WLD()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Loginpage", "Login");
            }

            int studentIdFromSession = (int)Session["StudentId"];

            var student = db.StudentTbls.FirstOrDefault(s => s.Id == studentIdFromSession);
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

            var viewModel = new WelcomeLetterForm
            {
                StudentName = personalDetails.StudentName
            };

            if (Request.QueryString["generatePdf"] == "true")
            {
                return new Rotativa.ViewAsPdf("WLD", viewModel)
                {
                    FileName = "WLD.pdf"
                };
            }

            return View(viewModel);
        }







        //    public ActionResult DownloadAllFormsAsZip()
        //    {
        //        if (Session["StudentId"] == null)
        //        {
        //            return RedirectToAction("Loginpage", "Login");
        //        }

        //        int studentId = (int)Session["StudentId"];

        //        // List of form names and their corresponding controller actions
        //        var forms = new List<(string fileName, string action)>
        //{
        //    ("01_PersonalData.pdf", "MBAstuPersonalData"),
        //    ("02_UndertakingByParent.pdf", "UndertakingFormbyParent"),
        //    ("03_UndertakingByParentGuardian.pdf", "UndertakingFormbyParent_gau"),
        //    ("04_UndertakingByParentVC.pdf", "undertakingbyparVC"),
        //    ("05_UndertakingByStudentVF.pdf", "UndertakingFormbystuVF"),
        //    ("06_UndertakingByStudentVA.pdf", "UndertakingFormbystuVA"),
        //    ("07_HostelAllotmentForm.pdf", "HostelAllotmentRequestForm"),
        //    ("08_HealthRecord.pdf", "HealthRecord"),
        //    ("09_HostelRules.pdf", "HostelRulesRegulations2018"),
        //    ("10_AntiRagging.pdf", "Antiragging"),
        //    ("11_SISForm.pdf", "sis"),
        //    ("12_WLDForm.pdf", "WLD")
        //};

        //        string tempDir = Server.MapPath("~/TempPDFs");
        //        if (!Directory.Exists(tempDir))
        //            Directory.CreateDirectory(tempDir);

        //        // Generate PDFs
        //        foreach (var (fileName, action) in forms)
        //        {
        //            var pdfPath = Path.Combine(tempDir, fileName);

        //            // Use ActionAsPdf to generate the PDF
        //            var pdfResult = new Rotativa.ActionAsPdf(action, new { studentId = studentId })
        //            {
        //                FileName = fileName
        //            };

        //            // Generate the PDF and save it to the temp directory
        //            var binary = pdfResult.BuildFile(ControllerContext);
        //            System.IO.File.WriteAllBytes(pdfPath, binary);
        //        }

        //        // Create ZIP file containing all the PDFs
        //        string zipPath = Path.Combine(tempDir, "AllForms.zip");
        //        if (System.IO.File.Exists(zipPath))
        //            System.IO.File.Delete(zipPath);

        //        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
        //        {
        //            zip.AddDirectory(tempDir, "Forms");
        //            zip.Save(zipPath);
        //        }

        //        return File(zipPath, "application/zip", "All_Forms.zip");
        //    }

        public ActionResult DownloadAllFormsAsZip()
        {
            var studentId = Session["StudentId"]?.ToString();
            if (string.IsNullOrEmpty(studentId))
                return new HttpStatusCodeResult(401, "Student not logged in.");

            var enrollmentNo = Session["enrollment_no"]?.ToString();
            string tempFolder = Server.MapPath($"~/App_Data/Temp/{studentId}/");
            string zipFilePath = Server.MapPath($"~/App_Data/StudentDetails_{studentId}.zip");

            // Log session variables for debugging
            try
            {
                System.IO.File.WriteAllText(
                    Path.Combine(tempFolder, "SessionDebug.txt"),
                    $"StudentId: {studentId}, EnrollmentNo: {enrollmentNo ?? "NULL"}, HasDownloadedForms: {Session["HasDownloadedForms"]?.ToString() ?? "NULL"}"
                );
            }
            catch { /* Ignore logging errors */ }

            // Clean and recreate temp folder
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            var viewActions = new List<string>
        {
            "MBAstuPersonalData", "UndertakingFormbyParent", "UndertakingFormbyParent_gau", "undertakingbyparVC",
            "UndertakingFormbystuVF", "sis", "WLD", "Antiragging", "HostelRulesRegulations2018",
            "HealthRecord", "HostelAllotmentRequestForm", "UndertakingFormbystuVA"
        };

            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string cookieHeader = Request.Headers["Cookie"];

            foreach (var actionName in viewActions)
            {
                try
                {
                    string url = $"{baseUrl}/Student/{actionName}?generatePdf=true";

                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 300000; // 5 minutes
                    request.ReadWriteTimeout = 300000;
                    request.Headers.Add("Cookie", cookieHeader);

                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        byte[] pdfBytes = ms.ToArray();

                        // Basic PDF check
                        if (pdfBytes.Length < 100 || pdfBytes[0] != 0x25 || pdfBytes[1] != 0x50)
                        {
                            System.IO.File.WriteAllText(
                                Path.Combine(tempFolder, $"{actionName}_ERROR.txt"),
                                "Failed to generate PDF. File did not start with %PDF header."
                            );
                            continue;
                        }

                        string pdfPath = Path.Combine(tempFolder, $"{actionName}.pdf");
                        System.IO.File.WriteAllBytes(pdfPath, pdfBytes);
                    }
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(
                        Path.Combine(tempFolder, $"{actionName}_ERROR.txt"),
                        ex.ToString()
                    );
                }
            }

            // Create ZIP file
            if (System.IO.File.Exists(zipFilePath))
                System.IO.File.Delete(zipFilePath);

            System.IO.Compression.ZipFile.CreateFromDirectory(tempFolder, zipFilePath);

            // Set session variable to block editing
            Session["HasDownloadedForms"] = true;
            Debug.WriteLine($"DownloadAllFormsAsZip: Set HasDownloadedForms=true for StudentId={studentId}");

            // Determine filename with fallback
            string fileName = string.IsNullOrEmpty(enrollmentNo)
                ? $"{studentId}_Details.zip"
                : $"{enrollmentNo}_Details.zip";

            // Return ZIP file to user
            byte[] zipBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return File(zipBytes, "application/zip", fileName);
        }
    }
}