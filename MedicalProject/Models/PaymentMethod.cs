using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

         [Required(ErrorMessage = "Please Enter Name !")]
        [DisplayName("Name")]
        public string MethodName { get; set; }

       
        public DateTime MethodCreatedDate { get; set; }

        public DateTime MethodUpdatedDate { get; set; }

        public Boolean IsValid { get; set; }
    }
}