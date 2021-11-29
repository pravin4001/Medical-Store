using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Login
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter User Name !")]
        [Display(Name ="Enter Username :")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password !")]
        [Display(Name = "Enter Password :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Boolean IsActive { get; set; }



    }
}