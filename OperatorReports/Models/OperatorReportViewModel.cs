using System.Collections.Generic;

namespace OperatorReports.Models
{
    public class OperatorReportViewModel
    {
        public int ID { get; set; }
        public string Name { get; set;}
        public int ProactiveSent { get; set; }
        public int ProactiveAnswered { get; set; }
        public int ProactiveResponseRate { get; set; }
        public int ReactiveReceived { get; set; }
        public int ReactiveAnswered { get; set; }
        public int ReactiveResponseRate { get; set; }
        public string TotalChatLength { get; set; }
        public string AverageChatLength { get; set; }
    }

    public class OperatorReportItems
    {
        public ICollection<OperatorReportViewModel> OperatorProductivity { get; set; }
    }
}