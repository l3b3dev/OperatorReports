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
using Services;
using Services.Interfaces;

namespace OperatorReports.Controllers
{
    public class HomeController : Controller
    {
        #region Private Variables

        private IReportsRepository _repository;
        private readonly IDurationParser _durationParser;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController()
        {
            //TODO: DI here as well
            _repository = new ReportsRepository();
            _durationParser = new DurationParser();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="durationParser"></param>
        public HomeController(IReportsRepository repository, IDurationParser durationParser)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _durationParser = durationParser ?? throw new ArgumentNullException(nameof(durationParser));
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
                AverageChatLength = !string.IsNullOrEmpty(r.AverageChatLength)? $"{r.AverageChatLength}m":"-",
                Name = r.Name,
                ProactiveAnswered = r.ProactiveAnswered,
                ProactiveResponseRate = r.ProactiveResponseRate,
                ProactiveSent = r.ProactiveSent,
                ReactiveAnswered = r.ReactiveAnswered,
                ReactiveReceived = r.ReactiveReceived,
                ReactiveResponseRate = r.ReactiveResponseRate,
                TotalChatLength = _durationParser.Parse(r.TotalChatLength)
            }).ToList();

            return View(productivityReport);
        }

        public ActionResult OperatorProductivityData()
        {
            return View();
        }
    }
}