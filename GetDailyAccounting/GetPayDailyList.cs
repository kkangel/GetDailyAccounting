using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CsharpHttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetDailyAccounting
{
    class GetPayDailyList
    {
        //访问 http://9.0.9.11/bpc/collection/colltransact/payment-list.html 增加cookie


        public PayDailyQuest Quest  { get; set; }
        public PayRefQuest payRefQuest { get; set; }
        public string  CookieAllTheWay { get; set; }

        public Queue<PayDailyAccount> queuePayDailyAccount { get; set; }
        public SessionInfoByUserId sessionInfoByUserId { get; set; }
        public SessionInformation userInfo { get; set; }
        public PayDailyQuest payQuest { get; set; }

        private HttpHelper httpHelper;
        private HttpResult httpResult;
        private HttpItem hiGetList;
        private DateTime startTime;
        private DateTime endTime;

      public  GetPayDailyList(DateTime startDateTime,DateTime endDateTime )
        {
            httpHelper = new HttpHelper();
            httpResult = new HttpResult();
            hiGetList = new HttpItem();
            payQuest = new PayDailyQuest();
             payRefQuest = new PayRefQuest();
            queuePayDailyAccount = new Queue<PayDailyAccount>();
            this.startTime = startDateTime;
            this.endTime = endDateTime;
            sessionInfoByUserId = new SessionInfoByUserId(); 
        }

        private void GetUserInfo()
        {
           
            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/findUserIdBySession";
            //hiGetList.URL = "http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from=0&limit=100";
            //hiGetList.Postdata= "{\"certiTypeCode\":\"\",\"timeType\":3,\"startTime\":\"2019-01-01\",\"endTime\":\"2019-01-09\",\"systemCode\":\"bpc\",\"loginComcode\":\"41019400\",\"operatorCode\":\"410105198709140028\",\"operatorName\":\"妯婅崳\"}";
            //hiGetList.ContentType = "application/json";
            //hiGetList.Method = "post";

            hiGetList.Cookie = CookieAllTheWay;

            httpResult = httpHelper.GetHtml(hiGetList);
             sessionInfoByUserId =
                JsonConvert.DeserializeObject<SessionInfoByUserId>(httpResult.Html);

            //userComCode=a.loginStructure;
            //userInfo = JsonConvert.DeserializeObject<SessionInformation>(httpresult.Html);
        }

        private void GetDayliList(DateTime dateTime)
        {
            bool isGetTrueBack = false;

            payQuest.certiTypeCode = "";
            payQuest.timeType = 3;
            payQuest.startTime = dateTime.ToString("yyyy-MM-dd");
            payQuest.endTime = dateTime.ToString("yyyy-MM-dd");
            payQuest.systemCode = sessionInfoByUserId.rbac_loginStructure.actors.Role[0].businessType.ToLower();
            //payQuest.systemCode = "bpc";
            payQuest.loginComcode = sessionInfoByUserId.loginStructureId;
            payQuest.operatorCode = sessionInfoByUserId.userId;
            //payQuest.operatorName = sessionInfoByUserId.userName;
            payQuest.operatorName = System.Web.HttpUtility.UrlEncode(sessionInfoByUserId.userName, System.Text.Encoding.UTF8);

            do
            {
                hiGetList.URL =
                    "http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from=0&limit=100";
                hiGetList.Cookie = CookieAllTheWay;
                hiGetList.ContentType = "application/json";
                hiGetList.Method = "post";
                hiGetList.Postdata = JsonConvert.SerializeObject(payQuest);

                httpResult = httpHelper.GetHtml(hiGetList);
                if (!httpResult.Html.Contains("请求被中止"))
                {
                    isGetTrueBack = true;
                }
            } while (!isGetTrueBack);
            PayDailyAccountResponseData dailyResponseData = new PayDailyAccountResponseData();
            dailyResponseData = JsonConvert.DeserializeObject<PayDailyAccountResponseData>(httpResult.Html);



            Console.WriteLine("正在获取{0}的缴费清单,当日共有{1}条数据,正在获取中", dateTime.ToString("yyyy-MM-dd"),dailyResponseData.total);
            
            if (dailyResponseData.dailyAccount.Count()>0)
            {
                foreach (var dailyAccount in dailyResponseData.dailyAccount)
                {
                    queuePayDailyAccount.Enqueue(dailyAccount);
                }
                
                for (int stepNum = 100; stepNum < Int32.Parse(dailyResponseData.total);stepNum=stepNum+100)
                {
                    isGetTrueBack = false;
                    Console.WriteLine("共{0}条,正在获取{1}条", dailyResponseData.total, stepNum.ToString());

                    do
                    {
                        hiGetList.URL =
                            string.Format("http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from={0}&limit=100",stepNum.ToString());
                        hiGetList.Cookie = CookieAllTheWay;
                        hiGetList.ContentType = "application/json";
                        hiGetList.Method = "post";
                        hiGetList.Postdata = JsonConvert.SerializeObject(payQuest);

                        httpResult = httpHelper.GetHtml(hiGetList);
                        if (!httpResult.Html.Contains("请求被中止"))
                        {
                            isGetTrueBack = true;
                        }
                    } while (!isGetTrueBack);
                    dailyResponseData = JsonConvert.DeserializeObject<PayDailyAccountResponseData>(httpResult.Html);
                    foreach (var dailyAccount in dailyResponseData.dailyAccount)
                    {
                        queuePayDailyAccount.Enqueue(dailyAccount);
                    }

                   
                }


            }



        }


        public void GetList()
        {
            
            
            
            hiGetList.URL = "http://9.0.9.11/workbench/workbench/index.html";
            hiGetList.Cookie = CookieAllTheWay;
            httpResult = httpHelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/findLoginStructures";
            hiGetList.Cookie = CookieAllTheWay;
            httpResult = httpHelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/getCurrentUserInfo";
            hiGetList.Cookie = CookieAllTheWay;
            httpResult = httpHelper.GetHtml(hiGetList);

            
            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/getSession";
            hiGetList.Cookie = CookieAllTheWay;
            httpResult = httpHelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/bpc/collection/colltransact/payment-list.html";
            hiGetList.Cookie = CookieAllTheWay;
            httpResult = httpHelper.GetHtml(hiGetList);
            string tmpcookie= httpResult.Cookie;
            tmpcookie = Regex.Replace(tmpcookie, "path=/[,;]*", "", RegexOptions.IgnoreCase);
            tmpcookie = Regex.Replace(tmpcookie, "httponly", "", RegexOptions.IgnoreCase);
            CookieAllTheWay = CookieAllTheWay + tmpcookie;

            GetUserInfo();


            for (DateTime date = startTime; date <= endTime; date=date.AddDays(1))
            {
               GetDayliList(date);

            }

            string filePayName = System.AppDomain.CurrentDomain.BaseDirectory +sessionInfoByUserId.userId+"工号下的"+startTime.ToString("yyyy-MM-dd") + "到"+ endTime.ToString("yyyy-MM-dd")+"的缴费清单"+".xls";
            string fileRefName = System.AppDomain.CurrentDomain.BaseDirectory + sessionInfoByUserId.userId + "工号下的" +
                                 startTime.ToString("yyyy-MM-dd") + "到" + endTime.ToString("yyyy-MM-dd") + "的保单清单" +
                                 ".xls";

            List<PayDailyAccount> listPayDailyAccount = queuePayDailyAccount.ToList();
            List<Dictionary<string,string>> pList=new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> pRefList = new List<Dictionary<string, string>>();
            foreach (var account in listPayDailyAccount)
            {
                string json = JsonConvert.SerializeObject(account);
                Dictionary<string, string> jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                pList.Add(jsonDict);
            }

            var insureList = from p in listPayDailyAccount
                where p.payRefReason == "R10"
                select p;

            Queue<PayRefAccount> queueRefAccounts= new Queue<PayRefAccount>();
            PayRefAccountResponseData refResponseData = new PayRefAccountResponseData();

            if (insureList.Count() > 0)
            {
                //每次50
                int rotate =(int) Math.Ceiling((double)insureList.Count() / 50);
                var arrayinsure = insureList.ToArray();

                Queue<PayDailyAccount> queQueryAccounts = new Queue<PayDailyAccount>();
                foreach (var account in insureList)
                {
                    queQueryAccounts.Enqueue(account);
                }
                Console.WriteLine("正在获取保单清单,共有{0}条数据,正在获取中", insureList.Count());

                for (int i = 0; i < rotate; i++)
                {
                    string strcertinolist = "";
                    Console.WriteLine("共{0}条,正在获取{1}条", insureList.Count(),((i+1)*50).ToString());
                    for (int j = 0; j < 50; j++)
                    {
                        int x = i * 50 + j;
                        if (x < insureList.Count())
                        {
                            strcertinolist = strcertinolist + arrayinsure[x].certiNO + "\n";
                        }
                       
                        
                    }
                    
                    payRefQuest.certinolist = strcertinolist;
                    payRefQuest.certitypecode = "P";
                    payRefQuest.payrefstatus = "ALL";
                    payRefQuest.querytype = "1";

                    hiGetList.URL =
                        "http://9.0.9.11/bpc/controller/bpc/multiple/voucherComprehensiveController/policyPayment?from=0&limit=100";
                    hiGetList.Cookie = CookieAllTheWay;
                    hiGetList.ContentType = "application/json";
                    hiGetList.Method = "post";
                    hiGetList.Postdata = JsonConvert.SerializeObject(payRefQuest);
                    httpResult = httpHelper.GetHtml(hiGetList);

                    
                    refResponseData = JsonConvert.DeserializeObject<PayRefAccountResponseData>(httpResult.Html);
                    foreach (var dailyAccount in refResponseData.refAccount)
                    {
                        queueRefAccounts.Enqueue(dailyAccount);
                    }

                }
                List<PayRefAccount> listPayRefAccounts = queueRefAccounts.ToList();
                
                foreach (var account in listPayRefAccounts)
                {
                    string json = JsonConvert.SerializeObject(account);
                    Dictionary<string, string> jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    pRefList.Add(jsonDict);
                }






            }



            ExcelGo.JsonToExcel(pRefList, fileRefName);
            ExcelGo.JsonToExcel(pList, filePayName);













          






        }
    }
}
