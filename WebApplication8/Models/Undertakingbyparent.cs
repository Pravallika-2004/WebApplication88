using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Undertakingbyparent
    {

        public string ParentName { get; set; }
        public string StudentName { get; set; }
        public string EnrollmentNo { get; set; }
        public string Place { get; set; } = "Hyderabad";  // Default to Hyderabad
        public DateTime Date { get; set; } = DateTime.Now; // Default to current date
        public string Signature { get; set; }
    }
}