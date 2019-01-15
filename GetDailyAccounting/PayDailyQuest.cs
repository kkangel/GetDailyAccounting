using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDailyAccounting
{
    public class PayDailyQuest
    {
        public string certiTypeCode { get; set; }
        public int timeType { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string systemCode { get; set; }
        public string loginComcode { get; set; }
        public string operatorCode { get; set; }
        public string operatorName { get; set; }
    }
}
