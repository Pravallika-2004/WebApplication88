//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication8.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PenaltyDetailsTbl
    {
        public int PenaltyID { get; set; }
        public string University { get; set; }
        public string ApplicationNo { get; set; }
        public string PenaltyType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string PaymentStatus { get; set; }
        public string EnrollmentNo { get; set; }
    }
}
