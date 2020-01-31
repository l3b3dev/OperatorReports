using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OperatorReports.Models;

namespace OperatorReports.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<OperatorReportViewModel> GetReports()
        {
            var result = new List<OperatorReportViewModel>();

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
                           
                            result.Add(opVM);
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
