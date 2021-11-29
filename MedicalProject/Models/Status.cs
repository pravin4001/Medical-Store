using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        [DisplayName(" Name ")]
        [Required(ErrorMessage = "Please Enter Status Name !")]
        public string StatusName { get; set; }

      
        public string StatusCreated { get; set; }

        public string StatusUpdated { get; set; }

        public Boolean IsOption { get; set; }
    }
}