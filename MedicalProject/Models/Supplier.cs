using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [DisplayName("Name")]
        [RegularExpression(@"^[a-z A-Z\S]+$", ErrorMessage = "Use letters only please")]
       
        [Required(ErrorMessage = "Please Enter Name !")]
        public string SupplierName { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Please Enter Address !")]
        [RegularExpression(@"^[a-zA-Z/S]+$", ErrorMessage = "Use letters only please")]

        public string SupplierAddress { get; set; }
       

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Display(Name = "PhoneNo")]
        [Required(ErrorMessage = "Please Enter Phone number !")]
        public string PhoneNumber { get; set; }

        [DisplayName(" Email ")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage = "Please Enter Email !")]
        public string Email { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "Please Enter Bank Name !")]
        public string BankName { get; set; }

        [Display(Name = "IFSC Code ")]
        [Required(ErrorMessage = "Please Enter IFSC Code !")]
        public string IFSCCode { get; set; }

        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Please Enter Valid Bank Account Number.")]
        [Display(Name = "Bank Account No")]
        [Required(ErrorMessage = "Please Enter Account Number !")]
        public string BankAccountNo { get; set; }

         public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Boolean IsActive { get; set; }
        


    }
}