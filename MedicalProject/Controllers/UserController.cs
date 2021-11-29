using MedicalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace MedicalProject.Controllers
{
    public class UserController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: User
        public ActionResult DashBoard()
        {

           
                using (SqlConnection connect = new SqlConnection(constring))
                {
                    connect.Open();
                    string totalproduct = "select count(*) from Product";
                    SqlCommand cmd = new SqlCommand(totalproduct, connect);
                    object data = cmd.ExecuteScalar();
                    ViewBag.product = Convert.ToInt32(data);


                    string totalsales = "select count(*) from Sales where IsSalesActive = 1";
                    SqlCommand cmd2 = new SqlCommand(totalsales, connect);
                    object data2 = cmd2.ExecuteScalar();
                    ViewBag.sales = Convert.ToInt32(data2);


                    string totalpurchase = "select count(*) from Purchase where PurchaseActive = 1";
                    SqlCommand cmdpurchase = new SqlCommand(totalpurchase, connect);
                    object datapurchase = cmdpurchase.ExecuteScalar();
                    ViewBag.purchase = Convert.ToInt32(datapurchase);

                }
                using (SqlConnection connect = new SqlConnection(constring))
                {
                    connect.Open();
                    string one = "select SUM(Price) from Product";
                    string two = "select SUM(TotalAmount) from Purchase where PurchaseActive = 1 ";
                    //string three = "SELECT CategoryName , COUNT(CategoryId) FROM CategoryMaster left JOIN Product ON CategoryMaster.MainCategoryId = product.CategoryIdGROUP BY CategoryName";

                    SqlCommand data1 = new SqlCommand(one, connect);
                    object price = data1.ExecuteScalar();
                    ViewBag.prices = Convert.ToInt32(price);

                    SqlCommand totalamount = new SqlCommand(two, connect);
                    object totlamounts = totalamount.ExecuteScalar();
                    ViewBag.totalpurchaseamount = Convert.ToInt32(totlamounts);


               


            }

                //using (SqlConnection connect = new SqlConnection(constring))
                //{
                //    connect.Open();
                //    string totalproduct = "select count(*) from Product";
                //    SqlCommand cmd = new SqlCommand(totalproduct, connect);
                //    object data = cmd.ExecuteScalar();
                //    ViewBag.product = Convert.ToInt32(data);

                //    string totalsales = "select count(*) from Sales where IsSalesActive = 1";
                //    SqlCommand cmd2 = new SqlCommand(totalsales, connect);
                //    object data2 = cmd2.ExecuteScalar();
                //    ViewBag.sales = Convert.ToInt32(data2);
                //}
                //    using (SqlConnection connect = new SqlConnection(constring))
                //    {
                //        connect.Open();
                //        string one = "select SUM(Price) from Product";
                //        string two = "select SUM(TotalAmount) from Purchase where PurchaseActive = 1 ";

                //        SqlCommand data1 = new SqlCommand(one, connect);
                //        object price = data1.ExecuteScalar();
                //        ViewBag.prices = Convert.ToInt32(price);
                //        SqlCommand totalamount = new SqlCommand(two, connect);
                //        object totlamounts = totalamount.ExecuteScalar();
                //        ViewBag.sellingamount = Convert.ToInt32(totlamounts);




                //    }



                return View();
                
           

          
        }


        public ActionResult CategoryView()
        {
            if (Session["UserName"] != null)
            {
              
            DataTable dtblCategory = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from CategoryMaster where ISAvailable=1 ", sqlCon);
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
        public ActionResult CategoryView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Category set ISAvailable=0 where MainCategoryId=@MainCategoryId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@MainCategoryId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("CategoryView");

        }

        public ActionResult CategoryInsert()
        {

            if (Session["UserName"]!= null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult CategoryInsert(Category categeory)
        {

            SqlConnection sqlcon = new SqlConnection(constring);
            string Categoryquery = "Select CategoryName from CategoryMaster where CategoryName=@CategoryName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Categoryquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@CategoryName",categeory.CategoryName);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["CategoryName"] = categeory.CategoryName.ToString();
                ViewData["Message2"] = "CategoryName Already Exists !";

                MessageBox.Show("Category Name Already Exist !!");
                return RedirectToAction("CategoryInsert");

            }
            else
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string query = "Insert INTO CategoryMaster Values(@CategoryName,@Description,@IsAvailable,@CreateDate,@UpdateDate,@Active)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@CategoryName", categeory.CategoryName);
                    sqlcmd.Parameters.AddWithValue("@Description", categeory.Description);
                    sqlcmd.Parameters.AddWithValue("@IsAvailable", 1);
                    sqlcmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@UpdateDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@Active",categeory.Active);


                    sqlcmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("CategoryView");
        }
        [HttpGet]
        public ActionResult CategoryUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            Category categeoryupdate = new Category();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from CategoryMaster where MainCategoryId=@MainCategoryId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@MainCategoryId", id);
                sqldata.Fill(dtbl);
            }

            categeoryupdate.CategoryId = Convert.ToInt32(dtbl.Rows[0][0]);
            categeoryupdate.CategoryName = dtbl.Rows[0][1].ToString();
            categeoryupdate.Description = dtbl.Rows[0][2].ToString();
            categeoryupdate.Active = Convert.ToBoolean(dtbl.Rows[0][6]);


            return View(categeoryupdate);

            }
            else
            {
                return RedirectToAction("LoginForm");
            }
        }
        [HttpPost]
        public ActionResult CategoryUpdate(Category categeoryupdate)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update CategoryMaster set CategoryName = @CategoryName,Descriptions=@Descriptions,UpdateDate=@UpdateDate,Active=@Active  where MainCategoryId = @MainCategoryId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@MainCategoryId", categeoryupdate.CategoryId);
                sqlcmd.Parameters.AddWithValue("@CategoryName", categeoryupdate.CategoryName);
                sqlcmd.Parameters.AddWithValue("@Descriptions", categeoryupdate.Description);
                sqlcmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@Active",categeoryupdate.Active);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("CategoryView");

        }

        public ActionResult CategoryDelete(int id)
        {
            using (SqlConnection connect = new SqlConnection(constring))
            {
                connect.Open();



                string category = "select count(*) from CategoryMaster a left join SubCategory b on a.MainCategoryId = b.CategoryId left join Product c on a.MainCategoryId = c.CategoryId left join Purchase d on a.MainCategoryId = d.CategoryId left join Sales s on a.MainCategoryId = s.CategoryId where MainCategoryId = @MainCategoryId";
                SqlCommand catcmd = new SqlCommand(category, connect);
                catcmd.Parameters.AddWithValue("@MainCategoryId", id);
                object subcategorycount = catcmd.ExecuteScalar();
                int countone = Convert.ToInt32(subcategorycount);
                if (countone >1)
                {
                    Session["MainCategoryId"] = id;
                    MessageBox.Show("This Category id already used on SubCategory And Product table ,first Delete Record on that table ");
                    return RedirectToAction("CategoryView");
                }
                else
                {
                   
                        string query = "Delete From CategoryMaster where MainCategoryId = @MainCategoryId";
                        SqlCommand sqlcmd = new SqlCommand(query, connect);
                        sqlcmd.Parameters.AddWithValue("@MainCategoryId", id);


                        sqlcmd.ExecuteNonQuery();


                  
                    return RedirectToAction("CategoryView");

                }
            }

           

        }

       
    }
}