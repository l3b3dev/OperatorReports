using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using DataAccessLogicComponent;
using DataAccessLogicComponent.Interfaces;
using OperatorReports.Models;
using Services.Excel.Interfaces;
using Services.Interfaces;

namespace OperatorReports.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        #region Private Variables

        private readonly IReportsRepository _repository;
        private readonly IFilterParamsParser _paramsParser;
        private readonly IDurationParser _durationParser;
        private readonly IReportCreator _creator;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="paramsParser"></param>
        /// <param name="durationParser"></param>
        /// <param name="creator"></param>
        public ReportsController(IReportsRepository repository, IFilterParamsParser paramsParser, IDurationParser durationParser, IReportCreator creator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _paramsParser = paramsParser ?? throw new ArgumentNullException(nameof(paramsParser));
            _durationParser = durationParser ?? throw new ArgumentNullException(nameof(durationParser));
            _creator = creator ?? throw new ArgumentNullException(nameof(creator));
        }

        // GET api/values
        [Route("")]
        public IEnumerable<OperatorReportViewModel> GetReports(string sw=null, string sd=null, string from=null, string to=null, string sdate=null)
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            var result = _repository.GetReports(_paramsParser.Parse(sw,sd,from,to,sdate)).Select(r => new OperatorReportViewModel
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

            return result;
        }

        [Route("excel")]
        public HttpResponseMessage GetExcelReport(string sw = null, string sd = null, string from = null,
            string to = null, string sdate = null)
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            var data = _repository.GetReports(_paramsParser.Parse(sw, sd, from, to, sdate));

            var stream = new MemoryStream();
            _creator.Generate(data, stream);
            stream.Position = 0;

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(stream);
            httpResponseMessage.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") {FileName = "Operator Productivity Report.xlsx"};
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        /// <summary>
        /// Gets all websites.
        /// </summary>
        /// <returns></returns>
        [Route("websites")]
        public IEnumerable<string> GetAllWebsites()
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            var result = _repository.GetAllWebsites().ToList();
            result.Insert(0,"ALLWEBSITES"); //per user requirements

            return result;
        }

        /// <summary>
        /// Gets all devices.
        /// </summary>
        /// <returns></returns>
        [Route("devices")]
        public IEnumerable<string> GetAllDevices()
        {
            //TODO: roll in ORM like EF or Dapper for Db access
            var result = _repository.GetAllDevices().ToList();
            result.Insert(0,"ALLDEVICES"); //per user requirements

            return result;
        }
    }
}
