using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GetDailyAccounting
{
    
    public class PayDailyAccount 
    {
	public string certiNO  { get; set; }//业务单号
	public string payRefReason  { get; set; }//费用类型 R10:签单收保费 R72:代收应缴车船税 V10:保费销项税
        public string payNo  { get; set; }//缴费期数
	public string totalPayNo  { get; set; }//总缴费期数
	public string recCurrency  { get; set; }//应收币种
	public string planFee  { get; set; }//应收金额
	public string payCurrency  { get; set; }//实收币种
	public string payRefFee  { get; set; }//实收金额
	public string transactEndDate  { get; set; }//缴费成功日期
	public string startDate  { get; set; }//起保日期
	public string productCode  { get; set; }//险种
	public string businessTypeCode  { get; set; }//业务渠道  03:互动 04:电子 05:非车/重客/综拓 06:车商 07:银保 08:个代 09:普通中介 10:客户自助
        public string appliName  { get; set; }//投保人
	public string prpallOperatorName  { get; set; } //出单员
	public string comcode  { get; set; }//归属机构
	public string collStatus  { get; set; }//缴费状态 0:缴费中 1:缴费成功 2:部分缴费成功 3:取消缴费 4:退费中 5:已退费
        public string policyStatus  { get; set; }//保单状态
	public string payRefStatus  { get; set; }//实收状态  0:未实收 1:已实收
	public string collectionNumber  { get; set; }//缴费唯一标识码
	public string payRefTime  { get; set; }//确认收付时间
	public string payWayCode  { get; set; }//支付方式 01:刷卡 02:扫码 03:网管 04:快捷 05:B2B 06:分期 07:支票 08:汇票 09:转账 10:预约协议 11:暂收款 12:电话支付 13:线下支付 BPCFZJ:非资金核销
        public string certiTypeCode  { get; set; }//ALL:全部 P:保单 T:投保单 E:批单 R:批改申请单
	public string centerCode  { get; set; }//核算单位
	public string branchCode  { get; set; }//成本中心
	public string seeFeeStatus  { get; set; }//见费出单标志 0:非见费出单 1:见费出单
	public string collBusinessTypeCode  { get; set; } //缴费状态 1:缴费成功 2:部分缴费成功 3:取消缴费 4:退费中 5:已退费 
	public string systemCode  { get; set; }//系统类型 GSSYT 国寿收银台
}
    public class PayDailyAccountResponseData
    {
        [JsonProperty(PropertyName = "Data$")]
        public List<PayDailyAccount> dailyAccount { get; set; }
        [JsonProperty(PropertyName = "entityCount$")]
        public string total  { get; set; } //总条数
        [JsonProperty(PropertyName = "limit")]
        public string limit { get; set; }//单次循环条数
        [JsonProperty(PropertyName = "from")]
        public string startNum { get; set; } //起始条数
    }
}
