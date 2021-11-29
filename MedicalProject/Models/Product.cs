using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [DisplayName("Categeory")]
        [Required]
        public string CategoryId { get; set; }

        [DisplayName("SubCategeory")]
        [Required]
        public string SubCategoryId { get; set; }

        [Required(ErrorMessage = "Please Enter Name !")]
        [DisplayName(" Name")]
       
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Company !")]
        [DisplayName("Company")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Please Enter Quantity !")]
        [DisplayName(" Quantity")]
        [Range(1, 100)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be numeric")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please Enter Price !")]
        [Display(Name = "Selling Price")]
        [Range(typeof(Decimal), "1", "9999", ErrorMessage = "Price xx.xx")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Please Enter Purchase Price !")]
        [Display(Name = "Purchase Price")]
        [Range(typeof(Decimal), "1", "9999", ErrorMessage = "Price xx.xx")]
        public float PurchasePrice { get; set; }

        public HttpPostedFileBase UploadFile { get; set; }

        [DisplayName("Image")]
        public string ProductImage { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreateDate { get; set; }
        
        public DateTime UpdateTime { get; set; }

        public bool IsActive { get; set; }


        [Display(Name = "GST Type")]
        public string HSNCODE { get; set; }

    }
}