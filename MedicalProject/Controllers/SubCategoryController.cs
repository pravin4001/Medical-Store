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
    public class SubCategoryController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: SubCategory
        public ActionResult SubCategoryView()
        {

            if (Session["UserName"] != null)
            {
               
          

            DataTable dtblCategory = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();   

                SqlDataAdapter sqlData = new SqlDataAdapter("select Id,SubCategoryName,CreateDateSub,UpdateDateSub,CategoryName from SubCategory,CategoryMaster where  CategoryMaster.MainCategoryId=SubCategory.CategoryId", sqlCon);
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
        public ActionResult SubCategoryView(List<int> checks)
        {

            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update SubCategory set IsActive=0 where CategoryId=@CategoryId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@CategoryId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("SubCategoryView");

        }
        [HttpGet]
        public ActionResult SubCategoryInsert()
        {
            if (Session["UserName"] != null)
            {
               
          
            using (SqlConnection sqlcon = new SqlConnection(constring))
            {
                sqlcon.Open();
                List<SelectListItem>SubCategory = new List<SelectListItem>();
                string query = " SELECT MainCategoryId,CategoryName FROM CategoryMaster";
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {


                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            SubCategory.Add(new SelectListItem
                            {
                                Text = sdr["CategoryName"].ToString(),
                                Value = sdr["MainCategoryId"].ToString()
                            });
                        }
                    }
                    ViewBag.subcatlog = SubCategory;
                }


            }
            return View(new SubCategory());
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
            [HttpPost]
        public ActionResult SubCategoryInsert(SubCategory subcategeory)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select SubCategoryName from SubCategory where SubCategoryName=@SubCategoryName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@SubCategoryName", subcategeory.Name);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["SubCategoryName"] = subcategeory.Name.ToString();
                ViewData["Message2"] = "SubCategoryName Already Exists !";

                MessageBox.Show("SubCategoryName Name Already Exist !!");
                return RedirectToAction("SubCategoryInsert");

            }
            else
            { 
                DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();



                string query = "Insert INTO SubCategory Values(@CategoryId,@Name,@CreateDateSub,@UpdateDateSub,@IsAvailable)";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@CategoryId", subcategeory.CategoryId);
                sqlcmd.Parameters.AddWithValue("@Name",subcategeory.Name);
                sqlcmd.Parameters.AddWithValue("@CreateDateSub", DateTime.Now);
               sqlcmd.Parameters.AddWithValue("@UpdateDateSub", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@IsAvailable", 1);


                sqlcmd.ExecuteNonQuery();

            }

            }
            return RedirectToAction("SubCategoryView");
        }
        [HttpGet]
        public ActionResult SubCategoryUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            using (SqlConnection sqlcon = new SqlConnection(constring))
            {
                sqlcon.Open();
                List<SelectListItem> SubCategory = new List<SelectListItem>();
                string query = " SELECT MainCategoryId, CategoryName FROM CategoryMaster";
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {


                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            SubCategory.Add(new SelectListItem
                            {
                                Text = sdr["CategoryName"].ToString(),
                                Value = sdr["MainCategoryId"].ToString()
                            });
                        }
                    }
                    ViewBag.subcatlog = SubCategory;
                }

            }


                SubCategory sub = new SubCategory();
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();


                    string query1 = "Select * from SubCategory where Id=@Id";
                    SqlDataAdapter sqldata = new SqlDataAdapter(query1, sqlCon);
                    sqldata.SelectCommand.Parameters.AddWithValue("@Id", id);
                    sqldata.Fill(dtbl);
                }
                sub.Id = Convert.ToInt32(dtbl.Rows[0][0]);
            sub.CategoryId = dtbl.Rows[0][1].ToString();
                sub.Name = dtbl.Rows[0][2].ToString();



                return View(sub);

            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }

        [HttpPost]
        public ActionResult SubCategoryUpdate(SubCategory sub)
        {
            if (ModelState.IsValid)
            {

                using (SqlConnection sqlCon = new SqlConnection(constring))
                {
                    sqlCon.Open();



                    string query = "Update SubCategory set CategoryId=@CategoryId, Name=@Name ,UpdateDateSub = @UpdateDateSub where Id = @Id";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                    sqlcmd.Parameters.AddWithValue("@ID",sub.Id);
                    sqlcmd.Parameters.AddWithValue("@CategoryId", sub.CategoryId);
                    sqlcmd.Parameters.AddWithValue("@Name", sub.Name);
                    sqlcmd.Parameters.AddWithValue("@UpdateDateSub",DateTime.Now);
                   


                    sqlcmd.ExecuteNonQuery();

                }

            }
            return RedirectToAction("SubCategoryView");
        }

        public ActionResult SubCategoryDelete(int id)
        {
            using (SqlConnection connect = new SqlConnection(constring))
            {
                connect.Open();



                string category = "select count(*) from SubCategory a  left join Product c on a.Id = c.SubCategory left join Purchase d on a.Id = d.SubCategoryId left join Sales s on a.Id = s.SubCategoryId where Id = @Id";
                SqlCommand catcmd = new SqlCommand(category, connect);
                catcmd.Parameters.AddWithValue("@Id", id);
                object subcategorycount = catcmd.ExecuteScalar();
                int countone = Convert.ToInt32(subcategorycount);
                if (countone > 1)
                {
                    Session["Id"] = id;
                    MessageBox.Show("This SubCategory id already used on child table ,first Delete Record on that table ");
                    return RedirectToAction("SubCategoryView");
                }
                else
                {

                    string query = "Delete From SubCategory where Id = @Id";
                    SqlCommand sqlcmd = new SqlCommand(query, connect);
                    sqlcmd.Parameters.AddWithValue("@Id", id);


                    sqlcmd.ExecuteNonQuery();



                    return RedirectToAction("SubCategoryView");

                }
            }

           

        }
    }
}