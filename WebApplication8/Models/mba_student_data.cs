using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class mba_student_data
    {

        // Basic Details
        public string name { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string enrollment_no { get; set; }
        public string mobile_no { get; set; }
        public string email_id { get; set; }
        public string blood_group { get; set; }
        public string nationality { get; set; }
        public string religion { get; set; }

        // Father's Details
        public string father_name { get; set; }
        public string father_mobile_no { get; set; }
        public string father_email_id { get; set; }

        // Mother's Details
        public string mother_name { get; set; }
        public string mother_mobile_no { get; set; }
        public string mother_email_id { get; set; }

        // Address
        public string address_for_communication { get; set; }

        // Local Guardian Details
        public string local_guardian_name { get; set; }
        public string local_guardian_mobile { get; set; }
        public string local_guardian_email_id { get; set; }
        public string local_guardian_address { get; set; }

        // Income Category
        public string parents_income_category { get; set; }

        // Work Experience
        public string work_experience { get; set; }

    }
}