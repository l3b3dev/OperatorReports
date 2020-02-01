using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OperatorReports.Models;

namespace OperatorReports.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        // GET api/values
        [Route("")]
        public IEnumerable<OperatorReportViewModel> GetReports(string sw=null, string sd=null, string from=null, string to=null, string sdate=null)
        {
            var result = new List<OperatorReportViewModel>();

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
            using (var conn =
                    new SqlConnection(
                        "Data Source=PHOENIX\\SQLEXPRESS;Initial Catalog=chat;User id=chat;Password=chat;") //TODO: move sensitive info like connection strings out
            )
            {
                conn.Open();

                using (var sqlcomm =
                    new SqlCommand("dbo.OperatorProductivity ", conn))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;

                    sqlcomm.Parameters.Add("@SelWeb", SqlDbType.VarChar).Value = sw;
                    sqlcomm.Parameters.Add("@SelDev", SqlDbType.VarChar).Value = sd;
                    sqlcomm.Parameters.Add("@From", SqlDbType.DateTime).Value = dtFrom;
                    sqlcomm.Parameters.Add("@To", SqlDbType.DateTime).Value = dtTo;

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
                           
                            result.Add(opVM);
                        }
                    }
                }
            }

            return result;
        }

        [Route("websites")]
        public IEnumerable<string> GetAllWebsites()
        {
            var result = new List<string>();

            //TODO: roll in ORM like EF or Dapper for Db access
            using (var conn =
                    new SqlConnection(
                        "Data Source=PHOENIX\\SQLEXPRESS;Initial Catalog=chat;User id=chat;Password=chat;") //TODO: move sensitive info like connection strings out
            )
            {
                conn.Open();

                using (var sqlcomm =
                    new SqlCommand("exec dbo.GetDistinctWebsite ", conn))
                {
                    using (var dr = sqlcomm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(Convert.ToString(dr[0]));
                        }
                    }
                }
            }

            return result;
        }

        [Route("devices")]
        public IEnumerable<string> GetAllDevices()
        {
            var result = new List<string>();

            //TODO: roll in ORM like EF or Dapper for Db access
            using (var conn =
                    new SqlConnection(
                        "Data Source=PHOENIX\\SQLEXPRESS;Initial Catalog=chat;User id=chat;Password=chat;") //TODO: move sensitive info like connection strings out
            )
            {
                conn.Open();

                using (var sqlcomm =
                    new SqlCommand("exec dbo.GetDistinctDevice ", conn))
                {
                    using (var dr = sqlcomm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(Convert.ToString(dr[0]));
                        }
                    }
                }
            }

            return result;
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
