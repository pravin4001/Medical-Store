using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class Purchase 
    {
        public int PurchaseId { get; set; }

        [DisplayName("Supplier")]
        [Required(ErrorMessage = "Please Select Supplier !")]
        public string SupplierId { get; set; }

        [Required(ErrorMessage = "Please Select Date !")]
        [DisplayName("Date")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category !")]
        public string CategoryId { get; set; }

        [DisplayName("SubCategory")]
        [Required(ErrorMessage = "Please Select SubCategory !")]
        public string SubCategoryId { get; set; }

        [DisplayName("Product")]
        [Required(ErrorMessage = "Please Select Product !")]
        public string ProductId { get; set; }

        [DisplayName("Quantity")]
        [Range(1, 100)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be numeric")]
        [Required(ErrorMessage = "Please Select Quantity !")]
        public int PurchaseQuantity { get; set; }

        [DisplayName("Method")]
        [Required(ErrorMessage = "Please Select Method !")]
        public string PaymentMethodId { get; set; }

        [DisplayName("Status ")]
        [Required(ErrorMessage = "Please Select Status !")]
        public string StatusId { get; set; }

      
        public float TotalAmount { get; set; }

        public float PurchasePrice { get; set; }


        public DateTime CreateDatePurchase { get; set; }

        public DateTime UpdateDatePurchase { get; set; }


        public Boolean ISActive { get; set; }

        //public List<ProductList> Productlist { get; set; }



    }
}