using MedicalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace MedicalProject.Controllers
{
    public class ProductController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: Product
        public ActionResult ProductView()
        {
            if (Session["UserName"] != null)
            {
               
          
            DataTable dtblCategory = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select ProductId,Name,Company,CategoryName,Quantity,PurchasePrice,Price,ProductImage,ExpiryDate from Product,CategoryMaster where  CategoryMaster.MainCategoryId=Product.CategoryId", sqlCon);
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
        public ActionResult ProductView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Product set IsActive=0 where ProductId=@ProductId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@ProductId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("ProductView");
        }
        [HttpGet]
        public ActionResult ProductInsert()
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
                List<SelectListItem> gs = new List<SelectListItem>();
                string query = " SELECT HSNCODE FROM GST";
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {


                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            gs.Add(new SelectListItem
                            {
                                Text = sdr["HSNCODE"].ToString(),
                                Value = sdr["HSNCODE"].ToString()
                            });
                        }
                    }
                    ViewBag.gst = gs;
                }
            }
            return View(new Product());
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult ProductInsert(Product product)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select Name from Product where Name=@Name ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@Name", product.Name);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["Name"] = product.Name.ToString();
                ViewData["Message2"] = "ProductName Already Exists !";

                MessageBox.Show("Product Name Already Exist !!");
                return RedirectToAction("ProductInsert");

            }
            else
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();

                    string ProductImage = Path.GetFileNameWithoutExtension(product.UploadFile.FileName);
                    string FileExtension = Path.GetExtension(product.UploadFile.FileName);
                    ProductImage = ProductImage + FileExtension;
                    product.ProductImage = "~/UserImages/" + ProductImage;
                    ProductImage = Path.Combine(Server.MapPath("~/UserImages/"), ProductImage);
                    product.UploadFile.SaveAs(ProductImage);

                    string query = "Insert INTO Product Values(@CategoryId,@SubCategoryId,@Name,@Company,@Quantity,@Price,@ProductImage,@ExpiryDate,@ProductCreateDate,@ProductUpdateDate,@IsActive,@HSNCODE,@PurchasePrice)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    sqlcmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("@Name", product.Name);
                    sqlcmd.Parameters.AddWithValue("@Company", product.Company);
                    sqlcmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    sqlcmd.Parameters.AddWithValue("@Price", product.Price);
                    sqlcmd.Parameters.AddWithValue("@ProductImage", product.ProductImage);
                    sqlcmd.Parameters.AddWithValue("@ExpiryDate", product.ExpiryDate);
                    sqlcmd.Parameters.AddWithValue("@ProductCreateDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@ProductUpdateDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsACtive", 1);
                    sqlcmd.Parameters.AddWithValue("@HSNCODE", product.HSNCODE);
                    sqlcmd.Parameters.AddWithValue("@PurchasePrice", product.PurchasePrice);


                    sqlcmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("ProductView");

        }

        [HttpGet]
        public ActionResult ProductUpdate(int id)
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
                List<SelectListItem> gs = new List<SelectListItem>();
                string query = " SELECT HSNCODE FROM GST";
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {


                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            gs.Add(new SelectListItem
                            {
                                Text = sdr["HSNCODE"].ToString(),
                                Value = sdr["HSNCODE"].ToString()
                            });
                        }
                    }
                    ViewBag.gst = gs;
                }
            }

            Product product = new Product();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();
                string query1 = "Select * from Product where ProductId=@ProductId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query1, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                sqldata.Fill(dtbl);

            }
            product.ProductId = Convert.ToInt32(dtbl.Rows[0][0]);
            product.CategoryId = dtbl.Rows[0][1].ToString();
            product.SubCategoryId = dtbl.Rows[0][2].ToString();
            product.Name = dtbl.Rows[0][3].ToString();
            product.Company = dtbl.Rows[0][4].ToString();
            product.Quantity = Convert.ToInt32(dtbl.Rows[0][5]);
            product.Price = (float)Convert.ToDouble(dtbl.Rows[0][6]);
            product.ProductImage = dtbl.Rows[0][7].ToString();
            product.ExpiryDate = Convert.ToDateTime(dtbl.Rows[0][8]);
            product.HSNCODE = dtbl.Rows[0][12].ToString();


            return View(product);


            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult ProductUpdate(Product product)
        {

            if (ModelState.IsValid)
            {

                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();
                    string str = "";
                    if (product.UploadFile != null)
                    {
                        str = ", ProductImage=@ProductImage";

                        string ProductImage = Path.GetFileNameWithoutExtension(product.UploadFile.FileName);
                        string FileExtension = Path.GetExtension(product.UploadFile.FileName);
                        ProductImage = ProductImage + FileExtension;
                        product.ProductImage = "~/UserImages/" + ProductImage;
                        ProductImage = Path.Combine(Server.MapPath("~/UserImages/"), ProductImage);
                        product.UploadFile.SaveAs(ProductImage);


                    }
                    else
                    {
                        str = "";

                    }


                    string query = "Update Product set CategoryId=@CategoryId,SubCategory=@SubCategory, Name=@Name ,Company=@Company,Quantity=@Quantity,Price=@Price" + str + ",ExpiryDate=@ExpiryDate,ProductUpdateDate = @ProductUpdateDate,ProductHSNCODE=@ProductHSNCODE,PurchasePrice=@PurchasePrice where ProductId = @ProductId";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                    sqlcmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    sqlcmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    sqlcmd.Parameters.AddWithValue("@SubCategory", product.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("@Name", product.Name);
                    sqlcmd.Parameters.AddWithValue("@Company", product.Company);
                    sqlcmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    sqlcmd.Parameters.AddWithValue("@Price", product.Price);
                    if (product.UploadFile != null)
                    {
                        sqlcmd.Parameters.AddWithValue("@ProductImage", product.ProductImage);
                    }

                    sqlcmd.Parameters.AddWithValue("@ExpiryDate", product.ExpiryDate);
                    sqlcmd.Parameters.AddWithValue("@ProductUpdateDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@ProductHSNCODE", product.HSNCODE);
                    sqlcmd.Parameters.AddWithValue("@PurchasePrice", product.PurchasePrice);



                    sqlcmd.ExecuteNonQuery();

                }

            }
            return RedirectToAction("ProductView");
        }

        public ActionResult ProductDelete(int id)
        {
            using (SqlConnection connect = new SqlConnection(constring))
            {
                connect.Open();



                string category = "select count(*) from Product a right join Purchase d on a.ProductId = d.ProductId left join Sales s on a.ProductId = s.ProductId where a.ProductId  = @ProductId";
                SqlCommand catcmd = new SqlCommand(category, connect);
                catcmd.Parameters.AddWithValue("@ProductId", id);
                object subcategorycount = catcmd.ExecuteScalar();
                int countone = Convert.ToInt32(subcategorycount);
                if (countone > 0)
                {
                    Session["ProductId"] = id;
                    MessageBox.Show("This Product id already used on child table ,first Delete Record on that table ");
                    return RedirectToAction("ProductView");
                }
                else
                {

                    string query = "Delete From Product where ProductId = @ProductId";
                    SqlCommand sqlcmd = new SqlCommand(query, connect);
                    sqlcmd.Parameters.AddWithValue("@ProductId", id);


                    sqlcmd.ExecuteNonQuery();



                    return RedirectToAction("ProductView");

                }


            }
        }
    }
}