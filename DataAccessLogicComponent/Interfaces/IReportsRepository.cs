using System;
using System.Collections.Generic;
using BusinessEntity;

namespace DataAccessLogicComponent.Interfaces
{
    /// <summary>
    /// IReportsRepository
    /// </summary>
    public interface IReportsRepository
    {
        /// <summary>
        /// Gets the reports.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<OperatorReport> GetReports(ReportFilter filter);
        /// <summary>
        /// Gets all websites.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllWebsites();
        /// <summary>
        /// Gets all devices.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllDevices();
    }
}