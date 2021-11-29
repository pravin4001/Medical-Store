using MedicalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace MedicalProject.Controllers
{
    public class CustomerController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
       
        [HttpGet]
        public ActionResult CustomerView()
        {
            if (Session["UserName"] != null)
            {
                DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from Customer where IsAvailable = 1", sqlCon);
                sqlData.Fill(dtbl);
            }

            return View(dtbl);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult CustomerView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Customer set IsAvailable=0 where CustomerId=@CustomerId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@CustomerId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("CustomerView");
        }

        public ActionResult CustomerInsert()
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
        public ActionResult CustomerInsert(Customer customer)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select CustomerName from Customer where CustomerName=@CustomerName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@CustomerName", customer.CustomerName);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["CustomerName"] = customer.CustomerName.ToString();
                ViewData["Message2"] = "CustomerName Already Exists !";

                MessageBox.Show("CustomerName Already Exist !!");
                return RedirectToAction("CustomerInsert");

            }
            else
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string query = "Insert INTO Customer Values(@CustomerName,@Address,@PinCode,@PhoneNumber,@Email,@CreateDate,@UpdateDate,@IsAvailable)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                    sqlcmd.Parameters.AddWithValue("@Address", customer.Address);
                    sqlcmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                    sqlcmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    sqlcmd.Parameters.AddWithValue("@Email", customer.Email);
                    sqlcmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@UpdateDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsAvailable", 1);


                    sqlcmd.ExecuteNonQuery();

                }
               



            }
            return RedirectToAction("CustomerView");
        }
        [HttpGet]
        public ActionResult CustomerUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            Customer customer = new Customer();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from Customer where CustomerId=@CustomerId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@CustomerId", id);
                sqldata.Fill(dtbl);
            }

            customer.CustomerId = Convert.ToInt32(dtbl.Rows[0][0]);
            customer.CustomerName = (dtbl.Rows[0][1]).ToString();
            customer.Address = (dtbl.Rows[0][2]).ToString();
            customer.PinCode = Convert.ToInt32(dtbl.Rows[0][3]);
            customer.PhoneNumber = (dtbl.Rows[0][4]).ToString();
            customer.Email = (dtbl.Rows[0][5]).ToString();
           



            return View(customer);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult CustomerUpdate(Customer customer)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update Customer set CustomerName = @CustomerName,Address=@Address,PinCode=@PinCode,PhoneNumber=@PhoneNumber,Email=@Email,UpdateDate=@UpdateDate where CustomerId = @CustomerId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                sqlcmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                sqlcmd.Parameters.AddWithValue("@Address", customer.Address);
                sqlcmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                sqlcmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                sqlcmd.Parameters.AddWithValue("@Email", customer.Email);
                sqlcmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("CustomerView");

        }

        public ActionResult CustomerDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Customer where CustomerId = @CustomerId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@CustomerId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("CustomerView");


        }

    }
}