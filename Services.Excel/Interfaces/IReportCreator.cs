using System.Collections.Generic;
using System.IO;
using BusinessEntity;

namespace Services.Excel.Interfaces
{
    /// <summary>
    /// IReportCreator
    /// </summary>
    public interface IReportCreator
    {
        /// <summary>
        /// Generates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void Generate(IEnumerable<OperatorReport> payload, string fileName);

        /// <summary>
        /// Generates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="stream"></param>
        /// <returns></returns>
        void Generate(IEnumerable<OperatorReport> payload, MemoryStream stream);
    }
}