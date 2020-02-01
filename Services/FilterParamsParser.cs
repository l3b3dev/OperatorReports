using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
using DataAccessLogicComponent.Interfaces;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// FilterParamsParser - scrubs params to guard against UI sql-injections
    /// </summary>
    /// <seealso cref="Services.Interfaces.IFilterParamsParser" />
    public class FilterParamsParser : IFilterParamsParser
    {
        private readonly IReportsRepository _reportsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterParamsParser"/> class.
        /// </summary>
        /// <param name="reportsRepository">The reports repository.</param>
        public FilterParamsParser(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        /// <summary>
        /// Parses the specified sw.
        /// </summary>
        /// <param name="sw">The sw.</param>
        /// <param name="sd">The sd.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="sdate">The sdate.</param>
        /// <returns></returns>
        public ReportFilter Parse(string sw, string sd, string from, string to, string sdate)
        {
            if (sw == "ALLWEBSITES") sw = null;
            if (sd == "ALLDEVICES") sd = null;

            //make sure selected website matches one from db before sending it to db
            if (sw != null && _reportsRepository.GetAllWebsites().All(w => w != sw)) sw = null;
            //make sure selected device matches one from db before sending it to db
            if (sd != null && _reportsRepository.GetAllDevices().All(d => d != sd)) sd = null;

            //parse params
            DateTime? dtFrom = null, dtTo = null;
            if (!string.IsNullOrEmpty(sdate))
            {
                //Per user requirements
                switch (sdate)
                {
                    case "Today":
                        dtFrom = DateTime.Today;
                        dtTo = DateTime.Today.AddDays(1);
                        break;
                    case "Yesterday":
                        dtFrom = DateTime.Today.AddDays(-1);
                        dtTo = DateTime.Today;
                        break;
                    case "ThisWeek":
                        dtFrom = DateTime.Today.AddDays(-1 * (int) (DateTime.Today.DayOfWeek));
                        dtTo = dtFrom.Value.AddDays(7);
                        break;
                    case "LastWeek":
                        dtFrom = DateTime.Today.AddDays(-1 * (int) (DateTime.Today.DayOfWeek)).AddDays(-7);
                        dtTo = dtFrom.Value.AddDays(7);
                        break;
                    case "ThisMonth":
                        dtFrom = DateTime.Today.AddDays(-1 * DateTime.Today.Day);
                        dtTo = dtFrom.Value.AddMonths(1);
                        break;
                    case "LastMonth":
                        dtFrom = DateTime.Today.AddDays(-1 * DateTime.Today.Day).AddMonths(-1);
                        dtTo = dtFrom.Value.AddMonths(1);
                        break;
                    case "ThisYear":
                        dtFrom = new DateTime(DateTime.Now.Year, 1, 1);
                        dtTo = new DateTime(DateTime.Now.Year, 12, 31);
                        break;
                    case "LastYear":
                        dtFrom = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
                        dtTo = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 31);
                        break;
                }
            }
            else
            {
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
            }

            return new ReportFilter
            {
                SelectedDevice = sd,
                SelectedWebsite = sw,
                SelectedToDate = dtTo,
                SelectedFromDate = dtFrom
            };
        }
    }
}
