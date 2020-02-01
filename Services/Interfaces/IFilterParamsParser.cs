using BusinessEntity;

namespace Services.Interfaces
{
    /// <summary>
    /// IFilterParamsParser
    /// </summary>
    public interface IFilterParamsParser
    {
        /// <summary>
        /// Parses the specified sw.
        /// </summary>
        /// <param name="sw">The sw.</param>
        /// <param name="sd">The sd.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="sdate">The sdate.</param>
        /// <returns></returns>
        ReportFilter Parse(string sw, string sd, string from, string to, string sdate);
    }
}