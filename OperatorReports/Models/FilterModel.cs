using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperatorReports.Models
{
    /// <summary>
    /// FilterModel
    /// </summary>
    public class FilterModel
    {
        /// <summary>
        /// Gets or sets the selected website.
        /// </summary>
        /// <value>
        /// The selected website.
        /// </value>
        public string SelectedWebsite { get; set; }
        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        /// <value>
        /// The selected device.
        /// </value>
        public string SelectedDevice { get; set; }
        /// <summary>
        /// Gets or sets the selected date from pre-defined date filter
        /// </summary>
        /// <value>
        /// The selected date.
        /// </value>
        public string SelectedDate { get; set; }
        /// <summary>
        /// Gets or sets the selected from date.
        /// </summary>
        /// <value>
        /// The selected from date from custom date filter
        /// </value>
        public string SelectedFromDate { get; set; }
        /// <summary>
        /// Gets or sets the selected to date from custom date filter
        /// </summary>
        /// <value>
        /// The selected to date.
        /// </value>
        public string SelectedToDate { get; set; }
    }
}