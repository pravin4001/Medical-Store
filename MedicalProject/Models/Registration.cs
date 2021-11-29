using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Registration
    {
        public int UserId { get; set; }


        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 3)]
        public String Name { get; set; }


        [Display(Name = "User Name ")]
        [Required(ErrorMessage = "User Name is Required")]
        [StringLength(50, MinimumLength = 3)]
        public String UserName { get; set; }


        [DisplayName("Password ")]
        [Required(ErrorMessage = "Password is Required")]
        [Range(1, 4, ErrorMessage = "Invalid Password.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Is Not match")]
        public string ConfirmPassword { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Boolean IsActive { get; set; }



    }
}