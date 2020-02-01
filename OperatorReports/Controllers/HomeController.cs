using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity;
using DataAccessLogicComponent;
using DataAccessLogicComponent.Interfaces;
using OperatorReports.Models;

namespace OperatorReports.Controllers
{
    public class HomeController : Controller
    {
        #region Private Variables

        private IReportsRepository _repository;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController()
        {
            //TODO: DI here as well
            _repository = new ReportsRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public HomeController(IReportsRepository repository)
        {
            _repository = repository;
        }

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

            productivityReport.OperatorProductivity = _repository.GetReports(new ReportFilter()).Select(r => new OperatorReportViewModel
            {
                ID = r.Id,
                AverageChatLength = r.AverageChatLength,
                Name = r.Name,
                ProactiveAnswered = r.ProactiveAnswered,
                ProactiveResponseRate = r.ProactiveResponseRate,
                ProactiveSent = r.ProactiveSent,
                ReactiveAnswered = r.ReactiveAnswered,
                ReactiveReceived = r.ReactiveReceived,
                ReactiveResponseRate = r.ReactiveResponseRate,
                TotalChatLength = r.TotalChatLength
            }).ToList();

            return View(productivityReport);
        }

        public ActionResult OperatorProductivityData()
        {
            return View();
        }
    }
}