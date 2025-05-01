using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class UndertakingbyparVA
    {

        
           

            
            public string ParentName { get; set; }

            
            public Nullable<int> ParentAge { get; set; }

            
            public string ParentAddressLine1 { get; set; }

            
            public string ParentAddressLine2 { get; set; }

            
            public string StudentName { get; set; }

        public string EnrollmentNo { get; set; }

       
            public string StudentProgram { get; set; }

            
            public string LocalGuardianName { get; set; }

            
            public Nullable<int> LocalGuardianAge { get; set; }

           
            public string LocalGuardianAddressLine1 { get; set; }

            
            public string LocalGuardianAddressLine2 { get; set; }
        }
    }

