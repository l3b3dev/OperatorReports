using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
using DataAccessLogicComponent.Interfaces;

namespace DataAccessLogicComponent
{
    /// <summary>
    /// ReportsRepository
    /// </summary>
    /// <seealso cref="DataAccessLogicComponent.Interfaces.IReportsRepository" />
    public class ReportsRepository : IReportsRepository
    {
        public IEnumerable<OperatorReport> GetReports(string sw = null, string sd = null, DateTime? from = null,
            DateTime? to = null)
        {
            var result = new List<OperatorReport>();

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
                    sqlcomm.Parameters.Add("@From", SqlDbType.DateTime).Value = from;
                    sqlcomm.Parameters.Add("@To", SqlDbType.DateTime).Value = to;

                    using (var dr = sqlcomm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var opVM = new OperatorReport
                            {
                                Id = Convert.ToInt32(dr[0]),
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

        /// <summary>
        /// Gets all websites.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all devices.
        /// </summary>
        /// <returns></returns>
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
    }
}
