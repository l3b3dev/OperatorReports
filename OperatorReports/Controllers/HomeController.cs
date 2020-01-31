using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperatorReports.Models;

namespace OperatorReports.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Reports logic
        /// </summary>
        /// <returns></returns>
        public ActionResult OperatorReport()
        {
            var productivityReport = new OperatorReportItems
            {
                OperatorProductivity = new List<OperatorReportViewModel>()
            };

            ViewBag.Message = "Operator Productivity Report";
            //TODO: roll in ORM like EF or Dapper for Db access
            using (var conn =
                    new SqlConnection(
                        "Data Source=PHOENIX\\SQLEXPRESS;Initial Catalog=chat;User id=chat;Password=chat;") //TODO: move sensitive info like connection strings out
            )
            {
                conn.Open();

                using (var sqlcomm =
                    new SqlCommand("exec dbo.OperatorProductivity ", conn))
                {
                    using (var dr = sqlcomm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var opVM = new OperatorReportViewModel
                            {
                                ID = Convert.ToInt32(dr[0]),
                                Name = Convert.ToString(dr[1]),
                                ProactiveSent = Convert.ToInt32(dr[2]),
                                ProactiveAnswered = Convert.ToInt32(dr[3]),
                                ProactiveResponseRate = Convert.ToInt32(dr[4]),
                                ReactiveReceived = Convert.ToInt32(dr[5]),
                                ReactiveAnswered = Convert.ToInt32(dr[6]),
                                ReactiveResponseRate = Convert.ToInt32(dr[7]),
                                TotalChatLength = Convert.ToString(dr[8]),
                                AverageChatLength = Convert.ToString(dr[9])
                            };
                           
                            productivityReport.OperatorProductivity.Add(opVM);
                        }
                    }
                }
            }

            return View(productivityReport);
        }

        public ActionResult OperatorProductivityData()
        {
            return View();
        }
    }
}