using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class undertakingbyparV_C_
    {
        public string StudentName { get; set; }
        public string EnrollmentNo { get; set; }
        public string ParentName { get; set; }
        public string Program { get; set; }
        public string Branch { get; set; }
        public Nullable<int> Year { get; set; }
        public string RoomNo { get; set; }

        public string LocalGuardianName { get; set; }
        public string LocalGuardianRelation { get; set; }
        public string LocalGuardianOccupation { get; set; }
        public string LocalGuardianAddress { get; set; }
        public string LocalGuardianPhone { get; set; }

        public string StudentSignature1 { get; set; }
        public string VerificationPlace { get; set; }
        public string VerificationDay { get; set; }
        public string VerificationMonth { get; set; }
        public string VerificationYear { get; set; }
        public string StudentSignature2 { get; set; }

        public string ParentEmail { get; set; }
        public string ParentMobile { get; set; }
        public string Date { get; set; }
    }
}