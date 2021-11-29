using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Category
    {

        
        public int CategoryId { get; set; }

        [DisplayName("Name")]
        [RegularExpression(@"^[a-z A-Z\S]+$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "please Enter Category Name !")]
        public string CategoryName { get; set; }

        [DisplayName("Description")]
        [RegularExpression(@"^[a-z A-Z\S]+$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "please Enter Description !")]
        public string Description { get; set; }

      
        public Boolean IsAvailable { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Display(Name = "ISActive")]
        public Boolean Active { get; set; }


    }
}