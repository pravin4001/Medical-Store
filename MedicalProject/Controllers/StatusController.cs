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
    public class StatusController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: PaymentMethod
        [HttpGet]
        public ActionResult StatusView()
        {
            if (Session["UserName"] != null)
            {
               
           
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from Status where IsOption = 1", sqlCon);
                sqlData.Fill(dtbl);
            }

            return View(dtbl);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }
        }
        public ActionResult StatusView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Status set IsOption=0 where StatusId=@StatusId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@StatusId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("StatusView");
        }

        public ActionResult StatusInsert()
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
        public ActionResult StatusInsert(Status status)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select StatusName from Status where StatusName=@StatusName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@StatusName", status.StatusName);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["StatusName"] = status.StatusName.ToString();
                ViewData["Message2"] = "StatusName Already Exists !";

                MessageBox.Show("StatusName Name Already Exist !!");
                return RedirectToAction("StatusInsert");

            }
            else
            {

                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string query = "Insert INTO Status Values(@StatusName,@StatusCreated,@StatusUpdated,@IsOption)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@StatusName", status.StatusName);
                    sqlcmd.Parameters.AddWithValue("@StatusCreated", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@StatusUpdated", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsOption", 1);


                    sqlcmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("StatusView");
        }
        [HttpGet]
        public ActionResult StatusUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            Status status = new Status();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from Status where StatusId=@StatusId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@StatusId", id);
                sqldata.Fill(dtbl);
            }

            status.StatusId = Convert.ToInt32(dtbl.Rows[0][0]);
            status.StatusName = (dtbl.Rows[0][1]).ToString();

            return View(status) ;
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult StatusUpdate(Status status)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update Status set StatusName = @StatusName,StatusUpdated=@StatusUpdated where StatusId = @StatusId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@StatusId", status.StatusId);
                sqlcmd.Parameters.AddWithValue("@StatusName", status.StatusName);
                sqlcmd.Parameters.AddWithValue("@StatusUpdated", DateTime.Now);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("StatusView");

        }

        public ActionResult StatusDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Status where StatusId = @StatusId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@StatusId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("StatusView");


        }
    }
}