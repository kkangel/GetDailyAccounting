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

        public void GetUserInfo()
        {
            HttpHelper httphelper = new HttpHelper();
            HttpResult httpresult = new HttpResult();
            HttpItem hiGetList = new HttpItem();
            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/findUserIdBySession";
            //hiGetList.URL = "http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from=0&limit=100";
            //hiGetList.Postdata= "{\"certiTypeCode\":\"\",\"timeType\":3,\"startTime\":\"2019-01-01\",\"endTime\":\"2019-01-09\",\"systemCode\":\"bpc\",\"loginComcode\":\"41019400\",\"operatorCode\":\"410105198709140028\",\"operatorName\":\"妯婅崳\"}";
            //hiGetList.ContentType = "application/json";
            //hiGetList.Method = "post";

            hiGetList.Cookie = CookieAllTheWay;

            httpresult = httphelper.GetHtml(hiGetList);
             sessionInfoByUserId =
                JsonConvert.DeserializeObject<SessionInfoByUserId>(httpresult.Html);

            //userComCode=a.loginStructure;
            //userInfo = JsonConvert.DeserializeObject<SessionInformation>(httpresult.Html);
        }
        public void GetList(string startTime,string endTime)
        {
            
            HttpHelper httphelper = new HttpHelper();
            HttpResult httpresult= new HttpResult();
            HttpItem hiGetList=new HttpItem();
            hiGetList.URL = "http://9.0.9.11/workbench/workbench/index.html";
            hiGetList.Cookie = CookieAllTheWay;
            httpresult = httphelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/findLoginStructures";
            hiGetList.Cookie = CookieAllTheWay;
            httpresult = httphelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/getCurrentUserInfo";
            hiGetList.Cookie = CookieAllTheWay;
            httpresult = httphelper.GetHtml(hiGetList);

            
            hiGetList.URL = "http://9.0.9.11/workbench/controller/workbench/jurisdictionmenu/getSession";
            hiGetList.Cookie = CookieAllTheWay;
            httpresult = httphelper.GetHtml(hiGetList);

            hiGetList.URL = "http://9.0.9.11/bpc/collection/colltransact/payment-list.html";
            hiGetList.Cookie = CookieAllTheWay;
            httpresult = httphelper.GetHtml(hiGetList);
            string tmpcookie= httpresult.Cookie;
            tmpcookie = Regex.Replace(tmpcookie, "path=/[,;]*", "", RegexOptions.IgnoreCase);
            tmpcookie = Regex.Replace(tmpcookie, "httponly", "", RegexOptions.IgnoreCase);
            CookieAllTheWay = CookieAllTheWay + tmpcookie;

            GetUserInfo();
            

            PayDailyQuest payQuest= new PayDailyQuest();
            payQuest.certiTypeCode = "";
            payQuest.timeType = 3;
            payQuest.startTime = startTime;
            payQuest.endTime = endTime;
            //payQuest.systemCode = sessionInfoByUserId.rbac_loginStructure.actors.Role[0].businessType;
            payQuest.systemCode = "bpc";
            payQuest.loginComcode = sessionInfoByUserId.loginStructureId;
            payQuest.operatorCode = sessionInfoByUserId.userId;
            //payQuest.operatorName = sessionInfoByUserId.userName;
            payQuest.operatorName = "樊荣";

            hiGetList.URL =
                "http://9.0.9.11/bpc/controller/bpc/collection/colltransact/payCollTransactDetail/collTransactDetailQuery?from=0&limit=100";
            hiGetList.Cookie = CookieAllTheWay;
            hiGetList.ContentType = "application/json";
            hiGetList.Method = "post";
            hiGetList.Postdata =  JsonConvert.SerializeObject(payQuest);

            httpresult = httphelper.GetHtml(hiGetList);
            Console.ReadLine();






        }
    }
}
