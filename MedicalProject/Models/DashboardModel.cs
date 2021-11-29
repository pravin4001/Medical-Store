using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class DashboardModel
    {
       
       [Display (Name ="From")]
        public DateTime startdate { get; set; }

        [Display(Name = "To")]
        public DateTime enddate { get; set; }
        public int Product_count { get; set; }
        public int Purchase_count { get; set; }
        public int Sales_count { get; set; }
        public int Customer_count { get; set; }
    }
}