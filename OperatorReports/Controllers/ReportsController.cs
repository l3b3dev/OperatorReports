using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLogicComponent;
using DataAccessLogicComponent.Interfaces;
using OperatorReports.Models;
using Services.Interfaces;

namespace OperatorReports.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        #region Private Variables

        private readonly IReportsRepository _repository;
        private readonly IFilterParamsParser _paramsParser;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="paramsParser"></param>
        public ReportsController(IReportsRepository repository, IFilterParamsParser paramsParser)
        {
            _repository = repository;
            _paramsParser = paramsParser;
        }

        // GET api/values
        [Route("")]
        public IEnumerable<OperatorReportViewModel> GetReports(string sw=null, string sd=null, string from=null, string to=null, string sdate=null)
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            var result = _repository.GetReports(_paramsParser.Parse(sw,sd,from,to,sdate)).Select(r => new OperatorReportViewModel
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

            return result;
        }

        /// <summary>
        /// Gets all websites.
        /// </summary>
        /// <returns></returns>
        [Route("websites")]
        public IEnumerable<string> GetAllWebsites()
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            return _repository.GetAllWebsites().ToList();
        }

        /// <summary>
        /// Gets all devices.
        /// </summary>
        /// <returns></returns>
        [Route("devices")]
        public IEnumerable<string> GetAllDevices()
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            return _repository.GetAllDevices().ToList();
        }
    }
}
