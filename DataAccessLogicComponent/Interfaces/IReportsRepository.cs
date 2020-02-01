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
        /// <param name="sw">The sw.</param>
        /// <param name="sd">The sd.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        IEnumerable<OperatorReport> GetReports(string sw = null, string sd = null, DateTime? from = null,
            DateTime? to = null);
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