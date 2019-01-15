using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   

namespace GetDailyAccounting
{
    public class PayRefAccount
    {
    public string payrefstatus { get; set; }
    public string appliname { get; set; }
    public string enddate { get; set; }
    public string underwritedate { get; set; }
    public string certino { get; set; }
    public string payrefdate { get; set; }
    public string policyno { get; set; }
    public string productcode { get; set; }
    public string comcode { get; set; }
    public string channelTypeCode { get; set; }
    public string reccurrency { get; set; }
    public string planfeeincludingtax { get; set; }
    public string planfee { get; set; }
    public string taxfee { get; set; }
    public string agentname { get; set; }
    public string insuredname { get; set; }
    public string mainsalesmanname { get; set; }
    public string startdate { get; set; }
    public string licenseno { get; set; }
    public string businessNatureCode { get; set; }
    }

    public class PayRefAccountResponseData
{
    [JsonProperty(PropertyName = "from")]
    public string startNum { get; set; }//起始条数

    [JsonProperty(PropertyName = "Data$")]
    public List<PayRefAccount> refAccount { get; set; }
    [JsonProperty(PropertyName = "entityCount$")]
        public string total  { get; set; }//总条数
    [JsonProperty(PropertyName = "limit")]
        public string limit { get; set; }//单次循环条数
    }
}
