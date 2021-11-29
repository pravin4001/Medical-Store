using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Customer
    {
        
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please Enter Name !")]
        [DisplayName("Name")]
       
        [RegularExpression(@"^[a-z A-Z\S]+$", ErrorMessage = "Use letters only please")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please Enter Address !")]
       
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Pin Code !")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Please Enter Valid Postal Code.")]

        [Display(Name = "Pin Code")]
        public int PinCode { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number !")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Email !")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [DisplayName("Email")]
        public string Email { get; set; }

       
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Boolean IsAvailable { get; set; }
    }
}