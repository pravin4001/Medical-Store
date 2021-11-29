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
    public class PaymentMethodController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: PaymentMethod
        [HttpGet]
        public ActionResult PaymentMethodView()
        {
            if (Session["UserName"] != null)
            {
               
           
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from PaymentMethod where IsValid = 1", sqlCon);
                sqlData.Fill(dtbl);
            }

            return View(dtbl);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        public ActionResult PaymentMethodView(List<int> checks)
        {
           
                
           
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update PaymentMethod set IsValid=0 where PaymentMethodId=@PaymentMethodId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@PaymentMethodId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("PaymentMethodView");
          
        }

        public ActionResult PaymentMethodInsert()
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
        public ActionResult PaymentMethodInsert(PaymentMethod paymentmethod)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select MethodName from PaymentMethod where MethodName=@MethodName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@MethodName", paymentmethod.MethodName);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["MethodName"] = paymentmethod.MethodName.ToString();
                ViewData["Message2"] = "MethodName Already Exists !";

                MessageBox.Show("Method Name Already Exist !!");
                return RedirectToAction("PaymentMethodInsert");

            }
            else
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string query = "Insert INTO PaymentMethod Values(@MethodName,@MethodCreatedDate,@MethodUpdatedDate,@IsValid)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@MethodName", paymentmethod.MethodName);
                    sqlcmd.Parameters.AddWithValue("@MethodCreatedDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@MethodUpdatedDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsValid", 1);


                    sqlcmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("PaymentMethodView");
        }
        [HttpGet]
        public ActionResult PaymentMethodUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
                PaymentMethod paymentmethod = new PaymentMethod();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from PaymentMethod where PaymentMethodId=@PaymentMethodId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@PaymentMethodId", id);
                sqldata.Fill(dtbl);
            }

            paymentmethod.PaymentMethodId = Convert.ToInt32(dtbl.Rows[0][0]);
            paymentmethod.MethodName = (dtbl.Rows[0][1]).ToString();
          
            return View(paymentmethod);

            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        [HttpPost]
        public ActionResult PaymentMethodUpdate(PaymentMethod paymentmethod)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update PaymentMethod set MethodName = @MethodName,MethodUpdatedDate=@MethodUpdatedDate where PaymentMethodId = @PaymentMethodId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", paymentmethod.PaymentMethodId);
                sqlcmd.Parameters.AddWithValue("@MethodName",paymentmethod.MethodName);
                sqlcmd.Parameters.AddWithValue("@MethodUpdatedDate", DateTime.Now);
               

                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("PaymentMethodView");

        }

        public ActionResult PaymentMethodDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From PaymentMethod where PaymentMethodId = @PaymentMethodId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@PaymentMethodId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("PaymentMethodView");


        }
    }
}