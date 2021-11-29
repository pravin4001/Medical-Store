using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalProject.Models
{
    public class ViewModel
    {
        public IEnumerable<Sales> sales { get; set; }
    }
    public class Sales
    {
       
        public int SalesMasterId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please Select Name !")]
        public int CustomerId { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Please Select Category !")]
        [DisplayName("Categeory")]
        public int CategeoryId { get; set; }
        [Required(ErrorMessage = "Please Select SubCategory !")]
        [DisplayName("SubCategeory")]
        public int SubCategeoryId { get; set; }
        [Required(ErrorMessage = "Please Select Product !")]
        [DisplayName("Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please Enter Quantity !")]
        [DisplayName("Quantity")]
        [Range(1, 100)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be numeric")]
        public int SalesQuantity { get; set; }

        [Required(ErrorMessage = "Please Select GST !")]
        [DisplayName("GST")]
        public int SalesHSNCODE { get; set; }

      
        public float NetAmount { get; set; }

        [Display(Name = "Payment Method")]
        [Required(ErrorMessage = "Please Select Method !")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Please Select Status !")]
        [DisplayName("Status")]
        public int StatusId { get; set; }

       
        public DateTime SalesCreateDate { get; set; }

        public DateTime SalesUpdateDate { get; set; }

        public Boolean IsSalesActive { get; set; }

        public int SalesId { get; set; }


    }
}