using MedicalProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace MedicalProject.Controllers
{
    public class DashboardController : Controller
    {
        List<dashboard> values = new List<dashboard>();
        // GET: Dashboard
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    DashboardModel dashboardModel = new DashboardModel();
        //    int Month = Convert.ToInt32( DateTime.Today.Month);
        //    int Year = Convert.ToInt32(DateTime.Today.Year);
        //    //string startdate = Mindate.ToString("yyyy-MM-dd");
        //    //ViewBag.date = startdate;

        

    

        //    string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
        //    // GET: User

        //    if (Session["UserName"] != null)
        //    {
        //        DashboardModel dashboard = new DashboardModel();
        //        using (SqlConnection connect = new SqlConnection(constring))
        //        {
        //            connect.Open();
        //            string totalproduct = "select count(*) from Product";
        //            SqlCommand cmd = new SqlCommand(totalproduct, connect);
        //            object data = cmd.ExecuteScalar();
        //            ViewBag.product = Convert.ToInt32(data);



        //            string totalsales = "select count(*) from Sales where IsSalesActive = 1";
        //            SqlCommand cmd2 = new SqlCommand(totalsales, connect);
        //            object data2 = cmd2.ExecuteScalar();
        //            ViewBag.sales = Convert.ToInt32(data2);


        //            string totalpurchase = "select count(*) from Purchase where PurchaseActive = 1";
        //            SqlCommand cmdpurchase = new SqlCommand(totalpurchase, connect);
        //            object datapurchase = cmdpurchase.ExecuteScalar();
        //            ViewBag.purchase = Convert.ToInt32(datapurchase);

        //            int product = Convert.ToInt32(data);
        //            int purchase = Convert.ToInt32(datapurchase);

        //            int total = product - purchase;

        //            ViewBag.Available = total;

        //        }
        //        using (SqlConnection connect = new SqlConnection(constring))
        //        {
        //            connect.Open();
        //            string three = "SELECT CategoryName ,COUNT(CategoryId) as countid FROM CategoryMaster left JOIN Product ON CategoryMaster.MainCategoryId = product.CategoryId GROUP BY CategoryName";


        //            SqlCommand dataone = new SqlCommand(three, connect);
        //            SqlDataReader rdr = dataone.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                values.Add(new dashboard(rdr["CategoryName"].ToString(), Convert.ToInt32(rdr["countid"])));
        //            }
        //            ViewBag.readdata = JsonConvert.SerializeObject(values);
        //        }
        //        using (SqlConnection connect = new SqlConnection(constring))
        //        {
        //            connect.Open();
        //            string one = "select SUM(Price) from Product";
        //            string two = "select SUM(TotalAmount) from Purchase where PurchaseActive = 1 ";




        //            SqlCommand data1 = new SqlCommand(one, connect);
        //            object price = data1.ExecuteScalar();
        //            ViewBag.prices = Convert.ToInt32(price);

        //            SqlCommand totalamount = new SqlCommand(two, connect);
        //            object totlamounts = totalamount.ExecuteScalar();
        //            ViewBag.totalpurchaseamount = Convert.ToInt32(totlamounts);



        //            string query = "select SUM(PurchasePrice) from Product where Isactive=1; ";
        //            SqlCommand sqlcmd = new SqlCommand(query, connect);
        //            object obj = sqlcmd.ExecuteScalar();
        //            ViewBag.Sumbuying = Convert.ToInt32(obj);

        //            string querysale = "select SUM(Price) from Product where Isactive=1; ";
        //            SqlCommand sqlcm = new SqlCommand(querysale, connect);
        //            object data = sqlcm.ExecuteScalar();
        //            ViewBag.SumSelling = Convert.ToInt32(data);

        //            int buy = Convert.ToInt32(obj);
        //            int sell = Convert.ToInt32(data);

        //            int total = sell - buy;

        //            ViewBag.profit = total;


        //            //int category22 = Convert.ToInt32(category1);
        //            //int category3 = Convert.ToInt32(perso);
        //            //int category4 = Convert.ToInt32(skin2);
        //            //int category5 = Convert.ToInt32(h3);
        //            //int category6 = Convert.ToInt32(ob1);

        //            //int total =category22+category3+category4+category5+category6;


        //            //SqlCommand totalsalesamount = new SqlCommand(three, connect);
        //            //object amount = totalsalesamount.ExecuteScalar();
        //            //ViewBag.totalsalesamount = Convert.ToInt32(amount);




        //        }



        //        return View();

        //    }
        //    else
        //    {
        //        return RedirectToAction("LoginForm", "Login");
        //    }



        //}
      
        public ActionResult Index([Optional] DashboardModel dashboard)
        {

            string constring = ConfigurationManager.ConnectionStrings["myConnection"].ToString();
            // GET: User

            if (Session["UserName"] != null)
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

                    int product = Convert.ToInt32(data);
                    int purchase = Convert.ToInt32(datapurchase);

                    int total = product - purchase;

                    ViewBag.Available = total;

                }
                using (SqlConnection connect = new SqlConnection(constring))
                {
                    connect.Open();
                    string three = "SELECT CategoryName ,COUNT(CategoryId) as countid FROM CategoryMaster left JOIN Product ON CategoryMaster.MainCategoryId = product.CategoryId GROUP BY CategoryName";


                    SqlCommand dataone = new SqlCommand(three, connect);
                    SqlDataReader rdr = dataone.ExecuteReader();
                    while (rdr.Read())
                    {
                        values.Add(new dashboard(rdr["CategoryName"].ToString(), Convert.ToInt32(rdr["countid"])));
                    }
                    ViewBag.readdata = JsonConvert.SerializeObject(values);
                }
                using (SqlConnection connect = new SqlConnection(constring))
                {
                    connect.Open();
                    string one = "select SUM(Price) from Product";
                    string two = "select SUM(TotalAmount) from Purchase where PurchaseActive = 1 ";




                    SqlCommand data1 = new SqlCommand(one, connect);
                    object price = data1.ExecuteScalar();
                    ViewBag.prices = Convert.ToInt32(price);

                    SqlCommand totalamount = new SqlCommand(two, connect);
                    object totlamounts = totalamount.ExecuteScalar();
                    ViewBag.totalpurchaseamount = Convert.ToInt32(totlamounts);




                    //string query = "select SUM(PurchasePrice) from Product where Isactive=1; ";
                    //SqlCommand sqlcmd = new SqlCommand(query, connect);
                    //object obj = sqlcmd.ExecuteScalar();
                    //ViewBag.Sumbuying = Convert.ToInt32(obj);

                    //string querysale = "select SUM(Price) from Product where Isactive=1; ";
                    //SqlCommand sqlcm = new SqlCommand(querysale, connect);
                    //object data = sqlcm.ExecuteScalar();
                    //ViewBag.SumSelling = Convert.ToInt32(data);
                   
                  

                    if (dashboard.startdate.ToString() == "01/01/0001 12:00:00 AM")
                    {
                        dashboard.enddate = DateTime.Now;
                        int dayy = DateTime.Now.Day;
                        int total = dayy - 1;
                        dashboard.startdate = DateTime.Now.AddDays(-total);

                        ViewBag.Start = dashboard.startdate.ToString("yyyy-MM-dd"); 
                        ViewBag.end = dashboard.enddate.ToString("yyyy-MM-dd");

                        //string startdate1 =("2021/10/1");
                        //string enddate1 =("2021/10/30"); 

                        try
                        {
                            string list = "select SUM(PurchasePrice) from Product where  ProductCreateDate between @startdate and @enddate";
                        SqlCommand dataone = new SqlCommand(list, connect);

                        dataone.Parameters.AddWithValue("@startdate", dashboard.startdate);
                        dataone.Parameters.AddWithValue("@enddate", dashboard.enddate);
                        Object da = dataone.ExecuteScalar();
                        ViewBag.Sumbuying = Convert.ToInt32(da);
                          
                        }
                        catch (System.Exception)
                        {
                            ViewBag.Sumbuying = 0;
                        }
                       
                        try
                        {
                            string list1 = "select SUM(Price) from Product where Isactive=1 and ProductCreateDate between @startdate and @enddate";
                            SqlCommand dataone1 = new SqlCommand(list1, connect);

                            dataone1.Parameters.AddWithValue("@startdate", dashboard.startdate);
                            dataone1.Parameters.AddWithValue("@enddate", dashboard.enddate);

                            Object da1 = dataone1.ExecuteScalar();

                            ViewBag.SumSelling = Convert.ToInt32(da1);
                           
                        }
                        catch (System.Exception)
                        {
                            ViewBag.SumSelling = 0;
                        }

                        int buy = Convert.ToInt32(ViewBag.Sumbuying);

                        int sell = Convert.ToInt32(ViewBag.SumSelling);
                          

                        int total1 = sell - buy;

                        ViewBag.profit = total1;
                    }
                    else
                    {
                        try
                        {


                            string list = "select SUM(PurchasePrice) from Product where Isactive=1 and ProductCreateDate between @startdate and @enddate";
                            SqlCommand dataone = new SqlCommand(list, connect);

                            dataone.Parameters.AddWithValue("@startdate", dashboard.startdate);
                            dataone.Parameters.AddWithValue("@enddate", dashboard.enddate);
                            Object da = dataone.ExecuteScalar();
                            ViewBag.Sumbuying = Convert.ToInt32(da);

                        }
                        catch (System.Exception)
                        {
                            ViewBag.Sumbuying = 0;
                        }
                        try
                        {
                            string list1 = "select SUM(Price) from Product where Isactive=1 and ProductCreateDate between @startdate and @enddate";
                            SqlCommand dataone1 = new SqlCommand(list1, connect);

                            dataone1.Parameters.AddWithValue("@startdate", dashboard.startdate);
                            dataone1.Parameters.AddWithValue("@enddate", dashboard.enddate);
                            Object da1 = dataone1.ExecuteScalar();
                            ViewBag.SumSelling = Convert.ToInt32(da1);
                        }
                        catch (System.Exception)
                        {
                            ViewBag.SumSelling = 0;
                        }
                        int buy = Convert.ToInt32(ViewBag.Sumbuying);
                        int sell = Convert.ToInt32(ViewBag.SumSelling);

                        int total = sell - buy;

                        ViewBag.profit = total;
                    }
                }
                   

                    //int category22 = Convert.ToInt32(category1);
                    //int category3 = Convert.ToInt32(perso);
                    //int category4 = Convert.ToInt32(skin2);
                    //int category5 = Convert.ToInt32(h3);
                    //int category6 = Convert.ToInt32(ob1);

                    //int total =category22+category3+category4+category5+category6;


                    //SqlCommand totalsalesamount = new SqlCommand(three, connect);
                    //object amount = totalsalesamount.ExecuteScalar();
                    //ViewBag.totalsalesamount = Convert.ToInt32(amount);




                




                return View();

            }
            else
            {
                return RedirectToAction("LoginForm", "Login");
            }






        }
    }
}