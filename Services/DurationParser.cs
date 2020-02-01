using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// DurationParser - Interpret TotalChatDuration per user requirements
    /// </summary>
    /// <seealso cref="Services.Interfaces.IDurationParser" />
    public class DurationParser : IDurationParser
    {
        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public string Parse(string duration)
        {
            //duration in mins
            if (string.IsNullOrEmpty(duration))
                return "0";
            if (!double.TryParse(duration, out var durResult))
                return "0";
            if (durResult <= 1)
                return $"{ Math.Round(durResult * 60,0)}s";

            var span = TimeSpan.FromMinutes(durResult);
            if (durResult > 1 && durResult <= 60)
                return span.ToString(@"m\m\ ss\s");
            if (durResult > 60 && durResult <= 24 * 60)
                return span.ToString(@"h\h\ m\m\ ss\s");

            //otherwise should be in days/hours/minutes/seconds
            return span.ToString(@"d\d\ h\h\ m\m\ ss\s");
        }
    }
}
