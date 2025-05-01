using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Hostel_allot_form
    {

        public int Id { get; set; }
        public string AdmitCardNo { get; set; }
        public string EnrollmentNo { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public Nullable<int> AdmissionYear { get; set; }
        public string BloodGroup { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PresentResidentialAddress { get; set; }
        public string ContactNumber { get; set; }
        public string FatherEmailAddress { get; set; }
        public string MotherEmailAddress { get; set; }
        public string StudentEmailAddress { get; set; }
        public string LocalGuardianName { get; set; }
        public string LocalGuardianAddress { get; set; }
        public string LocalGuardianContactNumber { get; set; }
        public string LocalGuardianEmailAddress { get; set; }
    }
}