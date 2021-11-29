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
    public class GSTController : Controller
    {
       
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();

        // GET: GST
        public ActionResult GSTView()
        {
            if (Session["UserName"] != null)
            {
               
           
            DataTable dtblCategory = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from GST where IsActivate=1", sqlCon);
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
        public ActionResult GSTView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update GST set IsActivate=0 where HSNCODE=@HSNCODE";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@HSNCODE", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GSTView");
        }
        public ActionResult GSTInsert()
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
        public ActionResult GSTInsert(GST gst)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select HSNCODE from GST where HSNCODE=@HSNCODE ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@HSNCODE", gst.HSNCode);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["HSNCODE"] = gst.HSNCode.ToString();
                ViewData["Message2"] = "HSNCODE Already Exists !";

                MessageBox.Show("HSNCODE Already Exist !!");
                return RedirectToAction("GSTInsert");

            }
            else
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(constring))
                {

                    sqlCon.Open();



                    string query = "Insert INTO GST Values(@HSNCODE,@CGST,@SGST,@IGST,@CreationDate,@UpdationDate,@IsActivate,@IsExempted)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                    sqlcmd.Parameters.AddWithValue("@HSNCODE", gst.HSNCode);
                    sqlcmd.Parameters.AddWithValue("@CGST", gst.CGST);
                    sqlcmd.Parameters.AddWithValue("@SGST", gst.SGST);
                    sqlcmd.Parameters.AddWithValue("@IGST", gst.IGST);
                    sqlcmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@UpdationDate", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@IsActivate", 1);
                    sqlcmd.Parameters.AddWithValue("@IsExempted", gst.IsExempted);


                    sqlcmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("GSTView");
        }
        [HttpGet]
        public ActionResult GSTUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            GST gst = new GST();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from GST where HSNCODE=@HSNCODE";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@HSNCODE", id);
                sqldata.Fill(dtbl);
            }

            gst.HSNCode = Convert.ToInt32(dtbl.Rows[0][0]);
            gst.CGST = (float)Convert.ToDouble(dtbl.Rows[0][1]);
            gst.SGST = (float)Convert.ToDouble(dtbl.Rows[0][2]);
            gst.IGST = (float)Convert.ToDouble(dtbl.Rows[0][3]);
           



            return View(gst);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult GSTUpdate(GST gst)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update GST set CGST = @CGST,SGST=@SGST,IGST=@IGST,UpdationDate=@UpdationDate where HSNCODE = @HSNCODE";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@HSNCODE", gst.HSNCode);
                sqlcmd.Parameters.AddWithValue("@CGST", gst.CGST);
                sqlcmd.Parameters.AddWithValue("@SGST", gst.SGST);
                sqlcmd.Parameters.AddWithValue("@IGST", gst.IGST);
                sqlcmd.Parameters.AddWithValue("@UpdationDate", DateTime.Now);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("GSTView");

        }

        public ActionResult GSTDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From GST where HSNCODE = @HSNCODE";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@HSNCODE", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("GSTView");


        }



    }
}