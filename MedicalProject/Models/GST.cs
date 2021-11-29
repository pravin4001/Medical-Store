using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class GST
    {
        [Required(ErrorMessage = "Please Enter HSNCODE !")]
     
        [DisplayName("HSNCDE")]
        public int HSNCode { get; set; }

   
        [Required(ErrorMessage = "Please Enter CGST")]
        [DisplayName("CGST")]
        public float CGST { get; set; }
      
        [Required(ErrorMessage = "Please Enter SGST !")]
        [DisplayName("SGST")]
        public float SGST { get; set; }

        [Required(ErrorMessage = "Please Enter IGST !")]
        [DisplayName("IGST")]
        public float IGST { get; set; }

       
        public DateTime CreationDate { get; set; }

        public DateTime UpdationDate { get; set; }

        public Boolean ISActivate { get; set; }

        [Display(Name = "IS Tax Free")]
        public Boolean IsExempted { get; set; }
    }
}

