using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace MedicalProject.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAll()
        {
            return Json(GetEmployees(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateReport(int Id)
        {
            string html = "";
            Customer customer = GetEmployees().Where(x => x.Id == Id).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            // Company Logo and Name.
            html += "<div style='text-align:center;'>" +
                    "<img src='" + GetUrl("Files/Logo.png") + "' height='50px' width='100px' />" +
                    "<br/><h2>Excelasoft Solutions</h2><hr/>" +
                    "</div>";

            // Employee Data.
            html += "<table>" +
                    "<tr><th>Id</th><td colspan='2'> : " + customer.Id + "</td></tr>" +
                    "<tr><th>Name</th><td colspan='2'> : " + customer.Name + "</td></tr>" +
                   
                    "<tr><th>Phone No</th><td colspan='2'> : " + customer.PNo + "</td></tr>" +
                    "<tr><th>Location</th><td colspan='2'> : " + customer.Address + "</td></tr>" +
                    "<tr><th>Company</th><td colspan='2'> : " + customer.Price + "</td></tr>" +
                    "</table>";

            using (MemoryStream stream = new MemoryStream())
            {
                StringReader sr = new StringReader(html);
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 50f, 10f, 30f, 10f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                TempData["Data"] = stream.ToArray();
            }

            return new JsonResult() { Data = new { FileName = customer.Name.Trim().Replace(" ", "_") + ".pdf" } };
        }

        [HttpGet]
        public virtual ActionResult Download(string filename)
        {
            if (TempData["Data"] != null)
            {
                byte[] data = TempData["Data"] as byte[];
                return File(data, "application/pdf", filename);
            }
            else
            {
                return new EmptyResult();
            }
        }

        private string GetUrl(string imagePath)
        {
            string appUrl = System.Web.HttpRuntime.AppDomainAppVirtualPath;
            if (appUrl != "/")
            {
                appUrl = "/" + appUrl;
            }
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, appUrl);

            return url + imagePath;
        }

        private List<Customer> GetEmployees()
        {
            List<Customer> employees = new List<Customer>();
            employees.Add(new Customer { Id = 1, Name = "Maria", PNo = "030-0074321", Address = "Austria", Price =22});
           
            return employees;
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
      
            public string PNo { get; set; }
            public string Address { get; set; }
            public float Price { get; set; }
        }
    }
}