using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    /// <summary>
    /// ReportFilter
    /// </summary>
    public class ReportFilter
    {
        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        /// <value>
        /// The selected device.
        /// </value>
        public string SelectedDevice { get; set; }
        /// <summary>
        /// Gets or sets the selected website.
        /// </summary>
        /// <value>
        /// The selected website.
        /// </value>
        public string SelectedWebsite { get; set; }
        /// <summary>
        /// Gets or sets the selected from date.
        /// </summary>
        /// <value>
        /// The selected from date.
        /// </value>
        public DateTime? SelectedFromDate { get; set; }
        /// <summary>
        /// Gets or sets the selected to date.
        /// </summary>
        /// <value>
        /// The selected to date.
        /// </value>
        public DateTime? SelectedToDate { get; set; }
    }
}
