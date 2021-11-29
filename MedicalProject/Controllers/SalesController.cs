using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using MedicalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Document = iTextSharp.text.Document;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace MedicalProject.Controllers
{

    public class SalesController : Controller
    {

        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: Sales

        [HttpGet]
        public ActionResult SalesView()
        {
            if (Session["UserName"] != null)
            {


                DataTable dtblCategory = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();

                    SqlDataAdapter sqlData = new SqlDataAdapter("select SalesId, CustomerName,Name,CategoryName,SalesQuantity,Price,BaseAmount,SalesCGST,SalesSGST,TotalGST,NetAmount,MethodName,StatusName from Sales,Customer,Product,PaymentMethod,CategoryMaster,Status where IsSalesActive=1 and Customer.CustomerId=Sales.CustomerId and Product.ProductId=Sales.ProductId and PaymentMethod.PaymentMethodId = Sales.PaymentMethodId and Status.StatusId=Sales.StatusId and CategoryMaster.MainCategoryId=Sales.CategoryId", sqlCon);
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
        public ActionResult SalesView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Sales set IsSalesActive=0 where SalesId=@SalesId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@SalesId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("SalesView");
        }


        [HttpGet]
        public ActionResult SalesInsert()
        {
            if (Session["UserName"] != null)
            {


                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT CustomerId,CustomerName FROM Customer";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["CustomerName"].ToString(),
                                    Value = sdr["CustomerId"].ToString()
                                });
                            }
                        }
                        ViewBag.customer = list;
                    }
                }

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


                return View(new Sales());
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult SalesInsert(Sales sales)
        {


            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();


                DataTable datatable = new DataTable();
                int str = sales.ProductId;

                SqlDataAdapter sdata = new SqlDataAdapter("Select Price,ProductHSNCODE,CGST,SGST,Quantity From Product,GST where Product.ProductId= " + str + " and Product.ProductHSNCODE=GST.HSNCODE;", sqlCon);
                sdata.Fill(datatable);

                float b = (float)Convert.ToDouble(datatable.Rows[0]["Price"]);
                int HSNCODE = Convert.ToInt32(datatable.Rows[0]["ProductHSNCODE"]);
                float CGST = (float)Convert.ToDouble(datatable.Rows[0]["CGST"]);
                float SGST = (float)Convert.ToDouble(datatable.Rows[0]["SGST"]);
                int quantity = Convert.ToInt32(datatable.Rows[0]["Quantity"]);

                int a = sales.SalesQuantity;



                float amount = (a * b);
                int totalquantity = quantity - a;

                // Add GST: GST Amount = (Original Cost x GST %)/ 100.
                // Net Price = Original Cost + GST Amount



                float CGSTAmount = (amount * CGST) / 100;
                float SGSTAmount = (amount * SGST) / 100;

                float TotalGST = CGSTAmount + SGSTAmount;

                sales.NetAmount = amount + TotalGST;

                int productId = sales.ProductId;

                string query = "Insert INTO Sales Values(@CustomerId,@OrderDate,@CategoryId,@SubCategoryId,@ProductId,@SalesQuantity,@BaseAmount,@SalesCGST,@SalesSGST,@TotalGST,@NetAmount,@PaymentMethodId,@StatusId,@SalesCreateDate,@SalesUpdateDate,@IsSalesActive)";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@CustomerId", sales.CustomerId);
                sqlcmd.Parameters.AddWithValue("@OrderDate", sales.OrderDate);
                sqlcmd.Parameters.AddWithValue("@CategoryId", sales.CategeoryId);
                sqlcmd.Parameters.AddWithValue("@SubCategoryId", sales.SubCategeoryId);
                sqlcmd.Parameters.AddWithValue("@ProductId", sales.ProductId);
                sqlcmd.Parameters.AddWithValue("@SalesQuantity", sales.SalesQuantity);
                sqlcmd.Parameters.AddWithValue("@BaseAmount", amount);
                sqlcmd.Parameters.AddWithValue("@SalesCGST", CGSTAmount);
                sqlcmd.Parameters.AddWithValue("@SalesSGST", SGSTAmount);
                sqlcmd.Parameters.AddWithValue("@TotalGST", TotalGST);
                sqlcmd.Parameters.AddWithValue("@NetAmount", sales.NetAmount);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", sales.PaymentMethodId);
                sqlcmd.Parameters.AddWithValue("@StatusId", sales.StatusId);
                sqlcmd.Parameters.AddWithValue("@SalesCreateDate", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@SalesUpdateDate", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@IsSalesActive", 1);

                sqlcmd.ExecuteNonQuery();

                string updatequantity = "Update Product set Quantity=@Quantity where ProductId=" + productId;
                SqlCommand sqlcmdproduct = new SqlCommand(updatequantity, sqlCon);

                sqlcmdproduct.Parameters.AddWithValue("@Quantity", totalquantity);
                sqlcmdproduct.ExecuteNonQuery();
            }
            return RedirectToAction("SalesView");

        }

        public ActionResult SalesUpdate(int id)
        {
            if (Session["UserName"] != null)
            {

                using (SqlConnection sqlcon = new SqlConnection(constring))
                {
                    sqlcon.Open();
                    List<SelectListItem> list = new List<SelectListItem>();
                    string query = " SELECT CustomerId,CustomerName FROM Customer";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {


                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = sdr["CustomerName"].ToString(),
                                    Value = sdr["CustomerId"].ToString()
                                });
                            }
                        }
                        ViewBag.customer = list;
                    }
                }

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


                Sales sales = new Sales();
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();
                    string query1 = "Select * from Sales where SalesId=@SalesId";
                    SqlDataAdapter sqldata = new SqlDataAdapter(query1, sqlCon);
                    sqldata.SelectCommand.Parameters.AddWithValue("@SalesId", id);
                    sqldata.Fill(dtbl);

                }

                sales.SalesId = Convert.ToInt32(dtbl.Rows[0][0]);
                sales.CustomerId = Convert.ToInt32(dtbl.Rows[0][1]);
                sales.OrderDate = Convert.ToDateTime(dtbl.Rows[0][2]);
                sales.CategeoryId = Convert.ToInt32(dtbl.Rows[0][3]);
                sales.SubCategeoryId = Convert.ToInt32(dtbl.Rows[0][4]);
                sales.ProductId = Convert.ToInt32(dtbl.Rows[0][5]);
                sales.SalesQuantity = Convert.ToInt32(dtbl.Rows[0][6]);

                sales.PaymentMethodId = Convert.ToInt32(dtbl.Rows[0][12]);
                sales.StatusId = Convert.ToInt32(dtbl.Rows[0][13]);



                return View(sales);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }


        }

        [HttpPost]
        public ActionResult SalesUpdate(Sales sales)
        {



            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                DataTable datatable = new DataTable();
                int str = sales.ProductId;

                SqlDataAdapter sdata = new SqlDataAdapter("Select Price,ProductHSNCODE,CGST,SGST,Quantity From Product,GST where Product.ProductId= " + str + " and Product.ProductHSNCODE=GST.HSNCODE;", sqlCon);
                sdata.Fill(datatable);

                float b = (float)Convert.ToDouble(datatable.Rows[0]["Price"]);
                int HSNCODE = Convert.ToInt32(datatable.Rows[0]["ProductHSNCODE"]);
                float CGST = (float)Convert.ToDouble(datatable.Rows[0]["CGST"]);
                float SGST = (float)Convert.ToDouble(datatable.Rows[0]["SGST"]);
                int Quantity = Convert.ToInt32(datatable.Rows[0]["Quantity"]);
                int a = sales.SalesQuantity;

                float amount = (a * b);

                int totalquantity = Quantity - a;
                // Add GST: GST Amount = (Original Cost x GST %)/ 100.Net Price = Original Cost + GST Amount


                float CGSTAmount = (amount * CGST) / 100;
                float SGSTAmount = (amount * SGST) / 100;

                float TotalGST = CGSTAmount + SGSTAmount;

                sales.NetAmount = amount + TotalGST;

                int productid = sales.ProductId;

                string query = "Update Sales set CustomerId=@CustomerId,OrderDate=@OrderDate, CategoryId=@CategoryId ,SubCategoryId=@SubCategoryId,ProductId=@ProductId,SalesQuantity=@SalesQuantity,PaymentMethodId=@PaymentMethodId,StatusId=@StatusId,BaseAmount=@BaseAmount,SalesCGST=@SalesCGST,SalesSGST=@SalesSGST,TotalGST=@TotalGST,NetAmount = @NetAmount,SalesUpdateDate=@SalesUpdateDate where SalesId = @SalesId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@SalesId", sales.SalesId);
                sqlcmd.Parameters.AddWithValue("@CustomerId", sales.CustomerId);
                sqlcmd.Parameters.AddWithValue("@OrderDate", sales.OrderDate);
                sqlcmd.Parameters.AddWithValue("@CategoryId", sales.CategeoryId);
                sqlcmd.Parameters.AddWithValue("@SubCategoryId", sales.SubCategeoryId);
                sqlcmd.Parameters.AddWithValue("@ProductId", sales.ProductId);
                sqlcmd.Parameters.AddWithValue("@SalesQuantity", sales.SalesQuantity);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", sales.PaymentMethodId);
                sqlcmd.Parameters.AddWithValue("@StatusId", sales.StatusId);
                sqlcmd.Parameters.AddWithValue("@BaseAmount", amount);
                sqlcmd.Parameters.AddWithValue("@SalesCGST", CGSTAmount);
                sqlcmd.Parameters.AddWithValue("@SalesSGST", SGSTAmount);
                sqlcmd.Parameters.AddWithValue("@TotalGST", TotalGST);
                sqlcmd.Parameters.AddWithValue("@NetAmount", sales.NetAmount);
                sqlcmd.Parameters.AddWithValue("@SalesUpdateDate", DateTime.Now);




                sqlcmd.ExecuteNonQuery();

                string updatequantity = "Update Product set Quantity=@Quantity where ProductId=" + productid;
                SqlCommand sqlcmdproduct = new SqlCommand(updatequantity, sqlCon);

                sqlcmdproduct.Parameters.AddWithValue("@Quantity", totalquantity);
                sqlcmdproduct.ExecuteNonQuery();

            }


            return RedirectToAction("SalesView");
        }

        public ActionResult SalesDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Sales where SalesId = @SalesId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@SalesId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("SalesView");


        }
        [HttpGet]
        public ActionResult Salesdata()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }

        [HttpPost]
        public ActionResult Salesdata(Sales sales)
        {
            var order = (List<int>)TempData["OrderValues"] ?? new List<int>();

            int[] list = { sales.CategeoryId, sales.SubCategeoryId, sales.ProductId, sales.SalesQuantity };
            order.AddRange(list);

            TempData["OrderValues"] = order;

            ViewBag.data = TempData["OrderValues"];
            TempData.Keep("OrderValues");
            //order.Add(sales.CategeoryId);
            //order.Add(sales.SubCategeoryId);
            //order.Add(sales.ProductId);
            //order.Add(sales.SalesQuantity);

            //// save for next post
            //TempData["OrderValues"] = order;

            //ViewBag.data = TempData["OrderValues"];
            //TempData.Keep("OrderValues");




            //var list = (List<Sales>)TempData["ProductItem"] ?? new List<Sales>();


            //list.Add(new Sales
            //{CategeoryId=sales.CategeoryId,SubCategeoryId=sales.SubCategeoryId, ProductId = sales.ProductId, SalesQuantity = sales.SalesQuantity });

            //ViewModel mymodel = new ViewModel();
            //mymodel.sales = list;
            //// save for next post
            //TempData["ProductItem"] = list;
            //ViewBag.list = TempData["ProductItem"];
            //TempData.Keep("RetainedValues");
            //return View(sales);
            return RedirectToAction("SalesInsert");
        }

        [Obsolete]
        public ActionResult Report(int id)
        {


            DataTable report = new DataTable();
            using (SqlConnection connect = new SqlConnection(constring))
            {
                connect.Open();

                string reportdata = "select SalesId,CustomerName,Address,PhoneNumber,Email,OrderDate,Name,SalesQuantity,BaseAmount,SalesCGST,SalesSGST,TotalGST,NetAmount,MethodName,StatusName from Sales,Product,Customer,PaymentMethod,Status where Customer.CustomerId=Sales.CustomerId and PaymentMethod.PaymentMethodId=Sales.PaymentMethodId and Product.ProductId=Sales.ProductId and Status.StatusId=Sales.StatusId and Sales.SalesId = @SalesId";



                SqlCommand reports = new SqlCommand(reportdata, connect);
                reports.Parameters.AddWithValue("@SalesId", id);
                SqlDataAdapter reportss = new SqlDataAdapter(reports);
                reportss.Fill(report);
                int SalesId = Convert.ToInt32(report.Rows[0]["SalesId"]);
                string CustomerName = (report.Rows[0]["CustomerName"]).ToString();
                string Address = (report.Rows[0]["Address"]).ToString();
                string PhoneNumber = (report.Rows[0]["PhoneNumber"]).ToString();
                string Email = (report.Rows[0]["Email"]).ToString();
                string OrderDate = (report.Rows[0]["OrderDate"]).ToString();
                string Name = (report.Rows[0]["Name"]).ToString();
                int SalesQuantity =Convert.ToInt32(report.Rows[0]["SalesQuantity"]);
               
                float BaseAmount = (float)Convert.ToDouble(report.Rows[0]["BaseAmount"]);
                float SalesCGST = (float)Convert.ToDouble(report.Rows[0]["SalesCGST"]);
                float SalesSGST = (float)Convert.ToDouble(report.Rows[0]["SalesSGST"]);
                float TotalGST = (float)Convert.ToDouble(report.Rows[0]["TotalGST"]);
                float NetAmount = (float)Convert.ToDouble(report.Rows[0]["NetAmount"]);
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
                        sb.Append(OrderDate);
                        sb.Append("</td></tr>");
                       
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
                        sb.Append("Base Price(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("CGST Amount(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("SGST Amount(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Total GST Price(Rs)");
                        sb.Append("</th>");
                        sb.Append("<th style = background-color : #D2080C;color:#ffffff>");
                        sb.Append("Net Amount(Rs)");
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
                        sb.Append(SalesQuantity);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(BaseAmount);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(SalesCGST);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(SalesSGST);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(TotalGST);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(NetAmount);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(MethodName);
                        sb.Append("</td>");
                        sb.Append("<td align = 'center'>");
                        sb.Append(StatusName);
                        sb.Append("</td>");

                        sb.Append("</tr></table>");
                        sb.Append("<div style='float:right;'><h4 align = 'right' ><b>TOTAL COST :</b>");
                        sb.Append(Convert.ToDouble(report.Rows[0]["NetAmount"]) + "( RS");
                        sb.Append("</h4></div>");





                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
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
                            var data = "Customer Name :- \n" + CustomerName + "\n" + Address + "\n" + PhoneNumber + "\n" + Email;
                            Paragraph p = new Paragraph(data, FontFactory.GetFont("Times New Roman", 14f));
                            p.SpacingBefore = 20;
                            p.SpacingAfter = 20;
                            // p.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc.Add(p);
                            var date = "Order Date :" + OrderDate;
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



                            string email = "select count(*) from Customer where Email = @Email";
                            SqlCommand emailname = new SqlCommand(email, connect);
                            emailname.Parameters.AddWithValue("@Email", Email);
                            object num = emailname.ExecuteScalar();
                            int nums = Convert.ToInt32(num);
                            if (nums > 0)
                            {
                                using (MailMessage mail = new MailMessage())
                                {
                                    String senderEmail = System.Configuration.ConfigurationManager.AppSettings["smtpUserEmail"].ToString();
                                    String senderPassword = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"].ToString();

                                    mail.From = new MailAddress(senderEmail);
                                    mail.To.Add(Email);
                                    mail.Subject = "Your Order Successfully Added";
                                    mail.Body = "<b>Hi</b> <h2>" + CustomerName + "</h2> ,Thanks for choosing Pharmacy! Your order has been placed succesfully.";



                                    mail.IsBodyHtml = true;
                                    //string attach = "C:/Users/Pravin/Downloads/Invoice_" + PurchaseId + ".pdf";

                                    mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "Invoice_" + SalesId + ".pdf"));
                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                                        smtp.EnableSsl = true;
                                        //ssmtp.UseDefaultCredentials = false;
                                        smtp.Send(mail);
                                    }
                                }
                                MessageBox.Show("Mail Send on " + Email + " Successfully.");
                            }
                            else
                            {
                                MessageBox.Show(CustomerName + "Mail Id Not Exists!");
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
                            var data1 = "Customer Name :- \n" + CustomerName + "\n" + Address + "\n" + PhoneNumber + "\n" + Email;
                            Paragraph p1 = new Paragraph(data, FontFactory.GetFont("Times New Roman", 14f));
                            p1.SpacingBefore = 20;
                            p1.SpacingAfter = 20;
                            // p.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc1.Add(p1);
                            var date1 = "Order Date :" + OrderDate;
                            Paragraph da1 = new Paragraph(date, FontFactory.GetFont("Times New Roman", 14f));
                            da1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                            pdfDoc1.Add(da1);
                            htmlparser1.Parse(sr1);
                            pdfDoc1.Close();



                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + SalesId + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc1);
                            Response.End();

                        }

                    }
                }
            }


            return RedirectToAction("OrderIndex");



        }


    }
}