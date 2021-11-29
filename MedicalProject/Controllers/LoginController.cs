using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalProject.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MedicalProject.Controllers
{
    public class LoginController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();

        public ActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(Login log)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string query = "Select UserName,Password from Login where UserName=@UserName and Password=@Password";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(query, sqlcon);
            sqlcommand.Parameters.AddWithValue("@UserName", log.UserName);
            sqlcommand.Parameters.AddWithValue("@Password", log.Password);
            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if(sdr.Read())
            {
                Session["UserName"] = log.UserName.ToString();
                
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewData["Message"] = "Incorrect UserName OR Password !";
            }
            sqlcon.Close();
            return View();
           
        }
        [HttpPost]
        public ActionResult RegistrationForm(Registration registration)
        {

            SqlConnection sqlcon = new SqlConnection(constring);
            string query = "Select UserName from Login where UserName=@UserName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(query, sqlcon);
            sqlcommand.Parameters.AddWithValue("@UserName", registration.UserName);
           
            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["UserName"] = registration.UserName.ToString();
                ViewData["Message1"] = "User Name Already Exists !";

            }
            else
            {
               
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string InsertReg = "Insert INTO Login Values(@Name,@UserName,@Password,@CreateDate,@UpdateDate,@IsActive)";
                    SqlCommand sqlcmd = new SqlCommand(InsertReg, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@Name", registration.Name);
                    sqlcmd.Parameters.AddWithValue("@UserName", registration.UserName);
                    sqlcmd.Parameters.AddWithValue("@Password", registration.Password);
                    sqlcmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@UpdateDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsActive",1);


                    sqlcmd.ExecuteNonQuery();

                }
                return RedirectToAction("LoginForm", "Login");
            }
            sqlcon.Close();

            return View();

        }

        public ActionResult RegistrationForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoginUpdate(int id)
        {
            Registration reg = new Registration();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from Login where UserId=@UserId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@UserId", id);
                sqldata.Fill(dtbl);
            }

            reg.UserId = Convert.ToInt32(dtbl.Rows[0][0]);
            reg.Name = (dtbl.Rows[0][1]).ToString();
            reg.UserName =(dtbl.Rows[0][2]).ToString();
            reg.Password = dtbl.Rows[0][3].ToString();
         




            return View(reg);


        }
        [HttpPost]
        public ActionResult LoginUpdate(Registration reg)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update Login set Name = @Name, UserName = @UserName,Password=@Password,UpdateDate=@UpdateDate where UserId = @UserId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@UserId", reg.UserId);
                sqlcmd.Parameters.AddWithValue("@Name", reg.Name);
                sqlcmd.Parameters.AddWithValue("@UserName", reg.UserName);
                sqlcmd.Parameters.AddWithValue("@Password", reg.Password);
                sqlcmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("UserDetails");

        }

        public ActionResult UserDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Login where UserId = @UserId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@UserId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("UserDetails");


        }

        public ActionResult UserDetails()
        {
            DataTable dtblCategory = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from Login", sqlCon);
                sqlData.Fill(dtblCategory);
            }

            return View(dtblCategory);
        }
        [HttpPost]
        public ActionResult UserDetails(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Login set IsActive=0 where UserId=@UserId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@UserId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("UserDetails");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

}