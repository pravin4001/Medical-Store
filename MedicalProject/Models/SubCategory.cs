using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class SubCategory
    { 
        public int Id { get; set; }

        [ForeignKey("Category")]
        [DisplayName(" Category ")]
        [Required(ErrorMessage = "Please Select Category !")]
        public string CategoryId { get; set; }

        [DisplayName(" Name ")]
        [Required(ErrorMessage = "Please Enter Name !")]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
        public Boolean IsAvailable { get; set; }
    }
    
}