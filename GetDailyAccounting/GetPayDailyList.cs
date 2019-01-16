using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CsharpHttpHelper;
using Newtonsoft.Json;

namespace GetDailyAccounting
{
    class GetPayDailyList
    {
        //访问 http://9.0.9.11/bpc/collection/colltransact/payment-list.html 增加cookie


        public PayDailyQuest Quest  { get; set; }
        public string  CookieAllTheWay { get; set; }
        
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
            hiGetList.URL =
                "http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from=0&limit=100";
            hiGetList.Cookie = CookieAllTheWay;
            hiGetList.ContentType = "application/json";
            hiGetList.Method = "post";
            hiGetList.Postdata = JsonConvert.SerializeObject(payQuest);

            httpResult = httpHelper.GetHtml(hiGetList);

            PayDailyAccountResponseData dailyResponseData = new PayDailyAccountResponseData();
            dailyResponseData = JsonConvert.DeserializeObject<PayDailyAccountResponseData>(httpResult.Html);


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

            for(DateTime date=startTime;date<=endTime;date.AddDays(1))
            { }
            




            

            


            


            Console.ReadLine();






        }
    }
}
