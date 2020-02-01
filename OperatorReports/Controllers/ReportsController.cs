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

namespace OperatorReports.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        #region Private Variables

        private IReportsRepository _repository;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public ReportsController(IReportsRepository repository)
        {
            _repository = repository;
        }

        // GET api/values
        [Route("")]
        public IEnumerable<OperatorReportViewModel> GetReports(string sw=null, string sd=null, string from=null, string to=null, string sdate=null)
        {
            //parse params
            DateTime? dtFrom=null, dtTo=null;
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                if (DateTime.TryParse(from, out var dfrom))
                {
                    dtFrom = dfrom;
                }
                if (DateTime.TryParse(to, out var dto))
                {
                    dtTo = dto;
                }
            }

            //TODO: roll in ORM like EF or Dapper for Db access
            var result = _repository.GetReports(sw,sd,dtFrom,dtTo).Select(r => new OperatorReportViewModel
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
