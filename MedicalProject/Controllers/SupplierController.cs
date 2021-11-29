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
    public class SupplierController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        // GET: Supplier
        [HttpGet]
        public ActionResult SupplierView()
        {
            if (Session["UserName"] != null)
            {
                
           
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("select * from Supplier where IsActive = 1", sqlCon);
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
        public ActionResult SupplierView(List<int> checks)
        {
            using (SqlConnection cons = new SqlConnection(constring))
            {
                cons.Open();
                foreach (int val in checks)
                {
                    string query1 = "Update Supplier set IsActive=0 where SupplierId=@SupplierId";

                    SqlCommand cmd = new SqlCommand(query1, cons);
                    cmd.Parameters.AddWithValue("@SupplierId", val);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("SupplierView");
        }

        public ActionResult SupplierInsert()
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
        public ActionResult SupplierInsert(Supplier supplier)
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            string Supplierquery = "Select SupplierName from Supplier where SupplierName=@SupplierName ";
            sqlcon.Open();
            SqlCommand sqlcommand = new SqlCommand(Supplierquery, sqlcon);
            sqlcommand.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);

            SqlDataReader sdr = sqlcommand.ExecuteReader();
            if (sdr.Read())
            {
                Session["SupplierName"] = supplier.SupplierName.ToString();
                ViewData["Message2"] = "SupplierName Already Exists !";

                MessageBox.Show("Supplier Name Already Exist !!");
                return RedirectToAction("SupplierInsert");

            }
            else
            {

           
                DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();



                string query = "Insert INTO Supplier Values(@SupplierName,@Address,@PhoneNumber,@Email,@BankName,@IfscCode,@BankAccountNo,@CreateDate,@UpdateDate,@IsActive)";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);

                sqlcmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                sqlcmd.Parameters.AddWithValue("@Address", supplier.SupplierAddress);
                sqlcmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                sqlcmd.Parameters.AddWithValue("@Email", supplier.Email);
                sqlcmd.Parameters.AddWithValue("@BankName",supplier.BankName);
                sqlcmd.Parameters.AddWithValue("@IfscCode", supplier.IFSCCode);
                sqlcmd.Parameters.AddWithValue("@BankAccountNo", supplier.BankAccountNo);
                sqlcmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@UpdateDate", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@IsActive", 1);


                sqlcmd.ExecuteNonQuery();

            }
            }
            return RedirectToAction("SupplierView");
        }
        [HttpGet]
        public ActionResult SupplierUpdate(int id)
        {
            if (Session["UserName"] != null)
            {
               
           
            Supplier supplier = new Supplier();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {
                sqlCon.Open();


                string query = "Select * from Supplier where SupplierId=@SupplierId";
                SqlDataAdapter sqldata = new SqlDataAdapter(query, sqlCon);
                sqldata.SelectCommand.Parameters.AddWithValue("@SupplierId", id);
                sqldata.Fill(dtbl);
            }

            supplier.SupplierId = Convert.ToInt32(dtbl.Rows[0][0]);
            supplier.SupplierName = (dtbl.Rows[0][1]).ToString();
            supplier.SupplierAddress = (dtbl.Rows[0][2]).ToString();
            supplier.PhoneNumber = (dtbl.Rows[0][3]).ToString();
            supplier.Email = (dtbl.Rows[0][4]).ToString();
            supplier.BankName = (dtbl.Rows[0][5]).ToString();
            supplier.IFSCCode = (dtbl.Rows[0][6]).ToString();
            supplier.BankAccountNo = (dtbl.Rows[0][7]).ToString();
          


            return View(supplier);
            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }

        }
        [HttpPost]
        public ActionResult SupplierUpdate(Supplier supplier)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();

                string query = "Update Supplier set SupplierName = @SupplierName,Address=@Address,PhoneNumber=@PhoneNumber,Email=@Email,BankName=@BankName,IfscCode=@IfscCode,BankAccountNo=@BankAccountNo,UpdateDate=@UpdateDate where SupplierId = @SupplierId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
                sqlcmd.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                sqlcmd.Parameters.AddWithValue("@Address", supplier.SupplierAddress);
                sqlcmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                sqlcmd.Parameters.AddWithValue("@Email", supplier.Email);
                sqlcmd.Parameters.AddWithValue("@BankName", supplier.BankName);
                sqlcmd.Parameters.AddWithValue("@IfscCode",supplier.IFSCCode);
                sqlcmd.Parameters.AddWithValue("@BankAccountNo", supplier.BankAccountNo);
                
                sqlcmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);


                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("SupplierView");

        }

        public ActionResult SupplierDelete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(constring))
            {

                sqlCon.Open();
                string query = "Delete From Supplier where SupplierId = @SupplierId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@SupplierId", id);


                sqlcmd.ExecuteNonQuery();


            }

            return RedirectToAction("SupplierView");


        }

    }
}