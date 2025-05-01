using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Healthhistoryform
    {

        
        //public int Id { get; set; }

        public string StudentName { get; set; }

        public string EnrollmentNo { get; set; }

        public string RoomNo { get; set; }

        public string StudentContactNo { get; set; }

        public Nullable<int> Age { get; set; }

        public string Gender { get; set; }

        public Nullable<decimal> Height { get; set; }

        public Nullable<decimal> Weight { get; set; }

        public string BloodGroup { get; set; }

        public string Past_Health_History { get; set; }

        public string Details_Of_Past_Health_History { get; set; }

        public string AllergyNames { get; set; } 

        public string History_Of_sub_Allergies { get; set; } // sub Allergy

        public string History_Of_drug_Allergies { get; set; } // Drug Allergy

        public DateTime Date { get; set; } = DateTime.Now; // Default to current date
        public string Signature { get; set; }
    }
}