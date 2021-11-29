using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MedicalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows;

namespace MedicalProject.Controllers
{
    public class PurchaseController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: Purchase
        [HttpGet]
        public ActionResult PurchaseView()
        {
            if (Session["UserName"] != null)
            {


                DataTable dtblCategory = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();

                    SqlDataAdapter sqlData = new SqlDataAdapter("select PurchaseId,SupplierName,PurchaseDate,CategoryName,SubCategoryName,Name,PurchaseQuantity,PurchaseNet,TotalAmount,MethodName,StatusName from Purchase,Product,CategoryMaster,SubCategory,Supplier,PaymentMethod,Status where PurchaseActive=1 and CategoryMaster.MainCategoryId=Purchase.CategoryId and SubCategory.Id=Purchase.SubCategoryId and Supplier.SupplierId=Purchase.SupplierId and PaymentMethod.PaymentMethodId=Purchase.PaymentMethodId and Status.StatusId=Purchase.StatusId and Product.ProductId=Purchase.ProductId", sqlCon);
                    sqlData.Fill(dtblCategory);
                }

                return View(dtblCategory);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult PurchaseView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Purchase set PurchaseActive=0 where PurchaseId=@PurchaseId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@PurchaseId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("PurchaseView");
        }


        [HttpGet]
        public ActionResult PurchaseInsert()
        {
            if (Session["UserName"] != null)
            {
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();



                    List<SelectListItem> Category = new List<SelectListItem>();
                    string query = " SELECT MainCategoryId,CategoryName FROM CategoryMaster";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Category.Add(new SelectListItem
                                {
                                    Text = sdr["CategoryName"].ToString(),
                                    Value = sdr["MainCategoryId"].ToString()
                                });
                            }
                        }
                        ViewBag.catlog = Category;
                    }


                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> SubCategory = new List<SelectListItem>();
                    string query = " SELECT Id,SubCategoryName FROM SubCategory";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                SubCategory.Add(new SelectListItem
                                {
                                    Text = sdr["SubCategoryName"].ToString(),
                                    Value = sdr["Id"].ToString()
                                });
                            }
                        }
                        ViewBag.subcatlog = SubCategory;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> Supplyer = new List<SelectListItem>();
                    string query = " SELECT SupplierId, SupplierName FROM Supplier";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Supplyer.Add(new SelectListItem
                                {
                                    Text = sdr["SupplierName"].ToString(),
                                    Value = sdr["SupplierId"].ToString()
                                });
                            }
                        }
                        ViewBag.suply = Supplyer;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT ProductId, Name FROM Product";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["Name"].ToString(),
                                    Value = sdr["ProductId"].ToString()
                                });
                            }
                        }
                        ViewBag.product = list;
                    }
                }

                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT PaymentMethodId, MethodName FROM PaymentMethod";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["MethodName"].ToString(),
                                    Value = sdr["PaymentMethodId"].ToString()
                                });
                            }
                        }
                        ViewBag.payment = list;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT StatusId, StatusName FROM Status";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["StatusName"].ToString(),
                                    Value = sdr["StatusId"].ToString()
                                });
                            }
                        }
                        ViewBag.status = list;
                    }
                }

                return View(new Purchase());
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult PurchaseInsert(Purchase purches)
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();



                DataTable datatable = new DataTable();
                string str = purches.ProductId;

                SqlDataAdapter sdata = new SqlDataAdapter("Select PurchasePrice,Quantity From Product where Product.ProductId=" + str, sqlCon);
                sdata.Fill(datatable);

                float PurchasePrice = (float)Convert.ToDouble(datatable.Rows[0]["PurchasePrice"]);
                int Quantity = Convert.ToInt32(datatable.Rows[0]["Quantity"]);



                int a = purches.PurchaseQuantity;

                purches.TotalAmount = a * PurchasePrice;

                int totalquantity = Quantity + a;

                string query = "Insert INTO Purchase Values(@SupplierId,@PurchaseDate,@CategoryId,@SubCategoryId,@ProductId,@PurchaseQuantity,@PaymentMethodId,@StatusId,@TotalAmount,@CreateDatePurchase,@UpdateDatePurchase,@PurchaseActive,@PurchaseNet)";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@SupplierId", purches.SupplierId);
                sqlcmd.Parameters.AddWithValue("@PurchaseDate", purches.PurchaseDate);
                sqlcmd.Parameters.AddWithValue("@CategoryId", purches.CategoryId);
                sqlcmd.Parameters.AddWithValue("@SubCategoryId", purches.SubCategoryId);
                sqlcmd.Parameters.AddWithValue("@ProductId", purches.ProductId);
                sqlcmd.Parameters.AddWithValue("@PurchaseQuantity", purches.PurchaseQuantity);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", purches.PaymentMethodId);
                sqlcmd.Parameters.AddWithValue("@StatusId", purches.StatusId);
                sqlcmd.Parameters.AddWithValue("@TotalAmount", purches.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@CreateDatePurchase", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@UpdateDatePurchase", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@PurchaseActive", 1);
                sqlcmd.Parameters.AddWithValue("@PurchaseNet", PurchasePrice);

                string ID = purches.ProductId;

                sqlcmd.ExecuteNonQuery();


                string Updatequantity = "Update Product set Quantity=@Quantity where ProductId=" + ID;
                SqlCommand sqlcmd1 = new SqlCommand(Updatequantity, sqlCon);

                sqlcmd1.Parameters.AddWithValue("@Quantity", totalquantity);

                sqlcmd1.ExecuteNonQuery();
            }
            return RedirectToAction("PurchaseView");

        }

        public ActionResult PurchaseUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();

                    List<SelectListItem> Category = new List<SelectListItem>();
                    string query = " SELECT MainCategoryId,CategoryName FROM CategoryMaster";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Category.Add(new SelectListItem
                                {
                                    Text = sdr["CategoryName"].ToString(),
                                    Value = sdr["MainCategoryId"].ToString()
                                });
                            }
                        }
                        ViewBag.catlog = Category;
                    }


                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> SubCategory = new List<SelectListItem>();
                    string query = " SELECT Id,SubCategoryName FROM SubCategory";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                SubCategory.Add(new SelectListItem
                                {
                                    Text = sdr["SubCategoryName"].ToString(),
                                    Value = sdr["Id"].ToString()
                                });
                            }
                        }
                        ViewBag.subcatlog = SubCategory;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> Supplyer = new List<SelectListItem>();
                    string query = " SELECT SupplierId, SupplierName FROM Supplier";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Supplyer.Add(new SelectListItem
                                {
                                    Text = sdr["SupplierName"].ToString(),
                                    Value = sdr["SupplierId"].ToString()
                                });
                            }
                        }
                        ViewBag.suply = Supplyer;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT ProductId, Name FROM Product";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["Name"].ToString(),
                                    Value = sdr["ProductId"].ToString()
                                });
                            }
                        }
                        ViewBag.product = list;
                    }
                }

                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT PaymentMethodId, MethodName FROM PaymentMethod";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["MethodName"].ToString(),
                                    Value = sdr["PaymentMethodId"].ToString()
                                });
                            }
                        }
                        ViewBag.payment = list;
                    }
                }
                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT StatusId, StatusName FROM Status";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["StatusName"].ToString(),
                                    Value = sdr["StatusId"].ToString()
                                });
                            }
                        }
                        ViewBag.status = list;
                    }
                }

                Purchase purchase = new Purchase();
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();
                    string query1 = "Select * from Purchase where PurchaseId=@PurchaseId";
                    SqlDataAdapter sqldata = new SqlDataAdapter(query1, sqlCon);
                    sqldata.SelectCommand.Parameters.AddWithValue("@PurchaseId", id);
                    sqldata.Fill(dtbl);

                }
                purchase.PurchaseId = Convert.ToInt32(dtbl.Rows[0][0]);
                purchase.SupplierId = dtbl.Rows[0][1].ToString();
                purchase.PurchaseDate = Convert.ToDateTime(dtbl.Rows[0][2]);
                purchase.CategoryId = dtbl.Rows[0][3].ToString();
                purchase.SubCategoryId = dtbl.Rows[0][4].ToString();
                purchase.ProductId = dtbl.Rows[0][5].ToString();
                purchase.PurchaseQuantity = Convert.ToInt32(dtbl.Rows[0][6]);
                purchase.PaymentMethodId = dtbl.Rows[0][7].ToString();
                purchase.StatusId = dtbl.Rows[0][8].ToString();



                return View(purchase);


            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult PurchaseUpdate(Purchase purchase)
        {

            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                DataTable datatable = new DataTable();
                string str = purchase.ProductId;

                SqlDataAdapter sdata = new SqlDataAdapter("Select PurchasePrice,Quantity From Product where Product.ProductId=" + str, sqlCon);
                sdata.Fill(datatable);

                float PurchasePrice = (float)Convert.ToDouble(datatable.Rows[0]["PurchasePrice"]);
                int Quantity = Convert.ToInt32(datatable.Rows[0]["Quantity"]);


                int a = purchase.PurchaseQuantity;
                string ID = purchase.ProductId;

                purchase.TotalAmount = a * PurchasePrice;

                int totalquantity = Quantity + a;


                string query = "Update Purchase set SupplierId=@SupplierId,PurchaseDate=@PurchaseDate, CategoryId=@CategoryId ,SubCategoryId=@SubCategoryId,ProductId=@ProductId,PurchaseQuantity=@PurchaseQuantity,PaymentMethodId=@PaymentMethodId,StatusId=@StatusId,TotalAmount = @TotalAmount,UpdateDatePurchase=@UpdateDatePurchase,PurchaseNet=@PurchaseNet where PurchaseId = @PurchaseId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@PurchaseId", purchase.PurchaseId);
                sqlcmd.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
                sqlcmd.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);
                sqlcmd.Parameters.AddWithValue("@CategoryId", purchase.CategoryId);
                sqlcmd.Parameters.AddWithValue("@SubCategoryId", purchase.SubCategoryId);
                sqlcmd.Parameters.AddWithValue("@ProductId", purchase.ProductId);
                sqlcmd.Parameters.AddWithValue("@PurchaseQuantity", purchase.PurchaseQuantity);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", purchase.PaymentMethodId);
                sqlcmd.Parameters.AddWithValue("@StatusId", purchase.StatusId);
                sqlcmd.Parameters.AddWithValue("@TotalAmount", purchase.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@UpdateDatePurchase", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@PurchaseNet", PurchasePrice);




                sqlcmd.ExecuteNonQuery();

                string Updatequantity = "Update Product set Quantity=@Quantity where ProductId=" + ID;
                SqlCommand sqlcmd1 = new SqlCommand(Updatequantity, sqlCon);

                sqlcmd1.Parameters.AddWithValue("@Quantity", totalquantity);

                sqlcmd1.ExecuteNonQuery();

            }


            return RedirectToAction("PurchaseView");
        }

        public ActionResult PurchaseDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Purchase where PurchaseId = @PurchaseId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@PurchaseId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("PurchaseView");


        }

        [Obsolete]
        public ActionResult Report(int id)
        {
            

            DataTable report = new DataTable();
            using (SqlConnection connect = new SqlConnection(constring))
            {
                connect.Open();

                string reportdata = "select PurchaseId,SupplierName,Address,PhoneNumber,Email,PurchaseDate,Name,PurchaseQuantity,PurchaseNet,TotalAmount,MethodName,StatusName from Purchase,Product,Supplier,PaymentMethod,Status where Supplier.SupplierId=Purchase.SupplierId and PaymentMethod.PaymentMethodId=Purchase.PaymentMethodId and Status.StatusId=Purchase.StatusId and Purchase.PurchaseId=@PurchaseId";



                SqlCommand reports = new SqlCommand(reportdata, connect);
                reports.Parameters.AddWithValue("@PurchaseId", id);
                SqlDataAdapter reportss = new SqlDataAdapter(reports);
                reportss.Fill(report);
                int PurchaseId = Convert.ToInt32(report.Rows[0]["PurchaseId"]);
                string SupplierName = (report.Rows[0]["SupplierName"]).ToString();
                string PurchaseDate = (report.Rows[0]["PurchaseDate"]).ToString();
                string Name = (report.Rows[0]["Name"]).ToString();
                string supplierAddress= (report.Rows[0]["Address"]).ToString();
                string supplierPhone = (report.Rows[0]["PhoneNumber"]).ToString();
                string supplierEmail = (report.Rows[0]["Email"]).ToString();
                int PurchaseQuantity = Convert.ToInt32(report.Rows[0]["PurchaseQuantity"]);
                float PurchaseNet = (float)Convert.ToDouble(report.Rows[0]["PurchaseNet"]);
                float TotalAmount = (float)Convert.ToDouble(report.Rows[0]["TotalAmount"]);
                string MethodName = (report.Rows[0]["MethodName"]).ToString();
                string StatusName = (report.Rows[0]["StatusName"]).ToString();

                //Chunk chunk = new Chunk("INVOICE", FontFactory.GetFont("Times New Roman"));
                //chunk.Font.Size = 70;
                //chunk.Font.Color = BaseColor.RED;

               
                
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();


                        //sb.Append(chunk);

                        sb.Append("<table width= '100%' cellspacing = '0'  margin-Top:'60px'; cellpadding = '2'>");
                        sb.Append("<br/><br/><tr><td colspan ='4'></td></tr>");
                        sb.Append("<tr><td><b></b>");

                        sb.Append("</td><td align = 'right'><b>Date :</b>");
                        sb.Append(PurchaseDate);
                        sb.Append("</td></tr>");
                        //sb.Append("<tr><td colspan = '2'><b>Company Name :</b>");
                        //sb.Append(company);
                        //sb.Append("<br>");
                        //sb.Append("Sai Tower,<br>");
                        //sb.Append("Near Bank Of Maharstra, <br>");
                        //sb.Append("Ambegaon bk, <br>");
                        //sb.Append(" Pune 411041 , Maharastra . <br>");
                        //sb.Append("</td></tr>");
                        sb.Append("</table>");
                        sb.Append("<br/>");
                        sb.Append("<table border = '1'>");
                        sb.Append("<tr>");
                        sb.Append("<th align = 'center' style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Name");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Quantity(Unit)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Net Price(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("TotalAmount(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Payment Method");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Payment Status");
                        sb.Append("</th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(Name);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(PurchaseQuantity);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(PurchaseNet);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(TotalAmount);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(MethodName);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(StatusName);
                        sb.Append("</td>");
                       
                        sb.Append("</tr></table>");
                        sb.Append("<div style='float:right;'><h4 align = 'right' ><b>TOTAL COST :</b>");
                        sb.Append(Convert.ToDouble(report.Rows[0]["TotalAmount"])+"( RS");
                        sb.Append("</h4></div>");





                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                       
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc,memoryStream);
                            pdfDoc.Open();
                            iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance("C:/Users/Pravin/Desktop/Pharmacy-Logo.jpg");
                            PNG.ScalePercent(30f);
                            PNG.SetAbsolutePosition(pdfDoc.PageSize.Width - 46f - 122f, pdfDoc.PageSize.Height - -50f - 216f);
                            pdfDoc.Add(PNG);

                            iTextSharp.text.Paragraph titolo = new iTextSharp.text.Paragraph(" INVOICE ", FontFactory.GetFont("Times New Roman", 22f, Font.BOLD));
                            //titolo.Font.Size = 22f;

                            titolo.Font.Color = BaseColor.RED;
                            titolo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            titolo.SpacingAfter = 40;
                            pdfDoc.Add(titolo);

                            var add = " Pharmacy PVT LMT \n Western Heights, \n Ambegaon bk, \n Pune-411041 \n Email: pharmacyindia@gmail.com ";

                            Paragraph companydetails = new Paragraph(add, FontFactory.GetFont("Times New Roman", 14f));
                            companydetails.SpacingAfter = 20;
                            pdfDoc.Add(companydetails);

                            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 2)));
                            pdfDoc.Add(line);
                            var data = "Supplier Name :- \n" + SupplierName + "\n" + supplierAddress + "\n" + supplierPhone + "\n" + supplierEmail;
                            Paragraph p = new Paragraph(data, FontFactory.GetFont("Times New Roman", 14f));
                            p.SpacingBefore = 20;
                            p.SpacingAfter = 20;
                            // p.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc.Add(p);
                            var date = "Order Date :" + PurchaseDate;
                            Paragraph da = new Paragraph(date, FontFactory.GetFont("Times New Roman", 14f));
                            da.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                            pdfDoc.Add(da);
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                           
                            //Response.ContentType = "application/pdf";
                            //Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + Convert.ToInt32(report.Rows[0]["PurchaseId"]) + ".pdf");
                            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            //Response.Write(pdfDoc);
                            //Response.End();

                            
                            byte[] bytes = memoryStream.ToArray();

                            memoryStream.Close();



                            string email = "select count(*) from Supplier where Email = @supplierEmail";
                            SqlCommand emailname = new SqlCommand(email, connect);
                            emailname.Parameters.AddWithValue("@supplierEmail", supplierEmail);
                            object num = emailname.ExecuteScalar();
                            int nums = Convert.ToInt32(num);
                            if (nums > 0)
                            {
                                using (MailMessage mail = new MailMessage())
                                {
                                    String senderEmail = System.Configuration.ConfigurationManager.AppSettings["smtpUserEmail"].ToString();
                                    String senderPassword = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"].ToString();

                                    mail.From = new MailAddress(senderEmail);
                                    mail.To.Add(supplierEmail);
                                    mail.Subject = "Your Order Successfully Added";
                                    mail.Body = "<b>Hi</b> <h2>" + SupplierName + "</h2> ,Thanks for choosing Pharmacy! Your order has been placed succesfully.";
                                   

                                   
                                    mail.IsBodyHtml = true;
                                    //string attach = "C:/Users/Pravin/Downloads/Invoice_" + PurchaseId + ".pdf";

                                    mail.Attachments.Add(new Attachment(new MemoryStream(bytes),"Invoice_" + PurchaseId + ".pdf"));
                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(senderEmail,senderPassword);
                                        smtp.EnableSsl = true;
                                        //ssmtp.UseDefaultCredentials = false;
                                        smtp.Send(mail);
                                    }
                                }
                                MessageBox.Show("Mail Send on "+supplierEmail+" Successfully.");
                             }
                             else
                             {
                                    MessageBox.Show(SupplierName+"Mail Id Not Exists!");
                             }
                            
                            StringReader sr1 = new StringReader(sb.ToString());
                            iTextSharp.text.Document pdfDoc1 = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc1);
                            PdfWriter writer1 = PdfWriter.GetInstance(pdfDoc1, Response.OutputStream);
                            pdfDoc1.Open();

                            iTextSharp.text.Image PNG1 = iTextSharp.text.Image.GetInstance("C:/Users/Pravin/Desktop/Pharmacy-Logo.jpg");
                            PNG1.ScalePercent(30f);
                            PNG1.SetAbsolutePosition(pdfDoc.PageSize.Width - 46f - 122f, pdfDoc.PageSize.Height - -50f - 216f);
                            pdfDoc1.Add(PNG1);

                            iTextSharp.text.Paragraph titolo1 = new iTextSharp.text.Paragraph(" INVOICE ", FontFactory.GetFont("Times New Roman", 22f, Font.BOLD));
                            //titolo.Font.Size = 22f;

                            titolo1.Font.Color = BaseColor.RED;
                            titolo1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            titolo1.SpacingAfter = 40;
                            pdfDoc1.Add(titolo1);

                            var add1 = " Pharmacy PVT LMT \n Western Heights, \n Ambegaon bk, \n Pune-411041 \n Email: pharmacyindia@gmail.com ";

                            Paragraph companydetails1 = new Paragraph(add1, FontFactory.GetFont("Times New Roman", 14f));
                            companydetails1.SpacingAfter = 20;
                            pdfDoc1.Add(companydetails1);

                            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 2)));
                            pdfDoc1.Add(line1);
                            var data1 = "Supplier Name :- \n" + SupplierName + "\n" + supplierAddress + "\n" + supplierPhone + "\n" + supplierEmail;
                            Paragraph p1 = new Paragraph(data, FontFactory.GetFont("Times New Roman", 14f));
                            p1.SpacingBefore = 20;
                            p1.SpacingAfter = 20;
                            // p.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc1.Add(p1);
                            var date1 = "Order Date :" + PurchaseDate;
                            Paragraph da1 = new Paragraph(date, FontFactory.GetFont("Times New Roman", 14f));
                            da1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                            pdfDoc1.Add(da1);
                            htmlparser1.Parse(sr1);
                            pdfDoc1.Close();


                           
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" +PurchaseId +".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc1);
                            Response.End();

                        }

                    }
                }
            }
            

            //String senderEmail = "";
            //String senderPassword = "";

            //string ReceiverEmail = "pravinpawarpatil@gmail.com";

            //using (MailMessage mm = new MailMessage(senderEmail,ReceiverEmail))
            //{
            //    string sub = "Invoice of Your Order";
            //    string body = "Thank you";



            //    mm.Subject = sub;
            //    mm.Body = body;

            //    mm.IsBodyHtml = false;

            //    using(SmtpClient smtp = new SmtpClient())
            //    {
            //        smtp.Host = "smtp.gmail.com";
            //        smtp.EnableSsl = true;

            //        NetworkCredential cred = new NetworkCredential(senderEmail, senderPassword);
            //        smtp.UseDefaultCredentials = true;
            //        smtp.Credentials = cred;
            //        smtp.Port = 587;
            //        smtp.Send(mm);

            //        ViewBag.message="Mail Send";
            //    }
            //}
                 
           


            return RedirectToAction("OrderIndex");



        }

    }
}